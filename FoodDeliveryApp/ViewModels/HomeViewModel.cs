namespace FoodDeliveryApp.ViewModels
{
    public class HomeViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<RestaurantHomeViewModel> PopularRestaurants { get; set; } = new List<RestaurantHomeViewModel>();
        public List<DishViewModel> PopularDishes { get; set; } = new List<DishViewModel>();
        public List<TestimonialViewModel> Testimonials { get; set; } = new List<TestimonialViewModel>();
        public List<OfferViewModel> Offers { get; set; } = new List<OfferViewModel>();
        public List<FaqViewModel> Faqs { get; set; } = new List<FaqViewModel>();
        public SearchViewModel Search { get; set; } = new SearchViewModel();
        public string? CurrentLocation { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }

    public class OfferViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime ValidUntil { get; set; }
        public string RestaurantName { get; set; }
    }

    public class RestaurantHomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public decimal Rating { get; set; }
        public string DeliveryTime { get; set; }
        public double Distance { get; set; }
        public int PriceLevel { get; set; }
    }

    public class DishViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class TestimonialViewModel
    {
        public string Quote { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AvatarUrl { get; set; }
    }


    public class FaqViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public class SearchViewModel
    {
        public string Query { get; set; }
        public string Location { get; set; }
    }

    // error view model
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string? ErrorMessage { get; set; } = string.Empty;
    }
}
