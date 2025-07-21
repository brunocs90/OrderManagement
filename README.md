# OrderManagement

Sistema para gerenciamento de pedidos, com processamento em lote e controle de status.

## O que o projeto faz
- Permite criar, consultar, atualizar e processar pedidos.
- Cada pedido possui itens, valores e status controlados.
- Suporta processamento em lote e atualização de status de forma concorrente e segura.

## Regra de Status do Pedido
O pedido segue o seguinte fluxo de status:

```
Pending → Calculated → Sent
           ↘
         Cancelled
```
- **Pending**: Pedido criado, aguardando cálculo dos valores.
- **Calculated**: Valores calculados, pronto para envio.
- **Sent**: Pedido enviado.
- **Cancelled**: Pedido cancelado (não pode mais ser processado).

Transições válidas:
- Pending → Calculated
- Calculated → Sent
- Qualquer status pode ser Cancelled (exceto já Cancelled ou Sent).

## Processamento e Semáforos
O processamento em lote utiliza semáforos (`SemaphoreSlim`) para limitar a concorrência (ex: até 10 pedidos processados simultaneamente), garantindo performance e evitando sobrecarga no banco de dados.
- Cada pedido é validado e processado de forma independente e thread-safe.
- O processamento calcula valores, atualiza status e timestamps conforme a transição.
- Erros são coletados e reportados ao final do processamento.

---

> Projeto .NET para gestão de pedidos, com API REST, processamento concorrente e regras de negócio robustas.