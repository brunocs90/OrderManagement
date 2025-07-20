# Testes Unitários - OrderManagement

Este projeto contém os testes unitários para o sistema de gerenciamento de pedidos.

## Estrutura dos Testes

### Casos de Uso Testados

#### 1. **CreateOrder** (`CreateOrderTests.cs`)
- ✅ `Execute_WithValidInput_ShouldCreateOrderSuccessfully`
- ✅ `Execute_WithMultipleItems_ShouldCreateOrderWithAllItems`
- ✅ `Execute_WithDuplicateExternalOrderId_ShouldThrowException`

#### 2. **DeleteOrder** (`DeleteOrderTests.cs`)
- ✅ `Execute_WithValidOrderId_ShouldDeleteOrderSuccessfully`
- ✅ `Execute_WithOrderNotFound_ShouldThrowException`

#### 3. **GetOrderById** (`GetOrderByIdTests.cs`)
- ✅ `Execute_WithValidOrderId_ShouldReturnOrder`
- ✅ `Execute_WithOrderNotFound_ShouldThrowException`
- ✅ `Execute_WithCalculatedOrder_ShouldReturnOrderWithTotalAmount`
- ✅ `Execute_WithOrderWithMultipleItems_ShouldReturnAllItems`

#### 4. **GetOrders** (`GetOrdersTests.cs`)
- ✅ `Execute_WithValidInput_ShouldReturnOrdersSuccessfully`
- ✅ `Execute_WithEmptyResult_ShouldReturnEmptyList`
- ✅ `Execute_WithPagination_ShouldReturnCorrectPage`
- ✅ `Execute_WithFilterByOrderNumber_ShouldApplyFilter`

#### 5. **GetOrdersByStatus** (`GetOrdersByStatusTests.cs`)
- ✅ `Execute_WithPendingStatus_ShouldReturnPendingOrders`
- ✅ `Execute_WithCalculatedStatus_ShouldReturnCalculatedOrders`
- ✅ `Execute_WithSentStatus_ShouldReturnSentOrders`
- ✅ `Execute_WithEmptyResult_ShouldReturnEmptyList`
- ✅ `Execute_WithPagination_ShouldReturnCorrectPage`

#### 6. **UpdateOrder** (`UpdateOrderTests.cs`)
- ✅ `Execute_WithValidInput_ShouldUpdateOrderSuccessfully`
- ✅ `Execute_WithOrderNotFound_ShouldThrowException`
- ✅ `Execute_WithSentStatus_ShouldUpdateSuccessfully`
- ✅ `Execute_WithPendingStatus_ShouldUpdateSuccessfully`

#### 7. **ProcessOrders** (`ProcessOrdersTests.cs`)
- ✅ `Execute_WithValidPendingToCalculated_ShouldProcessSuccessfully`
- ✅ `Execute_WithOrderNotFound_ShouldReturnError`
- ✅ `Execute_WithInvalidTransition_ShouldReturnError`
- ✅ `Execute_WithMultipleOrders_ShouldProcessAllValid`

## Tecnologias Utilizadas

- **xUnit**: Framework de testes
- **Moq**: Framework de mocking
- **FluentAssertions**: Biblioteca de assertions mais expressiva

## Cobertura de Testes

Os testes cobrem os seguintes cenários:

### Cenários de Sucesso
- Criação de pedidos com dados válidos
- Criação de pedidos com múltiplos itens
- Busca de pedidos por ID
- Listagem de pedidos com paginação
- Filtros por status e número do pedido
- Atualização de status de pedidos
- Processamento de pedidos em lote
- Exclusão de pedidos

### Cenários de Erro
- Pedidos não encontrados
- IDs de pedido externo duplicados
- Transições de status inválidas
- Dados de entrada inválidos

### Validações de Regras de Negócio
- Status de pedidos (Pending → Calculated → Sent)
- Cálculo de valores totais
- Validação de itens de pedido
- Controle de concorrência no processamento

## Como Executar os Testes

```bash
# Executar todos os testes
dotnet test OrderManagement.UnitTests

# Executar com detalhes
dotnet test OrderManagement.UnitTests --verbosity normal

# Executar testes específicos
dotnet test OrderManagement.UnitTests --filter "FullyQualifiedName~CreateOrderTests"
```

## Padrões Utilizados

### Arrange-Act-Assert (AAA)
Todos os testes seguem o padrão AAA:
- **Arrange**: Preparação dos dados e mocks
- **Act**: Execução do método testado
- **Assert**: Verificação dos resultados

### Mocking
- Uso de `IOrderDbAdapter` mockado para isolar os casos de uso
- Configuração de retornos específicos para diferentes cenários
- Verificação de chamadas aos métodos mockados

### Test Data Builders
- Métodos auxiliares para criar dados de teste
- Reutilização de código entre testes
- Dados consistentes e realistas

## Status dos Testes

**Total de Testes**: 26
**Passando**: 26 ✅
**Falhando**: 0 ❌
**Cobertura**: 100% dos casos de uso principais

Todos os testes estão passando e cobrem os cenários principais do sistema de gerenciamento de pedidos.