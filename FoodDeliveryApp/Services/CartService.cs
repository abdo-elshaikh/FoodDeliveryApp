using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Cart;
using Microsoft.EntityFrameworkCore;


namespace FoodDeliveryApp.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            return cart?.Items.Sum(i => i.Quantity) ?? 0;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.MenuItem)
                .ThenInclude(m => m.Restaurant)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddItemToCartAsync(string userId, CartItemViewModel cartItem)
        {
            if (cartItem.Quantity < 1 || cartItem.Quantity > 100)
            {
                throw new ArgumentException("Quantity must be between 1 and 100");
            }

            var menuItem = await _context.MenuItems
                .Include(m => m.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == cartItem.MenuItemId && m.IsAvailable);

            if (menuItem == null)
            {
                throw new ArgumentException("Menu item not found or unavailable");
            }

            var cart = await GetCartAsync(userId);

            var existingItem = cart.Items.FirstOrDefault(i => i.MenuItemId == cartItem.MenuItemId &&
                i.Customizations.OrderBy(c => c.OptionId).ThenBy(c => c.ChoiceId).SequenceEqual(
                    cartItem.Customizations.Select(c => new Customization { OptionId = c.OptionId, ChoiceId = c.ChoiceId, Price = c.Price })
                        .OrderBy(c => c.OptionId).ThenBy(c => c.ChoiceId)));

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
                if (existingItem.Quantity > 100)
                {
                    throw new ArgumentException("Total quantity cannot exceed 100");
                }
            }
            else
            {
                var newCartItem = new CartItem
                {
                    MenuItemId = cartItem.MenuItemId,
                    Quantity = cartItem.Quantity,
                    CartId = cart.Id,
                    Customizations = cartItem.Customizations.Select(c => new Customization
                    {
                        OptionId = c.OptionId,
                        ChoiceId = c.ChoiceId,
                        Price = c.Price
                    }).ToList()
                };
                cart.Items.Add(newCartItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemQuantityAsync(string userId, int cartItemId, int quantity)
        {
            if (quantity < 1 || quantity > 100)
            {
                throw new ArgumentException("Quantity must be between 1 and 100");
            }

            var cart = await GetCartAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.Id == cartItemId);

            if (cartItem == null)
            {
                throw new ArgumentException("Cart item not found");
            }

            cartItem.Quantity = quantity;
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromCartAsync(string userId, int cartItemId)
        {
            var cart = await GetCartAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.Id == cartItemId);

            if (cartItem == null)
            {
                throw new ArgumentException("Cart item not found");
            }

            cart.Items.Remove(cartItem);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetCartAsync(userId);
            cart.Items.Clear();
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            var cart = await GetCartAsync(userId);
            return cart.Items.Sum(i => (i.MenuItem.Price + i.Customizations.Sum(c => c.Price)) * i.Quantity);
        }
    }
}