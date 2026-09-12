using CleanSample.Domain.Enums;

namespace CleanSample.Domain.Entities;

public class Order : BaseEntity<long>
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? RequiredDate { get; set; }
    public int Status { get; set; } = (int)OrderStatusEnum.Draft;
    public OrderStatus? OrderStatus { get; set; }
    public string? Notes { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}
