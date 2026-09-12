using CleanSample.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanSample.Infrastructure.Services;

/// <summary>
/// Implementation of IFileStorageService that saves files to local wwwroot directory.
/// </summary>
public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<LocalFileStorageService> _logger;

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif"
    };

    public LocalFileStorageService(IWebHostEnvironment webHostEnvironment, ILogger<LocalFileStorageService> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string originalFileName, string subDirectory, CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(originalFileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
        {
            throw new ArgumentException($"File extension '{extension}' is not allowed. Allowed extensions are: {string.Join(", ", AllowedExtensions)}");
        }

        // Determine base web root path
        var rootPath = _webHostEnvironment.WebRootPath;
        if (string.IsNullOrEmpty(rootPath))
        {
            rootPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
        }

        // Normalize subDirectory (e.g. "uploads/products")
        var normalizedSubDir = subDirectory.Trim('/', '\\').Replace('/', Path.DirectorySeparatorChar);
        var targetDirectory = Path.Combine(rootPath, normalizedSubDir);

        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        // Generate unique secure filename
        var uniqueFileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var fullFilePath = Path.Combine(targetDirectory, uniqueFileName);

        using (var outputStream = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            if (fileStream.CanSeek)
            {
                fileStream.Position = 0;
            }
            await fileStream.CopyToAsync(outputStream, cancellationToken);
        }

        _logger.LogInformation("Saved file {OriginalName} to {Path}", originalFileName, fullFilePath);

        // Return relative URL format with forward slashes: /uploads/products/{uniqueFileName}
        var relativeUrl = "/" + subDirectory.Trim('/', '\\').Replace('\\', '/') + "/" + uniqueFileName;
        return relativeUrl;
    }

    public Task<bool> DeleteFileAsync(string? relativeFilePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(relativeFilePath))
        {
            return Task.FromResult(false);
        }

        try
        {
            var rootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(rootPath))
            {
                rootPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
            }

            var cleanRelativePath = relativeFilePath.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
            var fullFilePath = Path.Combine(rootPath, cleanRelativePath);

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
                _logger.LogInformation("Deleted file {Path}", fullFilePath);
                return Task.FromResult(true);
            }

            _logger.LogWarning("File not found for deletion: {Path}", fullFilePath);
            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {Path}", relativeFilePath);
            return Task.FromResult(false);
        }
    }
}
