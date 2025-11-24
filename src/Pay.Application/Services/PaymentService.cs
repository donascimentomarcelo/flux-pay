using System.Text.Json;
using Pay.Application.DTOs;
using Pay.Domain.Entity;
using Pay.Domain.Enums;
using Pay.Domain.Interfaces;
using Pay.Infrastructure.Entities;
using Pay.Infrastructure.Interfaces;

namespace Pay.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOutboxRepository _outboxRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOutboxRepository outboxRepository
        )
        {
            _paymentRepository = paymentRepository;
            _outboxRepository = outboxRepository;
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

            AddToOutbox(payment);

            return new CreatePayResponse
            {
                PaymentId = payment.Id,
                Status = payment.Status.ToString(),
            };
        }

        private async void AddToOutbox(Payment payment)
        {
            var message = new OutboxMessage
            {
                Type = "PaymentCreated",
                Payload = JsonSerializer.Serialize(
                    new
                    {
                        payment.Id,
                        payment.Amount,
                        payment.Method,
                        payment.Status,
                        payment.CustomerName,
                        payment.CustomerEmail,
                        payment.ExternalReference,
                    }
                ),
            };

            await _outboxRepository.AddAsync(message);
        }
    }
}
