using Supermercado.Domain.Core;
using Supermercado.Domain.Enums;

namespace Supermercado.Domain.Entities;

public class StockMovement : Entity
{
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    public int Quantity { get; private set; }
    public StockMovementType Type { get; private set; }
    public StockMovementReason Reason { get; private set; }
    public DateTime MovementDate { get; private set; }
    public Guid UserId { get; private set; }

    protected StockMovement() { }
    
    public StockMovement(Guid productId, int quantity, StockMovementType type, StockMovementReason reason, Guid userId)
    {
        ProductId = productId;
        Quantity = quantity;
        Type = type;
        Reason = reason;
        MovementDate = DateTime.UtcNow;
        UserId = userId;
    }
}
