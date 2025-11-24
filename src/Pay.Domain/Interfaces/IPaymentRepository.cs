using Pay.Domain.Entity;

namespace Pay.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task CreateAsync(Payment payment);
        Task<Payment?> GetByIdAsync(Guid id);
    }
}
