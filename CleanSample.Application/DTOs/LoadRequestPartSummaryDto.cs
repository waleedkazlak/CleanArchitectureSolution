namespace CleanSample.Application.DTOs;

/// <summary>
/// Summary DTO for LoadRequestPart containing essential columns
/// </summary>
public class LoadRequestPartSummaryDto
{
    public long LoadRequestId { get; set; }
    public int ProductId { get; set; }
    public int PartId { get; set; }
    public decimal RequiredQuantity { get; set; }
}
