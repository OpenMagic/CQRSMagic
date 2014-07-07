using System;
using FluentAssertions;

namespace CQRSMagic.Specifications.Support
{
    internal class ActionShould
    {
        internal static void Throw<TException>(Action action) where TException : Exception
        {
            action.ShouldThrow<TException>();
        }

        internal static void Throw<TArgumentException>(string paramName, Action action) where TArgumentException : ArgumentException
        {
            action.ShouldThrow<TArgumentException>().And.ParamName.Should().Be(paramName);
        }
    }
}
