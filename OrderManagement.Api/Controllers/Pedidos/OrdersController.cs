using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Commons.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Interfaces;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.Api.Controllers.Pedidos;

[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/orders")]
public class OrdersController(
    IGetOrders getOrders,
    IGetOrderById getOrderById,
    ICreateOrder createOrder,
    IDeleteOrder deleteOrder,
    IUpdateOrder updateOrder,
    IProcessOrders processOrders,
    IGetOrdersByStatus getOrdersByStatus) : ApiControllerBase
{
    private readonly IGetOrders getOrders = getOrders ?? throw new ArgumentNullException(nameof(getOrders));
    private readonly IGetOrderById getOrderById = getOrderById ?? throw new ArgumentNullException(nameof(getOrderById));
    private readonly ICreateOrder createOrder = createOrder ?? throw new ArgumentNullException(nameof(createOrder));
    private readonly IDeleteOrder deleteOrder = deleteOrder ?? throw new ArgumentNullException(nameof(deleteOrder));
    private readonly IUpdateOrder updateOrder = updateOrder ?? throw new ArgumentNullException(nameof(updateOrder));
    private readonly IProcessOrders processOrders = processOrders ?? throw new ArgumentNullException(nameof(processOrders));
    private readonly IGetOrdersByStatus getOrdersByStatus = getOrdersByStatus ?? throw new ArgumentNullException(nameof(getOrdersByStatus));

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrderByIdOutput))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(GetOrderByIdOutput))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        var order = await getOrderById.Execute(id);
        return Ok(order);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedListDto<GetOrdersOutput>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetOrders([FromQuery] GetOrdersInput input)
    {
        var result = await getOrders.Execute(input);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        await deleteOrder.Execute(id);
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateOrderOutput))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateOrder(CreateOrderInput input)
    {
        var result = await createOrder.Execute(input);
        return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateOrderOutput))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateOrder(Guid id, UpdateOrderInput input)
    {
        var updated = await updateOrder.Execute(id, input);
        return Ok(updated);
    }

    [HttpPost("process")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProcessOrdersOutput))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ProcessOrders(ProcessOrdersInput input)
    {
        var result = await processOrders.Execute(input);
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedListDto<GetOrdersOutput>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetOrdersByStatus(OrderStatus status, [FromQuery] GetOrdersByStatusInput input)
    {
        input.Status = status;
        var result = await getOrdersByStatus.Execute(input);
        return Ok(result);
    }
}