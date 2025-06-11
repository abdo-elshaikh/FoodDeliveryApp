using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext context, ILogger<CartItemRepository> logger)
            : base(context, logger)
        {
        }

        // get by cart id
        public async Task<List<CartItem>> GetByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.MenuItem)
                    .ThenInclude(mi => mi.Restaurant)
                .ToListAsync();
        }


        // add to cart
        public async Task AddToCartAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}
