namespace BillsPaymentSystem.App.Core
{
    using Commands.Contracts;
    using Contracts;
    using Data;
    using System;
    using System.Linq;
    using System.Reflection;

    public class CommandInterpreter : ICommandInterpreter
    {
        private const string postfix = "Command";

        public string Read(string[] args, BillsPaymentSystemContext context)
        {
            var command = args[0];
            var commandArgs = args.Skip(1).ToArray();

            var type = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == command + postfix);

            if (type == null)
            {
                throw new InvalidOperationException("Invalid command!");
            }

            var typeInstance = Activator.CreateInstance(type, context);

            return ((ICommand)typeInstance).Execute(commandArgs);
        }
    }
}
