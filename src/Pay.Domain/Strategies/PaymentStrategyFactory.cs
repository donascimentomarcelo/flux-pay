using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pay.Domain.Enums;

namespace Pay.Domain.Strategies
{
    public class PaymentStrategyFactory
    {
        private readonly PixStrategy pix = new();
        private readonly CardStrategy card = new();
        private readonly BoletoStrategy boleto = new();

        public IPaymentMethodStrategy GetStrategy(PaymentMethod paymentMethod)
        {
            return paymentMethod switch
            {
                PaymentMethod.Pix => pix,
                PaymentMethod.Card => card,
                PaymentMethod.Boleto => boleto,
                _ => throw new NotImplementedException(
                    $"Payment method '{paymentMethod}' is not supported."
                ),
            };
        }
    }
}
