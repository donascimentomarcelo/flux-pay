using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pay.Domain.Entity;
using Pay.Infrastructure.Entities;

namespace Pay.Infrastructure.Database
{
    public class FluxPayDbContext : DbContext
    {
        public FluxPayDbContext(DbContextOptions<FluxPayDbContext> options)
            : base(options) { }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.CustomerName).HasMaxLength(200);
                entity.Property(e => e.CustomerEmail).HasMaxLength(200);
                entity.Property(e => e.Method).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<OutboxMessage>(entity =>
            {
                entity.ToTable("outbox");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Payload).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}
