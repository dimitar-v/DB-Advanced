namespace BillsPaymentSystem.App.Core.Commands
{
    using Models.Enums;
    using Contracts;
    using Data;
    using System;
    using System.Linq;
    using System.Text;

    public class UserInfoCommand : ICommand
    {
        private readonly BillsPaymentSystemContext context;

        public UserInfoCommand(BillsPaymentSystemContext context)
        {
            this.context = context;
        }

        public string Execute(string[] args)
        {
            var userId = int.Parse(args[0]);

            var user = context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new InvalidOperationException($"User with id {userId} not found!");
            }

            var sb = new StringBuilder();

            sb.AppendLine($"User: {user.FirstName} {user.LastName}");
            var pmBankAccounts = context.PaymentMethods
                .Where(pm => pm.UserId == userId && pm.BankAccountId != null)
                .ToArray();

            sb.AppendLine("Bank Accounts:");
            foreach (var pm in pmBankAccounts)
            {
                var ba = context.BankAccounts
                    .FirstOrDefault(b => b.BankAccountId == pm.BankAccountId);

                sb.AppendLine($"--ID: {ba.BankAccountId}");
                sb.AppendLine($"--- Balance: {ba.Balance:F2}");
                sb.AppendLine($"--- Bank: {ba.BankName}");
                sb.AppendLine($"--- SWIFT: {ba.SWIFT}");
            }

            var pmCreditCards = context.PaymentMethods
                .Where(pm => pm.UserId == userId && pm.CreditCardId != null)
                .ToArray();
            sb.AppendLine("Credit Cards:");
            foreach (var pm in pmCreditCards)
            {
                var cc = context.CreditCards
                    .FirstOrDefault(c => c.CreditCardId == pm.CreditCardId);

                sb.AppendLine($"--ID: {pm.CreditCardId}");
                sb.AppendLine($"--- Limit: {cc.Limit:F2}");
                sb.AppendLine($"--- Money Owed: {cc.MoneyOwed:F2}");
                sb.AppendLine($"--- Limit Left: {cc.LimitLeft:F2}");
                sb.AppendLine($"--- Expiration Date: {cc.ExpirationDate.ToString("yyyy/MM")}");
            }

            return sb.ToString().Trim();
        }
    }
}
