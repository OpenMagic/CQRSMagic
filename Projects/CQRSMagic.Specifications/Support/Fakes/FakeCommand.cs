using System;

namespace CQRSMagic.Specifications.Support.Fakes
{
    public class FakeCommand : Command
    {
        public FakeCommand()
            : this(Guid.NewGuid())
        {
        }

        public FakeCommand(Guid aggregateId)
            : base(aggregateId)
        {
        }
    }
}