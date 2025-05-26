using FoodDeliveryApp.Models;using FoodDeliveryApp.Repositories.Interfaces;using FoodDeliveryApp.ViewModels.Promotion;using FoodDeliveryApp.ViewModels.PromotionViewModels;using FoodDeliveryApp.ViewModels.Restaurant;using FoodDeliveryApp.ViewModels.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDeliveryApp.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<RestaurantCategory> _categoryRepository;
        private readonly IRepository<Promotion> _promotionRepository;
        private readonly IRepository<PromotionUsage> _promoUsageRepository;

        public RestaurantController(
            IRestaurantRepository restaurantRepository,
            IMenuItemRepository menuItemRepository,
            IReviewRepository reviewRepository,
            IRepository<RestaurantCategory> categoryRepository,
            IRepository<Promotion> promotionRepository,
            IRepository<PromotionUsage> promoUsageRepository,
            UserManager<ApplicationUser> userManager)
        {
            _restaurantRepository = restaurantRepository;
            _menuItemRepository = menuItemRepository;
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
            _promotionRepository = promotionRepository;
            _promoUsageRepository = promoUsageRepository;
        }

        #region Restaurant Management

        // GET: Restaurant Home (Index) with filtering, sorting and pagination
        [Route("restaurants")]
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, string searchTerm, string sortOrder = "name", int page = 1, string cuisines = "", string dietary = "", string features = "", string deliveryFee = "")
        {
            const int pageSize = 9;
            var categories = await _categoryRepository.GetAllAsync();
            var restaurantsQuery = await _restaurantRepository.GetAllAsync();

            if (categoryId.HasValue)
                restaurantsQuery = restaurantsQuery.Where(r => r.CategoryId == categoryId.Value);
            
            if (!string.IsNullOrEmpty(searchTerm))
                restaurantsQuery = restaurantsQuery.Where(r => r.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                                                             r.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                             r.Category.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            var cuisineList = !string.IsNullOrEmpty(cuisines) ? cuisines.Split(',', StringSplitOptions.RemoveEmptyEntries) : new string[0];
            var dietaryList = !string.IsNullOrEmpty(dietary) ? dietary.Split(',', StringSplitOptions.RemoveEmptyEntries) : new string[0];
            var featureList = !string.IsNullOrEmpty(features) ? features.Split(',', StringSplitOptions.RemoveEmptyEntries) : new string[0];

            // Filter by delivery fee
            if (!string.IsNullOrEmpty(deliveryFee))
            {
                if (decimal.TryParse(deliveryFee, out var fee))
                {
                    restaurantsQuery = restaurantsQuery.Where(r => r.DeliveryFee <= fee);
                }
            }
            
            // Filter by dietary restrictions (this would need to be expanded with more detailed dietary data)
            if (dietaryList.Any())
            {
                restaurantsQuery = restaurantsQuery.Where(r => dietaryList.Any(d => r.Description.Contains(d, StringComparison.OrdinalIgnoreCase)));
            }
            
            // Filter by cuisines (using category as a proxy in this simple example)
            if (cuisineList.Any())
            {
                restaurantsQuery = restaurantsQuery.Where(r => cuisineList.Any(c => r.Category.Name.Contains(c, StringComparison.OrdinalIgnoreCase)));
            }

            // Sort restaurants
            restaurantsQuery = sortOrder switch
            {
                "name_desc" => restaurantsQuery.OrderByDescending(r => r.Name),
                "rating" => restaurantsQuery.OrderByDescending(r => r.Rating),
                "rating_asc" => restaurantsQuery.OrderBy(r => r.Rating),
                "delivery_fee" => restaurantsQuery.OrderBy(r => r.DeliveryFee),
                _ => restaurantsQuery.OrderBy(r => r.Name)
            };
            
            // Paginate restaurants
            var totalRestaurants = restaurantsQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRestaurants / pageSize);
            var restaurants = restaurantsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var restaurantViewModels = restaurants.Select(r => new RestaurantViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                LogoUrl = r.ImageUrl,
                CoverImageUrl = r.ImageUrl,
                PhoneNumber = r.PhoneNumber,
                Website = r.WebsiteUrl,
                Rating = (double)r.Rating,
                CategoryName = r.Category?.Name,
                IsOpen = IsRestaurantOpenNow(r),
                IsActive = r.IsActive,
                DeliveryFee = r.DeliveryFee,
                IsAdminOrOwner = IsOwnerOrAdmin(r.OwnerId),
            }).ToList();

            var viewModel = new RestaurantListViewModel
            {
                Restaurants = restaurantViewModels,
                Categories = new SelectList(categories, "Id", "Name"),
                CurrentCategoryId = categoryId,
                SearchTerm = searchTerm,
                CurrentSortOrder = sortOrder,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalCount = totalRestaurants,
                MaxDeliveryFee = deliveryFee,
                SelectedCuisines = cuisineList.ToList(),
                SelectedDietaryOptions = dietaryList.ToList(),
                IsAdmin = User.IsInRole("Admin"),
            };
            
            return View(viewModel);
        }

        // GET: Restaurant Details
        [Route("restaurants/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            if (restaurant == null) return NotFound();

            var menuItems = await _menuItemRepository.GetByRestaurantAsync(id);
            var reviews = await _reviewRepository.GetRecentReviewsAsync(id);
            var promotions = await _promotionRepository.FindAsync(p => p.RestaurantId == id);
            var isAdminOrOwner = IsOwnerOrAdmin(restaurant.OwnerId);

            var menuItemsModel = new List<RestaurantMenuItemViewModel>();
            foreach (var item in menuItems)
            {
                menuItemsModel.Add(new RestaurantMenuItemViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    ImageUrl = item.ImageUrl,
                    IsAvailable = item.IsAvailable
                });
            }
            
            var reviewsModel = new List<ReviewViewModel>();
            foreach (var review in reviews)
            {
                reviewsModel.Add(new ReviewViewModel
                {
                    Id = review.Id,
                    CustomerName = $"{review.CustomerProfile.FirstName} {review.CustomerProfile.LastName}",
                    Rating = (int)review.Rating,
                    Comment = review.Comment,
                    CreatedAt = review.CreatedAt,
                });
            }

            var promotionsModel = new List<PromotionViewModel>();
            foreach (var promotion in promotions.Where(p => p.IsActive && p.EndDate > DateTime.Now))
            {
                promotionsModel.Add(new PromotionViewModel
                {
                    Id = promotion.Id,
                    Description = promotion.Description,
                    DiscountValue = promotion.DiscountValue,
                    PromotionCode = promotion.Code,
                    StartDate = promotion.StartDate,
                    EndDate = promotion.EndDate,
                    MinimumOrderAmount = promotion.MinimumOrderAmount ?? 0,
                    UsageLimit = promotion.UsageLimit ?? 0,
                    IsActive = promotion.IsActive
                });
            }

            var model = new RestaurantDetailViewModel
            {
                Restaurant = new RestaurantViewModel
                {
                    Id = restaurant.Id,
                    Name = restaurant.Name,
                    Description = restaurant.Description,
                    PhoneNumber = restaurant.PhoneNumber,
                    CoverImageUrl = restaurant.ImageUrl,
                    Rating = (double)restaurant.Rating,
                    CategoryName = restaurant.Category?.Name,
                    IsOpen = IsRestaurantOpenNow(restaurant),
                    IsActive = restaurant.IsActive,
                    Website = restaurant.WebsiteUrl,
                    DeliveryFee = restaurant.DeliveryFee,
                    Address = new ViewModels.Address.AddressViewModel
                    {
                        AddressLine = restaurant.Address,
                        City = restaurant.City,
                        State = restaurant.State,
                        PostalCode = restaurant.PostalCode
                    },
                    OpeningTime = restaurant.OpeningTime,
                    ClosingTime = restaurant.ClosingTime,
                    IsAdminOrOwner = isAdminOrOwner,

                },
                MenuItems = menuItemsModel,
                Reviews = reviewsModel,
                Promotions = promotionsModel,
                IsAdminOrOwner = isAdminOrOwner,
            };

            return View(model);
        }

        // GET: Restaurant Create
        [Authorize(Roles = "Admin, Owner")]
        [Route("restaurants/create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to create a restaurant.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new CreateRestaurantViewModel
            {
                Categories = new SelectList(categories, "Id", "Name"),
                OpeningTime = new TimeSpan(9, 0, 0),
                ClosingTime = new TimeSpan(22, 0, 0),
            };

            if (User.IsInRole("Admin"))
            {
                var owners = await _userManager.GetUsersInRoleAsync("Owner");
                viewModel.OwnerId = "";
                viewModel.Owners = new SelectList(owners, "Id", "UserName");
            }
            else
            {
                viewModel.OwnerId = currentUser.Id;
            }

            return View(viewModel);
        }

        // POST: Restaurant Create
        [Authorize(Roles = "Admin, Owner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("restaurants/create")]
        public async Task<IActionResult> Create(CreateRestaurantViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategorySelectList();

                // Repopulate owners list if admin
                if (User.IsInRole("Admin"))
                {
                    var owners = await _userManager.GetUsersInRoleAsync("Owner");
                    model.Owners = new SelectList(owners, "Id", "UserName");
                }

                return View(model);
            }

            // Validate owner assignment
            if (User.IsInRole("Owner") && model.OwnerId != currentUser?.Id)
            {
                ModelState.AddModelError("", "You can only create restaurants for yourself.");
                model.Categories = await GetCategorySelectList();
                return View(model);
            }

            var restaurant = new Restaurant
            {
                Name = model.Name,
                Description = model.Description,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                OpeningTime = model.OpeningTime,
                ClosingTime = model.ClosingTime,
                CategoryId = model.CategoryId,
                OwnerId = User.IsInRole("Admin") ? model.OwnerId : currentUser?.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                WebsiteUrl = model.Website,
                LocationUrl = model.LocationUrl,
                DeliveryFee = model.DeliveryFee,
                TaxRate = model.TaxRate,
                Rating = 0 // New restaurant starts with 0 rating
            };

            if (model.ImageFile != null)
            {
                restaurant.ImageUrl = await SaveImageAsync(model.ImageFile, model.Name);
            }
            else if (!string.IsNullOrEmpty(model.ImageUrl))
            {
                restaurant.ImageUrl = model.ImageUrl;
            }

            try
            {
                await _restaurantRepository.AddAsync(restaurant);
                TempData["SuccessMessage"] = "Restaurant created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating restaurant: {ex.Message}";
                model.Categories = await GetCategorySelectList();

                if (User.IsInRole("Admin"))
                {
                    var owners = await _userManager.GetUsersInRoleAsync("Owner");
                    model.Owners = new SelectList(owners, "Id", "UserName");
                }

                return View(model);
            }
        }

        // GET: Restaurant Edit
        [Authorize(Roles = "Admin, Owner")]
        [Route("restaurants/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            if (restaurant == null) return NotFound();

            if (!IsOwnerOrAdmin(restaurant.OwnerId))
                return Forbid();

            var categories = await _categoryRepository.GetAllAsync();
            var currentUser = await _userManager.GetUserAsync(User);

            var viewModel = new EditRestaurantViewModel
            {
                Id = restaurant.Id,
                OwnerId = restaurant.OwnerId,
                Name = restaurant.Name,
                Description = restaurant.Description,
                PhoneNumber = restaurant.PhoneNumber,
                Address = restaurant.Address,
                City = restaurant.City,
                State = restaurant.State,
                PostalCode = restaurant.PostalCode,
                OpeningTime = restaurant.OpeningTime,
                ClosingTime = restaurant.ClosingTime,
                CategoryId = restaurant.CategoryId,
                ImageUrl = restaurant.ImageUrl,
                Categories = new SelectList(categories, "Id", "Name", restaurant.CategoryId),
                IsActive = restaurant.IsActive,
                Website = restaurant.WebsiteUrl,
                LocationUrl = restaurant.LocationUrl,
                DeliveryFee = restaurant.DeliveryFee,
                TaxRate = restaurant.TaxRate,
            };

            // If admin, show owner selection dropdown
            if (User.IsInRole("Admin"))
            {
                var owners = await _userManager.GetUsersInRoleAsync("Owner");
                viewModel.Owners = new SelectList(owners, "Id", "UserName", restaurant.OwnerId);
            }

            return View(viewModel);
        }

        // POST: Restaurant Edit
        [Authorize(Roles = "Admin, Owner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("restaurants/edit/{id:int}")]
        public async Task<IActionResult> Edit(EditRestaurantViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategorySelectList();

                if (User.IsInRole("Admin"))
                {
                    var owners = await _userManager.GetUsersInRoleAsync("Owner");
                    model.Owners = new SelectList(owners, "Id", "UserName", model.OwnerId);
                }
                return View(model);
            }

            var restaurant = await _restaurantRepository.GetByIdAsync(model.Id);
            if (restaurant == null) return NotFound();

            if (!IsOwnerOrAdmin(restaurant.OwnerId))
                return Forbid();

            // Validate owner assignment
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.IsInRole("Owner") && model.OwnerId != currentUser?.Id)
            {
                ModelState.AddModelError("", "You can only assign restaurants to yourself.");
                model.Categories = await GetCategorySelectList();
                return View(model);
            }

            restaurant.Name = model.Name;
            restaurant.Description = model.Description;
            restaurant.PhoneNumber = model.PhoneNumber;
            restaurant.Address = model.Address;
            restaurant.City = model.City;
            restaurant.State = model.State;
            restaurant.PostalCode = model.PostalCode;
            restaurant.OpeningTime = model.OpeningTime;
            restaurant.ClosingTime = model.ClosingTime;
            restaurant.CategoryId = model.CategoryId;
            restaurant.UpdatedAt = DateTime.UtcNow;
            restaurant.IsActive = model.IsActive;
            restaurant.WebsiteUrl = model.Website;
            restaurant.LocationUrl = model.LocationUrl;
            restaurant.DeliveryFee = model.DeliveryFee;
            restaurant.TaxRate = model.TaxRate;

            // Only allow admin to change owner
            if (User.IsInRole("Admin"))
            {
                restaurant.OwnerId = model.OwnerId;
            }

            if (model.ImageFile != null)
            {
                restaurant.ImageUrl = await SaveImageAsync(model.ImageFile, model.Name);
            }
            else if (!string.IsNullOrEmpty(model.ImageUrl))
            {
                restaurant.ImageUrl = model.ImageUrl;
            }

            try
            {
                await _restaurantRepository.UpdateAsync(restaurant);
                TempData["SuccessMessage"] = "Restaurant updated successfully!";
                return RedirectToAction(nameof(Details), new { id = restaurant.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating restaurant: {ex.Message}";
                model.Categories = await GetCategorySelectList();

                if (User.IsInRole("Admin"))
                {
                    var owners = await _userManager.GetUsersInRoleAsync("Owner");
                    model.Owners = new SelectList(owners, "Id", "UserName");
                }

                return View(model);
            }
        }

        // POST: Toggle Restaurant Status
        [Authorize(Roles = "Admin, Owner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("restaurants/toggle-status/{id:int}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            if (restaurant == null) return NotFound();

            if (!IsOwnerOrAdmin(restaurant.OwnerId))
                return Forbid();

            restaurant.IsActive = !restaurant.IsActive;
            await _restaurantRepository.UpdateAsync(restaurant);

            TempData["SuccessMessage"] = $"Restaurant {(restaurant.IsActive ? "activated" : "deactivated")} successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Restaurant Delete
        [Authorize(Roles = "Admin")]
        [Route("restaurants/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            if (restaurant == null) return NotFound();

            return View(restaurant);
        }

        // POST: Restaurant Delete
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(id);
            if (restaurant == null) return NotFound();

            try
            {
                await _restaurantRepository.RemoveAsync(restaurant);
                TempData["SuccessMessage"] = "Restaurant deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting restaurant: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private static bool IsRestaurantOpenNow(Restaurant restaurant)
        {
            var currentTime = DateTime.UtcNow.TimeOfDay;
            return currentTime >= restaurant.OpeningTime && currentTime <= restaurant.ClosingTime;
        }

        private async Task<SelectList> GetCategorySelectList()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return new SelectList(categories, "Id", "Name");
        }

        private bool IsOwnerOrAdmin(string ownerId)
        {
            var currentUserId = _userManager.GetUserId(User);
            var Admin = User.IsInRole("Admin");
            var Owner = User.IsInRole("Owner") && currentUserId == ownerId;
            return Admin || Owner;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile, string restaurantName)
        {
            if (imageFile == null || imageFile.Length == 0) return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "restaurants");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{restaurantName.Replace(" ", "-")}-{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/restaurants/{uniqueFileName}";
        }
        #endregion

        #region Promotion Management
        // Get: Promotion List
        [Authorize(Roles = "Admin, Owner")]
        [Route("restaurants/promotions/{restaurantId:int}")]
        public async Task<IActionResult> Promotions(int restaurantId)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (!IsOwnerOrAdmin(restaurant.OwnerId))
            {
                return Forbid();
            }

            var promotions = await _promotionRepository.FindAsync(p => p.RestaurantId == restaurantId);
            if (promotions == null || !promotions.Any())
            {
                TempData["InfoMessage"] = "No promotions found for this restaurant.";
            }

            var viewModel = new RestaurantPromotionsViewModel
            {
                RestaurantId = restaurantId,
                RestaurantName = restaurant.Name,
                Promotions = promotions.Select(p => new PromotionViewModel
                {
                    Id = p.Id,
                    PromotionCode = p.Code ?? string.Empty,
                    Description = p.Description ?? string.Empty,
                    DiscountValue = p.DiscountValue,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    MinimumOrderAmount = p.MinimumOrderAmount ?? 0,
                    UsageLimit = p.UsageLimit ?? 0,
                    RestaurantId = p.RestaurantId ?? 0,
                    RestaurantName = restaurant.Name,
                    IsActive = p.IsActive && p.StartDate <= DateTime.UtcNow && p.EndDate >= DateTime.UtcNow
                }).ToList()
            };

            return View(viewModel);
        }

        // GET: /Restaurant/Promotions/Create/5
        [Authorize(Roles = "Admin, Owner")]
        [Route("restaurants/promotions/create/{restaurantId:int}")]
        public async Task<IActionResult> CreatePromotion(int restaurantId)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (!IsOwnerOrAdmin(restaurant.OwnerId))
            {
                return Forbid();
            }

            var viewModel = new PromotionCreateViewModel
            {
                RestaurantId = restaurantId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                Code = string.Empty, // Initialize required string
                Description = string.Empty // Initialize required string
            };
            return View(viewModel);
        }

        // POST: /Restaurant/Promotions/Create
        [Authorize(Roles = "Admin, Owner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("restaurants/promotions/create/{restaurantId:int}")]
        public async Task<IActionResult> CreatePromotion(PromotionCreateViewModel model, int restaurantId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null) return NotFound();
            if (!IsOwnerOrAdmin(restaurant.OwnerId))
                return Forbid();
            var promotion = new Promotion
            {
                Code = model.Code,
                Description = model.Description,
                DiscountValue = model.DiscountValue,
                IsPercentage = model.IsPercentage,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                RestaurantId = model.RestaurantId,
                UsageLimit = model.UsageLimit,
                MinimumOrderAmount = (int)model.MinimumOrderAmount,
                IsActive = true
            };
            try
            {
                await _promotionRepository.AddAsync(promotion);
                TempData["SuccessMessage"] = "Promotion created successfully!";
                return RedirectToAction("Promotions", new { restaurantId = restaurant.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating promotion: {ex.Message}";
                return View(model);
            }
        }
        // GET: Promotion Edit
        [HttpGet]
        [Authorize(Roles = "Admin, Owner")]
        [Route("restaurants/promotions/edit/{id:int}")]
        public async Task<IActionResult> EditPromotion(int id)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null) return NotFound();
            var restaurant = await _restaurantRepository.GetByIdAsync(promotion.RestaurantId ?? 0);
            if (restaurant == null) return NotFound();
            if (!IsOwnerOrAdmin(restaurant.OwnerId))
                return Forbid();
            var viewModel = new PromotionEditViewModel
            {
                Id = promotion.Id,
                Code = promotion.Code,
                Description = promotion.Description,
                DiscountValue = promotion.DiscountValue,
                IsPercentage = promotion.IsPercentage,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                MinimumOrderAmount = (int)promotion.MinimumOrderAmount,
                UsageLimit = promotion.UsageLimit,
                RestaurantId = restaurant.Id,
                IsActive = promotion.IsActive
            };
            return View(viewModel);
        }

        // POST: Promotion Edit
        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        [ValidateAntiForgeryToken]
        [Route("restaurants/promotions/edit/{id:int}")]
        public async Task<IActionResult> EditPromotion(PromotionEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var promotion = await _promotionRepository.GetByIdAsync(model.Id);
            if (promotion == null) return NotFound();
            var restaurantId = promotion.RestaurantId ?? 0;
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null) return NotFound();
            if (!IsOwnerOrAdmin(restaurant.OwnerId))
                return Forbid();
            promotion.Code = model.Code;
            promotion.Description = model.Description;
            promotion.DiscountValue = model.DiscountValue;
            promotion.IsPercentage = model.IsPercentage;
            promotion.StartDate = model.StartDate;
            promotion.EndDate = model.EndDate;
            promotion.MinimumOrderAmount = (int)model.MinimumOrderAmount;
            promotion.UsageLimit = model.UsageLimit;
            promotion.IsActive = model.IsActive;
            try
            {
                await _promotionRepository.UpdateAsync(promotion);
                TempData["SuccessMessage"] = "Promotion updated successfully!";
                return RedirectToAction("Promotions", new { restaurantId = restaurant.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating promotion: {ex.Message}";
                return View(model);
            }
        }

        // POST: Promotion Delete
        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        [Route("restaurants/promotions/delete/{id:int}")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null) return NotFound();
            var restaurantId = promotion.RestaurantId ?? 0;
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (!IsOwnerOrAdmin(restaurant.OwnerId))
                return Forbid();
            try
            {
                await _promotionRepository.RemoveAsync(promotion);
                TempData["SuccessMessage"] = "Promotion deleted successfully!";
                return RedirectToAction("Promotions", new { restaurantId = restaurant.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting promotion: {ex.Message}";
                return RedirectToAction("Promotions", new { restaurantId = restaurant.Id });
            }
        }

        // POST: Toggle Promotion Status
        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        [Route("restaurants/promotions/toggle-status/{id:int}")]
        public async Task<IActionResult> TogglePromotionStatus(int id)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null) return NotFound();
            var restaurantId = promotion.RestaurantId ?? 0;
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (!IsOwnerOrAdmin(restaurant.OwnerId))
                return Forbid();
            promotion.IsActive = !promotion.IsActive;
            await _promotionRepository.UpdateAsync(promotion);
            TempData["SuccessMessage"] = $"Promotion {(promotion.IsActive ? "activated" : "deactivated")} successfully!";
            return RedirectToAction("Promotions", new { restaurantId = restaurant.Id });
        }

        // GET: Promotion Details
        [HttpGet]
        [Authorize(Roles = "Admin, Owner")]
        [Route("restaurants/promotions/details/{id:int}")]
        public async Task<IActionResult> PromotionDetails(int id)
        {
            var promotion = await _promotionRepository.GetByIdAsync(id);
            if (promotion == null) return NotFound();
            var restaurant = await _restaurantRepository.GetByIdAsync(promotion.RestaurantId ?? 0);
            if (restaurant == null) return NotFound();
            if (!IsOwnerOrAdmin(restaurant.OwnerId))
                return Forbid();
            var viewModel = new PromotionViewModel
            {
                Id = promotion.Id,
                PromotionCode = promotion.Code,
                Description = promotion.Description,
                DiscountValue = promotion.DiscountValue,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                MinimumOrderAmount = promotion.MinimumOrderAmount ?? 0,
                UsageLimit = promotion.UsageLimit ?? 0,
                RestaurantName = restaurant.Name,
                RestaurantId = restaurant.Id,
                IsActive = promotion.IsActive
            };
            return View(viewModel);
        }

        // GET: Apply Promotion
        [HttpGet]
        public async Task<IActionResult> ApplyPromotion(string code)
        {
            var promotion = await _promotionRepository.FirstOrDefaultAsync(p => p.Code == code);
            if (promotion == null || !promotion.IsActive)
            {
                TempData["ErrorMessage"] = "Invalid or inactive promotion code.";
                return RedirectToAction("Index", "Home");
            }
            // Check if the promotion is valid for the current user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to apply a promotion.";
                return RedirectToAction("Login", "Account");
            }
            // Check if the promotion is valid for the current restaurant
            var restaurant = await _restaurantRepository.GetByIdAsync(promotion.RestaurantId ?? 0);
            if (restaurant == null)
            {
                TempData["ErrorMessage"] = "Invalid restaurant for this promotion.";
                return RedirectToAction("Index", "Home");
            }

            // Check if the promotion is valid for the current order
            var model = new PromotionApplyViewModel
            {
                PromoCode = code,
                RestaurantId = restaurant.Id,
                DiscountAmount = promotion.IsPercentage ? promotion.DiscountValue : 0,
                FinalAmount = promotion.IsPercentage ? promotion.DiscountValue : 0,
                IsPromoApplied = true,
                TotalAmount = 0
            };
            return View(model);
        }


        // POST: Apply Promotion
        [HttpPost]
        [Authorize]
        [Route("restaurants/promotions/apply/{code}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyPromotion(PromotionApplyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    TempData["ErrorMessage"] = "You must be logged in to apply a promotion.";
                    return RedirectToAction("Login", "Account");
                }
                var promotion = await _promotionRepository.FirstOrDefaultAsync(p => p.Code == model.PromoCode);
                if (promotion == null || !promotion.IsActive)
                {
                    TempData["ErrorMessage"] = "Invalid or inactive promotion code.";
                    return RedirectToAction("Index", "Home");
                }
                // Check if the promotion is valid for the current order
                var restaurant = await _restaurantRepository.GetByIdAsync(promotion.RestaurantId ?? 0);
                if (restaurant == null)
                {
                    TempData["ErrorMessage"] = "Invalid restaurant for this promotion.";
                    return RedirectToAction("Index", "Home");
                }
                // Create a new promotion usage record
                await CreatePromotionUsageRecord(promotion.Id, currentUser.Id, 0); // Assuming orderId is 0 for now
                TempData["SuccessMessage"] = "Promotion applied successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to apply promotion.";
            }
            return RedirectToAction("Index", "Home");
        }

        // create a new promotion usage record
        private async Task CreatePromotionUsageRecord(int promotionId, string userId, int orderId)
        {
            var usage = new PromotionUsage
            {
                PromotionId = promotionId,
                UserId = userId,
                OrderId = orderId,
                UsedAt = DateTime.UtcNow
            };
            await _promoUsageRepository.AddAsync(usage);
        }
        #endregion
    }
}