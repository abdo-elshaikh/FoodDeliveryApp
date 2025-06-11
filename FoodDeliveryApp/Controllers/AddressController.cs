using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Address;
using System.Collections.Generic;
using System.Linq;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FoodDeliveryApp.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IUnitOfWork unitOfWork, ILogger<AddressController> logger, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }


        public IActionResult Index()
        {
            var userId = _currentUserService.GetCurrentUserId();
            var addresses = _unitOfWork.Addresses.GetUserAddressesAsync(userId).Result;


            var model = new AddressListViewModel
            {
                Addresses = addresses.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    StreetAddress = a.StreetAddress,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.PostalCode,
                    Country = a.Country,
                    IsDefault = a.IsDefault,
                    AddressType = a.AddressType
                }).ToList()
            };
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new AddressCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newAddress = new Address
                {
                    StreetAddress = model.StreetAddress,
                    City = model.City,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    Country = model.Country,
                    IsDefault = model.IsDefault,
                    AddressType = model.AddressType
                };
                newAddress.UserId = _currentUserService.GetCurrentUserId();
                await _unitOfWork.Addresses.AddAsync(newAddress);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var address = await _unitOfWork.Addresses.GetByIdAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            var model = new AddressCreateViewModel
            {
                StreetAddress = address.StreetAddress,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                IsDefault = address.IsDefault,
                AddressType = address.AddressType
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddressCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var address = await _unitOfWork.Addresses.GetByIdAsync(id);
                if (address == null)
                {
                    return NotFound();
                }
                address.StreetAddress = model.StreetAddress;
                address.City = model.City;
                address.State = model.State;
                address.PostalCode = model.PostalCode;
                address.Country = model.Country;
                address.IsDefault = model.IsDefault;
                address.AddressType = model.AddressType;
                await _unitOfWork.Addresses.UpdateAsync(address);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var address = await _unitOfWork.Addresses.GetByIdAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _unitOfWork.Addresses.GetByIdAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            await _unitOfWork.Addresses.DeleteAsync(address);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
