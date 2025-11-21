using Pay.Application.DTOs;

namespace Pay.Application.Services
{
    public interface IPaymentService
    {
        public CreatePayResponse Create(CreatePaymentRequest request);
    }
}
