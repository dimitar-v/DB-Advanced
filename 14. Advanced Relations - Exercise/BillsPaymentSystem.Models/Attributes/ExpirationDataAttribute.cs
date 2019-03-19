namespace BillsPaymentSystem.Models.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property)]
    public class ExpirationDataAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            => DateTime.Now > (DateTime)value ? new ValidationResult("Card is expired!") : ValidationResult.Success;
        //{
        //    var currentDateTime = DateTime.Now;
        //    var targetDateTime = (DateTime)value;

        //    if (currentDateTime > targetDateTime)
        //    {
        //        return new ValidationResult("Card is expired");
        //    }

        //    return ValidationResult.Success;
        //}
    }
}
