using System.Threading.Tasks;
using CQRS.Example.CQRS;
using CQRS.Example.Customers.Commands;
using CQRS.Example.Customers.Domain;

namespace CQRS.Example.Configure
{
    internal static class CommandHandlerConfiguration
    {
        internal static Task RegisterCommandHandlers(ICommandSender commandSender)
        {
            return Task.Factory.StartNew(() => RegisterCommandHandlersSync(commandSender));
        }

        private static void RegisterCommandHandlersSync(ICommandSender commandSender)
        {
            commandSender.RegisterHandler<CustomerCommandHandler, CreateCustomer>();
            commandSender.RegisterHandler<CustomerCommandHandler, RenameCustomer>();
        }
    }
}
