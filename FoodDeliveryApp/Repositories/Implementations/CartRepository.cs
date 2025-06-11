using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace FoodDeliveryApp.Repositories.Implementations
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ICurrentUserService _currentUserService;
        protected new readonly ApplicationDbContext _context;
        protected new readonly ILogger<Repository<Cart>> _logger;

        public CartRepository(ApplicationDbContext context, ILogger<Repository<Cart>> logger, ICurrentUserService currentUserService) : base(context, logger)
        {
            _context = context?? throw new ArgumentNullException(nameof(context));
            _logger = logger?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<Cart> CreateUserCartAsync(string userId)
        {
            try
            {
                // Check if user exists in the database
                var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
                if (!userExists)
                {
                    throw new InvalidOperationException($"User with ID {userId} does not exist.");
                }

                var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
                if (existingCart != null)
                {
                    return existingCart;
                }

                var newCart = new Cart()
                {
                    UserId = userId,
                    Items = new List<CartItem>(),
                    DeliveryFee = 0,
                    TaxRate = 0,
                    Status = CartStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };

                await _context.Carts.AddAsync(newCart);
                await _context.SaveChangesAsync();
                return newCart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating cart for user {UserId}", userId);
                throw;
            }
        }


        public async Task<Cart> GetByUserIdAsync(string userId)
        {
            try
            {
                var cart = await _context.Carts
                   .Include(c => c.Items)
                   .ThenInclude(ci => ci.MenuItem)
                   .ThenInclude(mi => mi.Restaurant)
                   .FirstOrDefaultAsync(c => c.UserId == userId);

                cart ??= await CreateUserCartAsync(userId);
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving cart by userId {UserId}", userId);
                throw;
            }
        }

        public async Task ClearCartItemsAsync(int cartId)
        {
            try
            {
                var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
                if (cart != null)
                {
                    cart.Items.Clear();
                    cart.DeliveryFee = 0;
                    cart.TaxRate = 0;
                    cart.TotalWithDiscount = 0;
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while clearing cart items");
                throw;
            }
        }

        public async Task<Cart> RemoveItemAsync(int cartId, int cartItemId)
        {
            try
            {
                var cartItem = await _context.CartItems
                   .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.MenuItemId == cartItemId);
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
                return await GetByIdAsync(cartId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while removing item from cart");
                throw;
            }
        }

        public async Task<Cart> UpdateCartItemQuantityAsync(string userId, int cartItemId, int quantity)
        {
            try
            {
                var cartItem = await _context.CartItems
                   .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.MenuItemId == cartItemId);
                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;
                    await _context.SaveChangesAsync();
                }
                return await GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating cart item quantity");
                throw;
            }
        }

        public async Task<Cart> AddItemToCartAsync(string userId, MenuItem menuItem, int quantity)
        {
            try
            {
                var cart = await GetByUserIdAsync(userId);
                cart ??= await CreateUserCartAsync(userId);

                var existingCartItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.MenuItemId == menuItem.Id);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    var cartItem = new CartItem()
                    {
                        CartId = cart.Id,
                        MenuItemId = menuItem.Id,
                        RestaurantId = menuItem.RestaurantId,
                        Quantity = quantity,
                        SpecialInstructions = ""
                    };
                    await _context.CartItems.AddAsync(cartItem);
                }

                cart.TotalWithDiscount = cart.Items.Sum(ci => ci.MenuItem.Price * ci.Quantity);
                cart.DeliveryFee = cart.Items.Sum(ci => ci.MenuItem.Price * ci.Quantity) * cart.TaxRate / 100;
                

                await _context.SaveChangesAsync();
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding item to cart");
                throw;
            }
        }

        public async Task<Cart> RemoveItemFromCartAsync(string userId, int cartItemId)
        {
            try
            {
                var cartItem = await _context.CartItems
                   .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.MenuItemId == cartItemId);
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
                return await GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while removing item from cart");
                throw;
            }
        }
    }
}
