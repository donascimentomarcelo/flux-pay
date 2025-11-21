using Pay.Domain.Enums;

namespace Pay.Api.Dto
{
    public class CreatePayRequest
    {
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerEmail { get; set; }
        public required string ExternalReference { get; set; }
    }
}
