using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IAddressRepository: IRepository<Address>
    {
        Task<IEnumerable<Address>> GetByCustomerAsync(int customerId);
        Task SetDefaultAddressAsync(int addressId, int customerId);
        Task<Address> GetDefaultAddressAsync(int customerId);
        // GetUserAddressesAsync
        Task<IEnumerable<Address>> GetUserAddressesAsync(string userId);
    }
}
