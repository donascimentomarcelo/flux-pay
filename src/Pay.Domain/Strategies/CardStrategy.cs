using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pay.Domain.Entity;

namespace Pay.Domain.Strategies
{
    public class CardStrategy : IPaymentMethodStrategy
    {
        public Task<PaymentStrategyResult> ProcessAsync(Payment payment)
        {
            bool approved = Random.Shared.Next(0, 100) < 75;

            return Task.FromResult(
                new PaymentStrategyResult
                {
                    Success = approved,
                    Message = approved ? "Card authorized." : "Card declined by issuer.",
                }
            );
        }
    }
}
