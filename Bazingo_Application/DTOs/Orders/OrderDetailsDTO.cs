using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Orders
{
    public class OrderDetailsDTO
    {
        public int OrderID { get; set; }
        public List<OrderProductDTO> Products { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
