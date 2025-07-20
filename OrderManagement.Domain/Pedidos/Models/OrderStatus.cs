namespace OrderManagement.Domain.Pedidos.Models;

public enum OrderStatus
{
    Pending,        // ⏳ Pendente - Aguardando cálculo
    Calculated,     // ✅ Calculado - Valores calculados
    Sent,          // 📤 Enviado - Enviado para produto B
    Cancelled      // ❌ Cancelado - Pedido cancelado
}