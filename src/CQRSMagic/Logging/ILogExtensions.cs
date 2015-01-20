using System;
using Common.Logging;

namespace CQRSMagic.Logging
{
    // ReSharper disable once InconsistentNaming
    public static class ILogExtensions
    {
        public static void ElapsedTime(this ILog log, string method, TimeSpan elapsedTime, int? expectedMaximumMilliseconds)
        {
            if (!expectedMaximumMilliseconds.HasValue || elapsedTime.TotalMilliseconds < expectedMaximumMilliseconds)
            {
                log.TraceFormat("{0} took {1:N0}ms.", method, elapsedTime.TotalMilliseconds);
            }
            else
            {
                log.TraceFormat("{0} excepted expected {1:N0}ms. It took {2:N0}ms.", method, expectedMaximumMilliseconds, elapsedTime.TotalMilliseconds);
            }
        }

        public static void Trace(this ILog log, string format, params object[] args)
        {
            log.TraceFormat(format, args);
        }
    }
}
