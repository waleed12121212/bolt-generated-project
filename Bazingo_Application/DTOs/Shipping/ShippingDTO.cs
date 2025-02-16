using System.ComponentModel.DataAnnotations;

namespace Bazingo_Application.DTOs.Shipping
{
    public class ShippingDTO
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingStatus { get; set; }
    }

    public class UpdateShippingDto
    {
        [Required]
        public string Status { get; set; }
    }
}
