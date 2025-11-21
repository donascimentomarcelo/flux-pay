using Pay.Application.DTOs;
using Pay.Domain.Entity;
using Pay.Domain.Enums;

namespace Pay.Application.Services
{
    public class PaymentService : IPaymentService
    {
        public CreatePayResponse Create(CreatePaymentRequest request)
        {
            var payment = new Payment
            {
                Amount = request.Amount,
                Method = request.Method,
                Status = PaymentStatus.Pending,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                ExternalReference = request.ExternalReference,
                CreatedAt = DateTime.UtcNow,
            };
            return new CreatePayResponse
            {
                PaymentId = payment.Id,
                Status = payment.Status.ToString(),
            };
        }
    }
}
