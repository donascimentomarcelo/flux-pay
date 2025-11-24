using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pay.Domain.Entity;
using Pay.Domain.Interfaces;
using Pay.Infrastructure.Database;

namespace Pay.Infrastructure.Repositories
{
    public class PaymentRepository(FluxPayDbContext context) : IPaymentRepository
    {
        private readonly FluxPayDbContext _context = context;

        public async Task CreateAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await _context.Payments.FirstAsync(p => p.Id == id);
        }
    }
}
