using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pay.Domain.Dto.Enums;

namespace Pay.Domain.Entity
{
    public class Pay
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Amount { get; set; }
        public PayMethod Method { get; set; }
        public PayStatus Status { get; set; } = PayStatus.Pending;

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }

        public string ExternalReference { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
