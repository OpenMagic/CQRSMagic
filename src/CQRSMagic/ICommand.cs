using System;

namespace CQRSMagic
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
