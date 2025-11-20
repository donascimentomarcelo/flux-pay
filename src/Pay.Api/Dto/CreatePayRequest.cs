using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pay.Api.Dto
{
    public class CreatePayRequest
    {
        public decimal Amount { get; set; }
        public PayMethod Method { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ExternalReference { get; set; }
    }
}
