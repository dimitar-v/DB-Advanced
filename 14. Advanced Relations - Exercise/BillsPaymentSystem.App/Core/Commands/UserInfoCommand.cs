namespace BillsPaymentSystem.App.Core.Commands
{
    using Models.Enums;
    using Contracts;
    using Data;
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;

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

            var user = context.Users
                .Include(u => u.PaymentMethods)
                    .ThenInclude(pm => pm.BankAccount)
                .Include(u => u.PaymentMethods)
                    .ThenInclude(pm => pm.CreditCard)
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new InvalidOperationException($"User with id {userId} not found!");
            }

            var sb = new StringBuilder();
                        
            sb.AppendLine($"User: {user.FirstName} {user.LastName}");
            sb.AppendLine("Bank Accounts:");
            foreach (var pm in user.PaymentMethods.Where(pm => pm.Type == PaymentType.BankAccount))
            {            
                sb.AppendLine($"--ID: {pm.BankAccount.BankAccountId}");
                sb.AppendLine($"--- Balance: {pm.BankAccount.Balance:F2}");
                sb.AppendLine($"--- Bank: {pm.BankAccount.BankName}");
                sb.AppendLine($"--- SWIFT: {pm.BankAccount.SWIFT}");
            }

            sb.AppendLine("Credit Cards:");
            foreach (var pm in user.PaymentMethods.Where(pm => pm.Type == PaymentType.CreditCard))
            {
                sb.AppendLine($"--ID: {pm.CreditCardId}");
                sb.AppendLine($"--- Limit: {pm.CreditCard.Limit:F2}");
                sb.AppendLine($"--- Money Owed: {pm.CreditCard.MoneyOwed:F2}");
                sb.AppendLine($"--- Limit Left: {pm.CreditCard.LimitLeft:F2}");
                sb.AppendLine($"--- Expiration Date: {pm.CreditCard.ExpirationDate.ToString("yyyy/MM", DateTimeFormatInfo.InvariantInfo)}");
            }

            return sb.ToString().Trim();
        }
    }
}
