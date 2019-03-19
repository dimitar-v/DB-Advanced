namespace BillsPaymentSystem.Models
{
    using BillsPaymentSystem.Models.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreditCard
    {
        public int CreditCardId { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Limit { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal MoneyOwed { get; set; }

        public decimal LimitLeft 
            => Limit - MoneyOwed;

        [ExpirationData]
        public DateTime ExpirationDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}
