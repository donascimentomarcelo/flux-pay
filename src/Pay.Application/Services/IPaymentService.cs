using Pay.Application.DTOs;

namespace Pay.Application.Services
{
    public interface IPaymentService
    {
        public Task<CreatePayResponse> Create(CreatePaymentRequest request);
    }
}
