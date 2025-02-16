using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Complaints
{
    public class ComplaintCreateDTO
    {
        public string UserID { get; set; }
        public int OrderID { get; set; }
        public string Description { get; set; }
    }
}
