using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.ViewModels;
using System.Diagnostics;

namespace FoodDeliveryApp.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(IWebHostEnvironment environment, ILogger<ErrorController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var errorViewModel = new ErrorViewModel
            {
                StatusCode = statusCode,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ShowRequestId = _environment.IsDevelopment(),
                IsDevelopment = _environment.IsDevelopment(),
                Exception = errorFeature?.Error
            };

            // Handle ERR_CONNECTION_REFUSED (simulate with status code 0 or custom)
            if (statusCode == 0)
            {
                errorViewModel.ErrorTitle = "Connection Refused";
                errorViewModel.ErrorMessage = "The server refused the connection. Please check if the server is running and try again.";
                return View("Error", errorViewModel);
            }

            switch (statusCode)
            {
                case 404:
                    errorViewModel.ErrorTitle = "Page Not Found";
                    errorViewModel.ErrorMessage = "The page you're looking for doesn't exist or has been moved.";
                    break;

                case 403:
                    errorViewModel.ErrorTitle = "Access Denied";
                    errorViewModel.ErrorMessage = "You don't have permission to access this resource.";
                    break;

                case 401:
                    errorViewModel.ErrorTitle = "Unauthorized";
                    errorViewModel.ErrorMessage = "Please log in to access this resource.";
                    break;

                case 503:
                    errorViewModel.ErrorTitle = "Service Unavailable";
                    errorViewModel.ErrorMessage = "The service is currently unavailable. Please try again later.";
                    break;

                case 500:
                    errorViewModel.ErrorTitle = "Server Error";
                    errorViewModel.ErrorMessage = "Something went wrong on our end. Please try again later.";
                    break;

                default:
                    errorViewModel.ErrorTitle = "Error";
                    errorViewModel.ErrorMessage = "An unexpected error occurred.";
                    break;
            }

            return View("Error", errorViewModel);
        }

        [Route("Error/Offline")]
        public IActionResult Offline()
        {
            // This action can be removed or merged into Error action since 503 is handled there now.
            return RedirectToAction("Error", new { statusCode = 503 });
        }
    }
} 