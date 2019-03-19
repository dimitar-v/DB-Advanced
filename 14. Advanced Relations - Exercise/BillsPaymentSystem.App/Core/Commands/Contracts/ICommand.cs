namespace BillsPaymentSystem.App.Core.Commands.Contracts
{
    interface ICommand
    {
        string Execute(string[] args);
    }
}
