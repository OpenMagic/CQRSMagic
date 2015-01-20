using System;
using System.Reflection;

namespace CQRSMagic
{
    public interface ISubscriberInfo
    {
        TypeInfo TypeInfo { get; }
        Type[] ImplementedInterfaces { get; }
    }
}