using System.Threading.Tasks;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.ViewModels.Driver;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryApp.Controllers
{
    public class DriverController : Controller
    {
        private readonly IDriverService _driverService;
        private readonly ICurrentUserService _currentUserService;

        public DriverController(IDriverService driverService, ICurrentUserService currentUserService)
        {
            _driverService = driverService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(DriverCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var driver = await _driverService.CreateDriverAsync(model);
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to register as driver. Please try again.");
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userId = _currentUserService.GetCurrentUserId();
            var driver = await _driverService.GetDriverByUserIdAsync(userId);
            if (driver == null)
                return NotFound();

            return View(driver);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = _currentUserService.GetCurrentUserId();
            var driver = await _driverService.GetDriverByUserIdAsync(userId);
            if (driver == null)
                return NotFound();

            return View(driver);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DriverApiController : ControllerBase
    {
        private readonly IDriverService _driverService;

        public DriverApiController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driverService.GetAllDriversAsync();
            return Ok(drivers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            var driver = await _driverService.GetDriverByIdAsync(id);
            if (driver == null)
                return NotFound();

            return Ok(driver);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDriver(DriverCreateViewModel model)
        {
            var driver = await _driverService.CreateDriverAsync(model);
            return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, DriverUpdateViewModel model)
        {
            var driver = await _driverService.UpdateDriverAsync(id, model);
            if (driver == null)
                return NotFound();

            return Ok(driver);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var result = await _driverService.DeleteDriverAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] DriverStatus status)
        {
            var driver = await _driverService.UpdateDriverStatusAsync(id, status);
            if (driver == null) return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/location")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationUpdateModel model)
        {
            var driver = await _driverService.UpdateDriverLocationAsync(id, model.Latitude, model.Longitude);
            if (driver == null)
                return NotFound();

            return NoContent();
        }
    }

    public class LocationUpdateModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
    }
} 