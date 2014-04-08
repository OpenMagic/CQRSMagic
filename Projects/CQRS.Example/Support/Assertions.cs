using System;
using System.Collections.Generic;
using System.Linq;
using CQRS.Example.Customers.Domain;

namespace CQRS.Example.Support
{
    public static class Assertions
    {
        public static void AssertCustomerNamesAre(this IEnumerable<Customer> customers, IEnumerable<string> expectedNames)
        {
            AssertCustomerNamesAre(customers.ToArray(), expectedNames.ToArray());
        }

        private static void AssertCustomerNamesAre(this ICollection<Customer> customers, IList<string> expectedNames)
        {
            if (customers.Count != expectedNames.Count)
            {
                throw new Exception(string.Format("Expected {0:N0} customers but found {1:N0} customers.", expectedNames.Count, customers.Count));
            }

            var actualNames = customers.Select(c => c.Name);
            var compareNames = actualNames.Select((n, i) => new {AreEqual = (n == expectedNames[i]), ActualName = n, ExpectedName = expectedNames[i]});
            var notEqualNames = compareNames.Where(x => !x.AreEqual).ToArray();

            if (notEqualNames.Length > 0)
            {
                throw new Exception(string.Format("{0:N0} customer names are not equal:\r\n{1}", notEqualNames.Length, string.Join("\r\n", notEqualNames.Select(x => string.Format("Actual name: {0} != Expected name: {1}", x.ActualName, x.ExpectedName)))));
            }
        }
    }
}
