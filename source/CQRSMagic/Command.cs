using System;

namespace CQRSMagic
{
    public class Command : ICommand
    {
        private readonly Guid _id;

        public Command(Guid id)
        {
            _id = id;
        }

        public Guid Id
        {
            get { return _id; }
        }
    }
}