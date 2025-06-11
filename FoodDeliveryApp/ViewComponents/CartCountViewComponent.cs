using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FoodDeliveryApp.Services.Interfaces;

namespace FoodDeliveryApp.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CartCountViewComponent(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _currentUserService.GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return View(0);
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            var cartCount = cart?.Items?.Count?? 0;

            return View(cartCount);
        }
    }
} 