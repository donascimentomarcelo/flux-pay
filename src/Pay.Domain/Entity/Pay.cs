using Pay.Domain.Enums;

namespace Pay.Domain.Entity
{
    public class Pay
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Amount { get; set; }
        public PayMethod Method { get; set; }
        public PayStatus Status { get; set; } = PayStatus.Pending;

        public required string CustomerName { get; set; }
        public required string CustomerEmail { get; set; }

        public required string ExternalReference { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
