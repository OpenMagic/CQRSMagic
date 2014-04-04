using System;

namespace CQRSMagic.Specifications.Support.Fakes
{
    public class FakeCommand : Command
    {
        public FakeCommand(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}