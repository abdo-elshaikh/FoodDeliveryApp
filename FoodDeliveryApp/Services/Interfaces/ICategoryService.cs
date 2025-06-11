using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.Category;

namespace FoodDeliveryApp.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryViewModel>> GetPopularCategoriesAsync();
        Task<CategoryViewModel> GetCategoryByIdAsync(int id);
        Task<List<CategoryViewModel>> GetAllCategoriesAsync();
    }
} 