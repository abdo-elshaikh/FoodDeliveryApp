using System.Threading.Tasks;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task TrackPageViewAsync(string pageName, string userName);
        Task TrackEventAsync(string eventName, string userId, Dictionary<string, string> properties);
    }
}
