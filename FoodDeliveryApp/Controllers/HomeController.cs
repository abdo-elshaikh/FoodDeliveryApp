﻿﻿﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _unitOfWork.RestaurantCategories.GetAllAsync();

                var popularSearchesEntities = await _unitOfWork.SearchHistory.GetPopularSearchesAsync();
                var popularSearches = popularSearchesEntities.Select(s => s.Query).ToList();

                var viewModel = new HomeViewModel
                {
                    Hero = new HeroSection
                    {
                        BackgroundImageUrl = "/images/hero-bg.jpg"
                    },
                    Categories = categories
                        .Select(c => new HomeCategoryVM
                        {
                            Id = c.Id,
                            Name = c.Name,
                            ImageUrl = c.ImageUrl ?? string.Empty,
                            Description = c.Description ?? string.Empty,
                            RestaurantCount = c.Restaurants?.Count ?? 0
                        }).ToList(),

                    FeaturedDishes = (await _unitOfWork.MenuItems.GetPopularDishesAsync(10))
                        .Select(d => new FeaturedDishViewModel
                        {
                            Id = d.Id,
                            Name = d.Name,
                            Description = d.Description ?? string.Empty,
                            Price = d.Price,
                            ImageUrl = d.ImageUrl ?? string.Empty,
                            RestaurantName = d.Restaurant?.Name ?? string.Empty,
                            RestaurantId = d.RestaurantId,
                            IsAvailable = d.IsAvailable
                        }).ToList(),

                    TopRatedRestaurants = (await _unitOfWork.Restaurants.GetTopRatedRestaurantsAsync(10))
                        .Select(r => new HomeRestaurantViewModel
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Description = r.Description ?? string.Empty,
                            ImageUrl = r.ImageUrl ?? string.Empty,
                            Rating = (double)r.Rating,
                            ReviewCount = r.Reviews?.Count ?? 0,
                            CategoryName = r.Category?.Name ?? string.Empty,
                            DeliveryFee = r.DeliveryFee,
                        }).ToList(),

                    ActivePromotions = (await _unitOfWork.Promotions.GetActivePromotionsAsync())
                        .Select(p => new PromotionViewModel
                        {
                            Id = p.Id,
                            Code = p.Code,
                            Description = p.Description,
                            DiscountValue = p.DiscountValue,
                            IsPercentage = p.IsPercentage,
                            MinimumOrderAmount = p.MinimumOrderAmount ?? 0m,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            RestaurantId = p.RestaurantId,
                            RestaurantName = p.Restaurant?.Name ?? string.Empty
                        }).ToList(),


                    AppDownload = new AppDownloadSection
                    {
                        AppScreenshotUrl = "/images/app-screenshot.png"
                    },

                    PopularSearches = popularSearches
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading home page");
                return View("Error");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult NotFound()
        {
            return View("~/Views/Shared/NotFound.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}
