using System;
using System.Reflection;

namespace CQRSMagic
{
    public class SubscriberInfo : ISubscriberInfo
    {
        public SubscriberInfo(TypeInfo typeInfo, Type[] implementedInterfaces)
        {
            TypeInfo = typeInfo;
            ImplementedInterfaces = implementedInterfaces;
        }

        public TypeInfo TypeInfo { get; private set; }
        public Type[] ImplementedInterfaces { get; private set; }
    }
}