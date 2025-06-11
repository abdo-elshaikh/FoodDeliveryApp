using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.Promotion;
using Microsoft.Extensions.Logging;

namespace FoodDeliveryApp.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PromotionService> _logger;

        public PromotionService(IUnitOfWork unitOfWork, ILogger<PromotionService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<PromotionViewModel>> GetActivePromotionsAsync()
        {
            try
            {
                var promotions = await _unitOfWork.Promotions.GetActivePromotionsAsync();
                return promotions.Select(p => new PromotionViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Code = p.Code,
                    EndDate = p.ValidUntil,
                    IsActive = p.IsActive,
                    ImageUrl = p.ImageUrl ?? string.Empty,
                    IsPercentage = p.IsPercentage,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching active promotions");
                throw;
            }
        }

        public async Task<PromotionViewModel> GetPromotionByIdAsync(int id)
        {
            try
            {
                var promotion = await _unitOfWork.Promotions.GetByIdAsync(id);
                if (promotion == null)
                    return null;

                return new PromotionViewModel
                {
                    Id = promotion.Id,
                    Title = promotion.Title,
                    Description = promotion.Description,
                    Code = promotion.Code,
                    EndDate = promotion.ValidUntil,
                    IsActive = promotion.IsActive,
                    ImageUrl = promotion.ImageUrl ?? string.Empty,
                    IsPercentage = promotion.IsPercentage,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching promotion by ID: {Id}", id);
                throw;
            }
        }

        public async Task<PromotionViewModel> GetPromotionByCodeAsync(string code)
        {
            try
            {
                var promotion = await _unitOfWork.Promotions.GetByCodeAsync(code);
                if (promotion == null)
                    return null;

                return new PromotionViewModel
                {
                    Id = promotion.Id,
                    Title = promotion.Title,
                    Description = promotion.Description,
                    Code = promotion.Code,
                    EndDate = promotion.ValidUntil,
                    IsActive = promotion.IsActive,
                    ImageUrl = promotion.ImageUrl ?? string.Empty,
                    IsPercentage = promotion.IsPercentage,

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching promotion by code: {Code}", code);
                throw;
            }
        }

        public async Task<bool> ValidatePromotionAsync(string code, decimal orderAmount)
        {
            try
            {
                var promotion = await _unitOfWork.Promotions.GetByCodeAsync(code);
                if (promotion == null || !promotion.IsActive)
                    return false;

                if (promotion.ValidUntil < DateTime.UtcNow)
                    return false;

                if (orderAmount < promotion.MinimumOrderAmount)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating promotion code: {Code}", code);
                throw;
            }
        }
    }
} 