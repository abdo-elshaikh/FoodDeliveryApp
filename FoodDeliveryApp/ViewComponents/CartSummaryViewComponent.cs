using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FoodDeliveryApp.ViewModels.Cart;
using System.Threading.Tasks;

namespace FoodDeliveryApp.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CartSummaryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return View(new CartSummaryViewModel { ItemCount = 0, Total = 0m });
            }

            var cart = await _context.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.MenuItem)
                .Where(c => c.UserId == userId)
                .FirstOrDefaultAsync();

            if (cart == null || !cart.Items.Any())
            {
                return View(new CartSummaryViewModel { ItemCount = 0, Total = 0m });
            }

            var viewModel = new CartSummaryViewModel
            {
                ItemCount = cart.Items.Sum(c => c.Quantity),
                Total = cart.Items
                    .Where(c => c.MenuItem != null)
                    .Sum(c => c.Quantity * c.MenuItem.Price)
            };

            return View(viewModel);
        }
    }
}
