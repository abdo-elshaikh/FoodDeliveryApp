using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.ViewModels.Address;

namespace FoodDeliveryApp.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger<AddressService> _logger;

        public AddressService(IAddressRepository addressRepository, ILogger<AddressService> logger)
        {
            _addressRepository = addressRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Address>> GetUserAddressesAsync(string userId)
        {
            try
            {
                return await _addressRepository.GetUserAddressesAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user addresses for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Address> GetDefaultAddressAsync(string userId)
        {
            try
            {
                return await _addressRepository.GetDefaultAddressAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting default address for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            try
            {
                return await _addressRepository.GetByIdAsync(addressId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting address {AddressId}", addressId);
                throw;
            }
        }

        public async Task<Address> CreateAddressAsync(AddressCreateViewModel model, string userId)
        {
            try
            {
                var address = new Address
                {
                    StreetAddress = model.StreetAddress,
                    City = model.City,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    Country = model.Country,
                    Notes = model.Notes,
                    UserId = userId,
                    IsDefault = model.IsDefault,
                    AddressType = model.AddressType,
                    CreatedAt = DateTime.UtcNow
                };

                if (model.IsDefault)
                {
                    await _addressRepository.SetDefaultAddressAsync(address.Id, userId);
                }

                await _addressRepository.AddAsync(address);
                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating address for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Address> UpdateAddressAsync(int addressId, AddressEditViewModel model)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(addressId);
                if (address == null)
                {
                    throw new KeyNotFoundException($"Address with ID {addressId} not found");
                }

                address.StreetAddress = model.StreetAddress;
                address.City = model.City;
                address.State = model.State;
                address.PostalCode = model.PostalCode;
                address.Country = model.Country;
                address.Notes = model.Notes;
                address.IsDefault = model.IsDefault;
                address.AddressType = model.AddressType;
                address.UpdatedAt = DateTime.UtcNow;

                if (model.IsDefault)
                {
                    await _addressRepository.SetDefaultAddressAsync(addressId, address.UserId);
                }

                await _addressRepository.UpdateAsync(address);
                return address;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating address {AddressId}", addressId);
                throw;
            }
        }

        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            try
            {
                var address = await _addressRepository.GetByIdAsync(addressId);
                if (address == null)
                {
                    return false;
                }

                await _addressRepository.DeleteAsync(address);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting address {AddressId}", addressId);
                throw;
            }
        }

        public async Task<bool> SetDefaultAddressAsync(int addressId, string userId)
        {
            try
            {
                await _addressRepository.SetDefaultAddressAsync(addressId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting default address {AddressId} for user {UserId}", addressId, userId);
                throw;
            }
        }

        public bool ValidateAddress(AddressViewModel address)
        {
            try
            {
                // Basic validation
                if (string.IsNullOrWhiteSpace(address.StreetAddress) ||
                    string.IsNullOrWhiteSpace(address.City) ||
                    string.IsNullOrWhiteSpace(address.State) ||
                    string.IsNullOrWhiteSpace(address.PostalCode) ||
                    string.IsNullOrWhiteSpace(address.Country))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating address");
                throw;
            }
        }
    }
} 