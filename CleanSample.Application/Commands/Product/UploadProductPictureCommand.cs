using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.Product;

/// <summary>
/// Command to upload a product picture file.
/// </summary>
public class UploadProductPictureCommand : IRequest<UploadProductPictureResultDto?>
{
    public int? ProductId { get; set; }
    public Stream FileStream { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string? ContentType { get; set; }
    public long Length { get; set; }
}
