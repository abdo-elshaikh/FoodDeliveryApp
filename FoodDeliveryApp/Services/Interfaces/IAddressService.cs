using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Address;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetUserAddressesAsync(string userId);
        Task<Address> GetDefaultAddressAsync(string userId);
        Task<Address> GetAddressByIdAsync(int addressId);
        Task<Address> CreateAddressAsync(AddressCreateViewModel model, string userId);
        Task<Address> UpdateAddressAsync(int addressId, AddressEditViewModel model);
        Task<bool> DeleteAddressAsync(int addressId);
        Task<bool> SetDefaultAddressAsync(int addressId, string userId);
        bool ValidateAddress(AddressViewModel address);
    }
} 