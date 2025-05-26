using Microsoft.AspNetCore.Hosting;

namespace FoodDeliveryApp.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileService> _logger;
        private readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);
        private readonly string _uploadsDirectory;

        public FileService(IWebHostEnvironment webHostEnvironment, ILogger<FileService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _uploadsDirectory = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subDirectory, string fileName = null)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("Attempted to save null or empty file.");
                throw new ArgumentException("File cannot be null or empty.");
            }

            // Validate file type (e.g., images only)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                _logger.LogWarning("Invalid file extension: {Extension}", extension);
                throw new ArgumentException($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}");
            }

            // Generate file name if not provided
            fileName = fileName ?? $"{Guid.NewGuid()}{extension}";
            var relativePath = Path.Combine("Uploads", subDirectory, fileName).Replace("\\", "/");
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

            // Ensure directory exists
            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await _fileLock.WaitAsync();
            try
            {
                _logger.LogInformation("Saving file to {Path}", fullPath);
                using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    await file.CopyToAsync(stream);
                }
                return $"/{relativePath}";
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Error saving file to {Path}", fullPath);
                throw;
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task<byte[]> ReadFileAsync(string filePath)
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
            if (!File.Exists(fullPath))
            {
                _logger.LogWarning("File not found: {Path}", fullPath);
                throw new FileNotFoundException("File not found.", fullPath);
            }

            await _fileLock.WaitAsync();
            try
            {
                _logger.LogInformation("Reading file from {Path}", fullPath);
                using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var bytes = new byte[stream.Length];
                    await stream.ReadAsync(bytes, 0, bytes.Length);
                    return bytes;
                }
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Error reading file from {Path}", fullPath);
                throw;
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public async Task DeleteFileAsync(string filePath)
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
            if (!File.Exists(fullPath))
            {
                _logger.LogWarning("File not found for deletion: {Path}", fullPath);
                return;
            }

            await _fileLock.WaitAsync();
            try
            {
                _logger.LogInformation("Deleting file: {Path}", fullPath);
                File.Delete(fullPath);
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Error deleting file: {Path}", fullPath);
                throw;
            }
            finally
            {
                _fileLock.Release();
            }
        }

        public string GetFileUrl(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                _logger.LogWarning("File path is empty.");
                return "/images/placeholder.jpg";
            }

            var relativePath = filePath.StartsWith("/") ? filePath : $"/{filePath}";
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));
            return File.Exists(fullPath) ? relativePath : "/images/placeholder.jpg";
        }
    }
}