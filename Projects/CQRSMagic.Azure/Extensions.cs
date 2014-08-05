using System;

namespace CQRSMagic.Azure
{
    public static class Extensions
    {
        public static Guid ToAggregateId(this string partitionKey)
        {
            return new Guid(partitionKey);
        }

        public static DateTimeOffset ToEventCreated(this string rowKey)
        {
            var cleanRowKey = rowKey.RemoveTransactionIndexFromRowKey();
            var value = DateTimeOffset.Parse(cleanRowKey);

            return value;
        }

        public static string RemoveTransactionIndexFromRowKey(this string rowKey)
        {
            var index = rowKey.LastIndexOf('-');
            var value = rowKey.Substring(0, index);

            return value;
        }
    }
}
