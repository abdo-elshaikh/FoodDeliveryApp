using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        protected new readonly ApplicationDbContext _context;
        public AddressRepository(ApplicationDbContext context, ILogger<Repository<Address>> logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task SetDefaultAddressAsync(int addressId, string userId)
        {
            // Reset all defaults first
            var currentDefaults = await _context.Addresses
                .Where(a => a.UserId == userId && a.IsDefault)
                .ToListAsync();

            foreach (var address in currentDefaults)
            {
                address.IsDefault = false;
                _context.Update(address);
            }

            // Set new default
            var newDefault = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);

            if (newDefault != null)
            {
                newDefault.IsDefault = true;
                _context.Update(newDefault);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Address> GetDefaultAddressAsync(string userId)
            => await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);

        public async Task<IEnumerable<Address>> GetUserAddressesAsync(string userId)
        {
            var addresses = await _context.Addresses
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            if (addresses == null || !addresses.Any())
            {
                return new List<Address>();
            }
            return addresses.OrderByDescending(a => a.IsDefault).ThenBy(a => a.AddressType).ToList();
        }
    }
}