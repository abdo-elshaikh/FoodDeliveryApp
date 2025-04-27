using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        private readonly ApplicationDbContext _context;
        public AddressRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Address>> GetByCustomerAsync(int customerId)
            => await _context.Addresses
                .Where(a => a.CustomerProfileId == customerId)
                .OrderByDescending(a => a.IsDefault)
                .ThenBy(a => a.Title)
                .ToListAsync();

        public async Task SetDefaultAddressAsync(int addressId, int customerId)
        {
            // Reset all defaults first
            var currentDefaults = await _context.Addresses
                .Where(a => a.CustomerProfileId == customerId && a.IsDefault)
                .ToListAsync();

            foreach (var address in currentDefaults)
            {
                address.IsDefault = false;
                _context.Update(address);
            }

            // Set new default
            var newDefault = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId && a.CustomerProfileId == customerId);

            if (newDefault != null)
            {
                newDefault.IsDefault = true;
                _context.Update(newDefault);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Address> GetDefaultAddressAsync(int customerId)
            => await _context.Addresses
                .FirstOrDefaultAsync(a => a.CustomerProfileId == customerId && a.IsDefault);

        public async Task<IEnumerable<Address>> GetUserAddressesAsync(string userId)
        {
            var addresses = await _context.Addresses
                .Include(a => a.CustomerProfile)
                .Where(a => a.CustomerProfile.UserId == userId)
                .ToListAsync();

            if (addresses == null || !addresses.Any())
            {
                return new List<Address>();
            }
            return addresses.OrderByDescending(a => a.IsDefault).ThenBy(a => a.Title).ToList();
        }
    }
}