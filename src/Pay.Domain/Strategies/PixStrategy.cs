using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pay.Domain.Entity;

namespace Pay.Domain.Strategies
{
    public class PixStrategy : IPaymentMethodStrategy
    {
        public Task<PaymentStrategyResult> ProcessAsync(Payment payment)
        {
            return Task.FromResult(
                new PaymentStrategyResult
                {
                    Success = true,
                    Message = "Payment processed with Pix strategy.",
                }
            );
        }
    }
}
