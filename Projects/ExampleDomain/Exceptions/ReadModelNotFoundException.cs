using System;

namespace ExampleDomain.Exceptions
{
    public class ReadModelNotFoundException : Exception
    {
        public ReadModelNotFoundException(string message) : base(message)
        {
        }
    }
}