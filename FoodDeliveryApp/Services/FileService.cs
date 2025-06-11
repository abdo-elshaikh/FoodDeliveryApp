using Microsoft.AspNetCore.Hosting;
using FoodDeliveryApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileService> _logger;
        private readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);
        private readonly string _uploadDirectory;

        public FileService(IWebHostEnvironment webHostEnvironment, ILogger<FileService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _uploadDirectory = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
            
            // Ensure upload directory exists
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public async Task<(string, string)> SaveFileAsync(IFormFile file, string subDirectory = "")
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("Attempted to save null or empty file");
                    throw new ArgumentException("File is null or empty", nameof(file));
                }

                // Validate file size (e.g., max 5MB)
                if (file.Length > 5 * 1024 * 1024)
                {
                    _logger.LogWarning("File size exceeds maximum allowed size of 5MB");
                    throw new ArgumentException("File size exceeds maximum allowed size of 5MB", nameof(file));
                }

                // Create subdirectory if specified
                string targetDirectory = _uploadDirectory;
                if (!string.IsNullOrWhiteSpace(subDirectory))
                {
                    targetDirectory = Path.Combine(_uploadDirectory, subDirectory);
                    if (!Directory.Exists(targetDirectory))
                    {
                        Directory.CreateDirectory(targetDirectory);
                    }
                }

                // check if file already exists with the same name
                string fileName = Path.GetFileName(file.FileName);
                string existingFilePath = Path.Combine(targetDirectory, fileName);
                if (File.Exists(existingFilePath))
                {
                    _logger.LogWarning("A file with the same name already exists: {FilePath}", existingFilePath);
                    return (fileName, existingFilePath);
                }

                // Generate unique filename
                string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                string filePath = Path.Combine(targetDirectory, uniqueFileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _logger.LogInformation("File saved successfully: {FilePath}", filePath);

                return (uniqueFileName, filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving file {FileName}", file?.FileName);
                throw;
            }
        }

        public async Task<byte[]> GetFileAsync(string fileName, string subDirectory = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    _logger.LogWarning("Attempted to get file with null or empty filename");
                    throw new ArgumentException("Filename cannot be null or empty", nameof(fileName));
                }

                string filePath = Path.Combine(_uploadDirectory, subDirectory, fileName);
                
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("File not found: {FilePath}", filePath);
                    throw new FileNotFoundException("File not found", filePath);
                }

                return await File.ReadAllBytesAsync(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving file {FileName}", fileName);
                throw;
            }
        }

        public Task DeleteFileAsync(string fileName, string subDirectory = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    _logger.LogWarning("Attempted to delete file with null or empty filename");
                    throw new ArgumentException("Filename cannot be null or empty", nameof(fileName));
                }

                string filePath = Path.Combine(_uploadDirectory, subDirectory, fileName);
                
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
                    return Task.CompletedTask;
                }

                File.Delete(filePath);
                _logger.LogInformation("File deleted successfully: {FilePath}", filePath);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FileName}", fileName);
                throw;
            }
        }

        public bool FileExists(string fileName, string subDirectory = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    _logger.LogWarning("Attempted to check existence of file with null or empty filename");
                    return false;
                }

                string filePath = Path.Combine(_uploadDirectory, subDirectory, fileName);
                return File.Exists(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking file existence {FileName}", fileName);
                throw;
            }
        }

        public string GetFileUrl(string fileName, string subDirectory = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    _logger.LogWarning("Attempted to get URL for file with null or empty filename");
                    throw new ArgumentException("Filename cannot be null or empty", nameof(fileName));
                }

                string relativePath = string.IsNullOrWhiteSpace(subDirectory)
                    ? $"uploads/{fileName}"
                    : $"uploads/{subDirectory}/{fileName}";

                return $"/{relativePath}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file URL for {FileName}", fileName);
                throw;
            }
        }
    }
}