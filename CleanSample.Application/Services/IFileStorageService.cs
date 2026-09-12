namespace CleanSample.Application.Services;

/// <summary>
/// Service interface for handling file uploads and deletions in accordance with clean architecture.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Saves a file from a stream into the specified subdirectory and returns the relative public URL path.
    /// </summary>
    Task<string> SaveFileAsync(Stream fileStream, string originalFileName, string subDirectory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file from storage given its relative URL path.
    /// </summary>
    Task<bool> DeleteFileAsync(string? relativeFilePath, CancellationToken cancellationToken = default);
}
