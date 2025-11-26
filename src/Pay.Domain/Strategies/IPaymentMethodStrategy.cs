using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pay.Domain.Entity;

namespace Pay.Domain.Strategies
{
    public interface IPaymentMethodStrategy
    {
        Task<PaymentStrategyResult> ProcessAsync(Payment payment);
    }
}
