namespace BillsPaymentSystem.App
{
    using Models;
    using Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DbInitializer
    {
        public static void Seed(BillsPaymentSystemContext context)
        {
            SeedUsers(context);

            SeedCreditCart(context);

            SeedBankAccount(context);

            SeedPaymentMethod(context);
        }

        private static void SeedPaymentMethod(BillsPaymentSystemContext context)
        {
            var payment = new PaymentMethod
            {
                Type = Models.Enums.PaymentType.BankAccount,
                UserId = 1,
                BankAccountId = 1
            };

            if (IsValid(payment))
            {
                context.PaymentMethods.Add(payment);
            }
                   
            payment = new PaymentMethod
            {
                Type = Models.Enums.PaymentType.CreditCard,
                UserId = 2,
                CreditCardId = 1
            };

            if (IsValid(payment))
            {
                context.PaymentMethods.Add(payment);
            }

            payment = new PaymentMethod
            {
                UserId = 3,
                BankAccountId = 2,
                CreditCardId = 2
            };

            if (IsValid(payment))
            {
                context.PaymentMethods.Add(payment);
            }

            payment = new PaymentMethod
            {
                Type = Models.Enums.PaymentType.CreditCard,
                UserId = 4
            };

            if (IsValid(payment))
            {
                context.PaymentMethods.Add(payment);
            }
                       
            context.SaveChanges();
        }

        private static void SeedBankAccount(BillsPaymentSystemContext context)
        {
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                var bankAccount = new BankAccount
                {
                    Balance = random.Next(-50, 2000),
                    BankName = i + "Bank",
                    SWIFT = $"BGSFB{i}"
                };

                if (!IsValid(bankAccount))
                {
                    continue;
                }

                context.BankAccounts.Add(bankAccount);
            }

            context.SaveChanges();
        }

        private static void SeedCreditCart(BillsPaymentSystemContext context)
        {
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                var creditCard = new CreditCard
                {
                    Limit = random.Next(-50, 2000),
                    MoneyOwed = random.Next(-50, 2000),
                    ExpirationDate = DateTime.Now.AddDays(random.Next(-50, 20))
                };

                if (!IsValid(creditCard))
                {
                    continue;
                }

                context.CreditCards.Add(creditCard);
            }

            context.SaveChanges();
        }

        private static void SeedUsers(BillsPaymentSystemContext context)
        {
            string[] firstNames = { "Ivan", "Veso", "Gosho", "Pesho", null, "" };
            string[] lastNames = { "Ivanov", "Veselinov", "Goshev", "Peshov", null, "ERROR" };
            string[] emails = { "ivanov@gmail.com", "veso@iclowd.com", "goshev@abv.bg", "pesho@mail.bg", null, "ERROR" };
            string[] passwords = { "ivano456v", "veso324%^", "Go6@", "pesho456bg", null, "ERROR" };

            for (int i = 0; i < firstNames.Length; i++)
            {
                var user = new User
                {
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    Email = emails[i],
                    Password = passwords[i]
                };

                if (!IsValid(user))
                {
                    continue;
                }

                context.Users.Add(user);
            }

            context.SaveChanges();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(entity, validationContext, validationResults, true);
        }
    }
}
