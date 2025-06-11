using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.Home;
using Microsoft.Extensions.Logging;

namespace FoodDeliveryApp.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IUnitOfWork unitOfWork, ILogger<ReviewService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<ReviewViewModel>> GetRecentReviewsAsync(int count)
        {
            try
            {
                var reviews = await _unitOfWork.Reviews.GetRecentReviewsAsync(count);
                return reviews.Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    UserName = r.User?.UserName ?? "Anonymous",
                    UserAvatar = r.User?.ProfilePictureUrl ?? string.Empty,
                    Comment = r.Content ?? "N/A",
                    Rating = (int)r.Rating,
                    RestaurantName = r.Restaurant?.Name ?? "N/A",
                    CreatedAt = r.CreatedAt,
                    Images = new List<string>()
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching recent reviews");
                throw;
            }
        }

        public async Task<List<ReviewViewModel>> GetRestaurantReviewsAsync(int restaurantId, int page = 1, int pageSize = 10)
        {
            try
            {
                var reviews = await _unitOfWork.Reviews.GetRestaurantReviewsAsync(restaurantId);
                return reviews.Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    UserName = r.User?.UserName ?? "Anonymous",
                    UserAvatar = r.User?.ProfilePictureUrl,
                    Comment = r.Content,
                    Rating = (int)r.Rating,
                    RestaurantName = r.Restaurant?.Name,
                    CreatedAt = r.CreatedAt,
                    Images = new List<string>()
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching restaurant reviews for restaurant {RestaurantId}", restaurantId);
                throw;
            }
        }

        public async Task<ReviewViewModel> GetReviewByIdAsync(int id)
        {
            try
            {
                var review = await _unitOfWork.Reviews.GetByIdAsync(id);
                if (review == null)
                    return null;

                return new ReviewViewModel
                {
                    Id = review.Id,
                    UserName = review.User?.UserName ?? "Anonymous",
                    UserAvatar = review.User?.ProfilePictureUrl ?? string.Empty,
                    Comment = review.Content ?? "N/A",
                    Rating = (int)review.Rating,
                    RestaurantName = review.Restaurant?.Name ?? "N/A",
                    CreatedAt = review.CreatedAt,
                    Images = new List<string>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching review by ID: {Id}", id);
                throw;
            }
        }

        public async Task<ReviewViewModel> CreateReviewAsync(ReviewViewModel review)
        {
            try
            {
                var newReview = new Models.Review
                {
                    Content = review.Comment,
                    Rating = review.Rating,
                    RestaurantId = review.RestaurantId,
                    UserId = review.UserId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Reviews.AddAsync(newReview);
                await _unitOfWork.SaveChangesAsync();

                return new ReviewViewModel
                {
                    Id = newReview.Id,
                    UserName = newReview.User?.UserName ?? "Anonymous",
                    UserAvatar = newReview.User?.ProfilePictureUrl,
                    Comment = newReview.Content,
                    Rating = (int)newReview.Rating,
                    RestaurantName = newReview.Restaurant?.Name,
                    CreatedAt = newReview.CreatedAt,
                    Images = new List<string>()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating review");
                throw;
            }
        }

        public async Task<bool> UpdateReviewAsync(ReviewViewModel review)
        {
            try
            {
                var existingReview = await _unitOfWork.Reviews.GetByIdAsync(review.Id);
                if (existingReview == null)
                    return false;

                existingReview.Content = review.Comment;
                existingReview.Rating = review.Rating;
                existingReview.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating review: {Id}", review.Id);
                throw;
            }
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            try
            {
                var review = await _unitOfWork.Reviews.GetByIdAsync(id);
                if (review == null)
                    return false;

                await _unitOfWork.Reviews.DeleteAsync(review);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting review: {Id}", id);
                throw;
            }
        }
    }
} 