using Pay.Infrastructure.Database;
using Pay.Infrastructure.Entities;
using Pay.Infrastructure.Interfaces;

namespace Pay.Infrastructure.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly FluxPayDbContext _dbContext;

        public OutboxRepository(FluxPayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(OutboxMessage message)
        {
            await _dbContext.OutboxMessages.AddAsync(message);
            await _dbContext.SaveChangesAsync();
        }
    }
}
