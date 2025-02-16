using System;

namespace Bazingo_Application.DTOs.Shipping
{
    public class UpdateShippingStatusDto
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string TrackingNumber { get; set; }
        public string Notes { get; set; }
    }
}
