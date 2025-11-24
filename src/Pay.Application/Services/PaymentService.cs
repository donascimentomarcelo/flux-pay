using Pay.Application.DTOs;
using Pay.Domain.Entity;
using Pay.Domain.Enums;
using Pay.Domain.Interfaces;

namespace Pay.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<CreatePayResponse> Create(CreatePaymentRequest request)
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

            await _paymentRepository.CreateAsync(payment);

            return new CreatePayResponse
            {
                PaymentId = payment.Id,
                Status = payment.Status.ToString(),
            };
        }
    }
}
