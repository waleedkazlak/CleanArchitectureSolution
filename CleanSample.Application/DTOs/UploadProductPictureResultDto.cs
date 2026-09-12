namespace CleanSample.Application.DTOs;

/// <summary>
/// Result of product picture upload.
/// </summary>
public class UploadProductPictureResultDto
{
    public string PictureUrl { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public int? ProductId { get; set; }
    public ProductDto? Product { get; set; }
}
