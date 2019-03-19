namespace BillsPaymentSystem.App.Core
{
    using BillsPaymentSystem.App.IO.Contracts;
    using Contracts;
    using Data;
    using System;

    public class Engine : IEngine
    {
        private readonly ICommandInterpreter commandInterpreter;
        private readonly IReader reader;
        private readonly IWriter writer;

        public Engine(ICommandInterpreter commandInterpreter, IReader reader, IWriter writer)
        {
            this.commandInterpreter = commandInterpreter;
            this.reader = reader;
            this.writer = writer;
        }

        public void Run()
        {
            string input;

            while ((input = reader.ReadLine()) != "Exit")
            {
                var args = input.Split(" ",StringSplitOptions.RemoveEmptyEntries);

                using (var context = new BillsPaymentSystemContext())
                {
                    string result;

                    try
                    {
                        result = commandInterpreter.Read(args, context);
                    }
                    catch (InvalidOperationException ex)
                    {
                        result = "ERROR: " + ex.Message;
                    }
                    catch (Exception ex)
                    {
                        result = "ERROR: " + ex.Message;
                    }
                    
                    writer.WriteLine(result);
                }
            }
        }
    }
}
