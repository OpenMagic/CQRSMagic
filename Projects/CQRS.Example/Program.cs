using System;
using System.Threading.Tasks;
using CQRS.Example.Configure;
using CQRS.Example.CQRS;
using CQRS.Example.Customers.Commands;
using CQRS.Example.Customers.Domain;
using CQRS.Example.Support;

namespace CQRS.Example
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                var types = typeof(Program).Assembly.GetTypes();
                var container = ContainerConfiguration.Configure();

                Task.WaitAll(new[]
                {
                    container.GetInstance<ICommandSender>().RegisterHandlers(types)
                });

                EventPublisherConfiguration.RegisterEventHandlers(container.GetInstance<IEventPublisher>());

                var messageBus = container.GetInstance<IMessageBus>();
                var customerRepository = container.GetInstance<ICustomerRepository>();
                
                Console.WriteLine("Adding two customers...");

                messageBus.SendCommand(new CreateCustomer(Guid.NewGuid(), "Tim Murphy")).WaitAll();
                messageBus.SendCommand(new CreateCustomer(Guid.NewGuid(), "Nicole Dunkley")).WaitAll();

                customerRepository.GetAll().Result.AssertCustomerNamesAre(new[] { "Nicole Dunkley", "Tim Murphy" });

                Console.WriteLine("Changing customer name...");
                var nicole = customerRepository.FindByName("Nicole Dunkley").Result;

                messageBus.SendCommand(new RenameCustomer(nicole.CustomerId, "Nicole Murphy"));

                customerRepository.GetAll().Result.AssertCustomerNamesAre(new[] { "Nicole Dunkley", "Tim Murphy" });

                throw new NotImplementedException("replay");
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }

            Pause("Press any key to exit...");
        }

        private static void Pause(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ReadKey();
        }

        private static void WriteException(Exception exception)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
