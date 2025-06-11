using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface IFileService
    {
        Task<(string fileName, string filePath)> SaveFileAsync(IFormFile file, string subDirectory = "");

        Task<byte[]> GetFileAsync(string fileName, string subDirectory = "");

        Task DeleteFileAsync(string fileName, string subDirectory = "");

        bool FileExists(string fileName, string subDirectory = "");

        string GetFileUrl(string fileName, string subDirectory = "");
    }
} 