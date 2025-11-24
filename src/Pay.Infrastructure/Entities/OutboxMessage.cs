using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pay.Infrastructure.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public string? Error { get; set; }
    }
}
