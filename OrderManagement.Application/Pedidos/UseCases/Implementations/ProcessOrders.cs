using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Interfaces;
using OrderManagement.Domain.Pedidos.Exceptions;
using OrderManagement.Domain.Pedidos.Models;
using OrderManagement.Shared;
using System.Collections.Concurrent;

namespace OrderManagement.Application.Pedidos.UseCases.Implementations;

internal class ProcessOrders(IOrderDbAdapter orderDbAdapter) : IProcessOrders
{
    private readonly IOrderDbAdapter orderDbAdapter = orderDbAdapter
        ?? throw new ArgumentNullException(nameof(orderDbAdapter));

    public async Task<ProcessOrdersOutput> Execute(ProcessOrdersInput input)
    {
        if (input is null)
            throw new CreateOrderException(CreateOrderError.UnformedData);

        ValidationHelper.Validate(input);

        var output = new ProcessOrdersOutput
        {
            TotalProcessed = input.OrderIds.Count
        };

        // Busca todos os pedidos por IDs
        var orders = await orderDbAdapter.GetByIds(input.OrderIds);
        var ordersDict = orders.ToDictionary(o => o.Id);

        await ProcessOrdersInParallel(input.OrderIds, input.TargetStatus, output, ordersDict);

        return output;
    }

    private async Task ProcessOrdersInParallel(List<Guid> orderIds, OrderStatus targetStatus, ProcessOrdersOutput output, Dictionary<Guid, Order> ordersDict)
    {
        const int maxConcurrency = 10;
        var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);

        // Usar collections thread-safe
        var errors = new ConcurrentBag<string>();
        var processedOrders = new ConcurrentBag<ProcessedOrderInfo>();
        var successCount = 0;
        var errorCount = 0;

        var tasks = orderIds.Select(async orderId =>
        {
            var result = await ProcessOrderAsync(orderId, targetStatus, semaphore, ordersDict);

            if (result.IsSuccess)
            {
                Interlocked.Increment(ref successCount);
                processedOrders.Add(result.ProcessedOrder!);
            }
            else
            {
                Interlocked.Increment(ref errorCount);
                errors.Add(result.ErrorMessage!);
            }
        });

        await Task.WhenAll(tasks);

        // Atualiza o output uma única vez
        output.SuccessCount = successCount;
        output.ErrorCount = errorCount;
        output.Errors = errors.ToList();
        output.ProcessedOrders = processedOrders.ToList();
    }

    private async Task<ProcessOrderResult> ProcessOrderAsync(Guid orderId, OrderStatus targetStatus, SemaphoreSlim semaphore, Dictionary<Guid, Order> ordersDict)
    {
        await semaphore.WaitAsync();
        try
        {
            // Validações centralizadas
            var validationResult = ValidateOrderForProcessing(orderId, targetStatus, ordersDict);
            if (!validationResult.IsValid)
            {
                return ProcessOrderResult.Failure(validationResult.ErrorMessage!);
            }

            var order = validationResult.Order!;
            var previousStatus = order.Status;

            // Processa o pedido
            await ProcessOrder(order, targetStatus);

            // Salva as alterações
            await orderDbAdapter.Save(order);

            return ProcessOrderResult.Success(new ProcessedOrderInfo
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                ExternalOrderId = order.ExternalOrderId,
                PreviousStatus = previousStatus,
                NewStatus = order.Status,
                TotalAmount = order.TotalAmount,
                ProcessedAt = DateTimeOffset.UtcNow
            });
        }
        catch (Exception ex)
        {
            return ProcessOrderResult.Failure($"Erro ao processar pedido {orderId}: {ex.Message}");
        }
        finally
        {
            semaphore.Release();
        }
    }

    private OrderValidationResult ValidateOrderForProcessing(Guid orderId, OrderStatus targetStatus, Dictionary<Guid, Order> ordersDict)
    {
        // Verifica se o pedido existe
        if (!ordersDict.TryGetValue(orderId, out var order))
        {
            return OrderValidationResult.Invalid($"Pedido não encontrado: {orderId}");
        }

        // Valida se o pedido pode ser processado
        if (!CanProcessOrder(order, targetStatus))
        {
            return OrderValidationResult.Invalid($"Pedido {order.OrderNumber} não pode ser processado: Status atual {order.Status} não permite mudança para {targetStatus}");
        }

        // Valida se tem itens
        if (order.Items.Count == 0)
        {
            return OrderValidationResult.Invalid($"Pedido {order.OrderNumber} não pode ser processado: Sem itens");
        }

        return OrderValidationResult.Valid(order);
    }

    private bool CanProcessOrder(Order order, OrderStatus targetStatus)
    {
        return order.Status switch
        {
            OrderStatus.Pending => targetStatus == OrderStatus.Calculated,     // ✅ Pending → Calculated
            OrderStatus.Calculated => targetStatus == OrderStatus.Sent,        // ✅ Calculated → Sent
            OrderStatus.Sent => false,                                         // ❌ Sent → Bloqueado
            OrderStatus.Cancelled => false,                                    // ❌ Cancelled → Bloqueado
            _ => false                                                         // ❌ Status desconhecido
        };
    }

    private async Task ProcessOrder(Order order, OrderStatus targetStatus)
    {
        // Atualiza o status
        order.Status = targetStatus;

        // Operações específicas por status
        switch (targetStatus)
        {
            case OrderStatus.Calculated:
                // Calcula o total dos itens
                order.TotalAmount = order.Items.Sum(item => item.TotalPrice);
                order.CalculatedAt = DateTimeOffset.UtcNow;
                break;
            case OrderStatus.Sent:
                // Marca timestamp de envio
                order.SentAt = DateTimeOffset.UtcNow;
                break;
        }

        // Simula processamento pesado
        await Task.Delay(100);
    }
}

// Classes auxiliares para melhor organização
public class ProcessOrderResult
{
    public bool IsSuccess { get; }
    public ProcessedOrderInfo? ProcessedOrder { get; }
    public string? ErrorMessage { get; }

    private ProcessOrderResult(bool isSuccess, ProcessedOrderInfo? processedOrder, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ProcessedOrder = processedOrder;
        ErrorMessage = errorMessage;
    }

    public static ProcessOrderResult Success(ProcessedOrderInfo processedOrder)
        => new(true, processedOrder, null);

    public static ProcessOrderResult Failure(string errorMessage)
        => new(false, null, errorMessage);
}

public class OrderValidationResult
{
    public bool IsValid { get; }
    public Order? Order { get; }
    public string? ErrorMessage { get; }

    private OrderValidationResult(bool isValid, Order? order, string? errorMessage)
    {
        IsValid = isValid;
        Order = order;
        ErrorMessage = errorMessage;
    }

    public static OrderValidationResult Valid(Order order)
        => new(true, order, null);

    public static OrderValidationResult Invalid(string errorMessage)
        => new(false, null, errorMessage);
}