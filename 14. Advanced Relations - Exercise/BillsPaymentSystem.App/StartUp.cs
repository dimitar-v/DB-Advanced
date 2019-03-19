namespace BillsPaymentSystem.App
{
    using Core;
    using Data;
    using IO;

    public class StartUp
    {
        static void Main(string[] args)
        {
            // 2.	Seed Some Data
            //using (var context = new BillsPaymentSystemContext())
            //{
            //    DbInitializer.Seed(context);
            //}

            // 3.	User Details
            var reader = new ConsoleReader();
            var writer = new ConsoleWriter();
            var commandInterpreter = new CommandInterpreter();
            var engine = new Engine(commandInterpreter, reader, writer);
            engine.Run();
        }
    }
}
