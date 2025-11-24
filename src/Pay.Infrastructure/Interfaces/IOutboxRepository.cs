using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pay.Infrastructure.Entities;

namespace Pay.Infrastructure.Interfaces
{
    public interface IOutboxRepository
    {
        Task AddAsync(OutboxMessage message);
    }
}
