namespace FoodDeliveryApp.ViewModels.Profile
{
    public class AddressViewModel
    {
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public string? AddressName { get; set; } = string.Empty;
        public string? Street { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? ZipCode { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt
        {
            get; set;
        }
    }

    
    public class AddressListViewModel
    {
        public int CustomerId { get; set; }
        public List<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();
        public int TotalCount { get; set; }
    }

}
