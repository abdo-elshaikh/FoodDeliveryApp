using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IAddressRepository: IRepository<Address>
    {
        Task SetDefaultAddressAsync(int addressId, string userId);
        Task<Address> GetDefaultAddressAsync(string userId);
        Task<IEnumerable<Address>> GetUserAddressesAsync(string userId);
    }
}
