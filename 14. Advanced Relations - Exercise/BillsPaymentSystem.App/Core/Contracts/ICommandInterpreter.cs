namespace BillsPaymentSystem.App.Core.Contracts
{
    using Data;

    public interface ICommandInterpreter
    {
        string Read(string[] args, BillsPaymentSystemContext context);
    }
}
