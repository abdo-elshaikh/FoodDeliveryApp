using System.Threading.Tasks;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendOrderNotificationAsync(
            string userId,
            NotificationType type,
            string title,
            string message);
    }

    public enum NotificationType
    {
        OrderPlaced,
        OrderDelivered,
        OrderCancelled
    }

} 