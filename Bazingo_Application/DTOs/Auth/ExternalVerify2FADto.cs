using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Application.DTOs.Auth
{
    public class ExternalVerify2FADto
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}
