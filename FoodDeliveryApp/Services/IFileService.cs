using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string subDirectory, string fileName = null);
        Task<byte[]> ReadFileAsync(string filePath);
        Task DeleteFileAsync(string filePath);
        string GetFileUrl(string filePath);
    }
}
