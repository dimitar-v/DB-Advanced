namespace BillsPaymentSystem.App.IO
{
    using Contracts;
    using System;

    public class ConsoleReader : IReader
    {
        public string ReadLine()
            => Console.ReadLine();
    }
}
