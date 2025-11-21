using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pay.Application.DTOs
{
    public class CreatePayResponse
    {
        public required Guid PaymentId { get; set; }
        public required string Status { get; set; }
    }
}
