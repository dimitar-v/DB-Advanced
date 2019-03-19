namespace BillsPaymentSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            PaymentMethods = new HashSet<PaymentMethod>();
        }

        public int UserId { get; set; }

        [Required]
        [MinLength(3), MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3), MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")]
        [MaxLength(80)]
        public string Email { get; set; }

        [Required]
        [MinLength(6), MaxLength(25)]
        public string Password { get; set; }

        public  ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
