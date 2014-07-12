using System.Collections.Generic;
using System.Linq;
using ExampleDomain.Contacts.Commands;

namespace CQRSMagic.Specifications.Support
{
    public static class FakeCommands
    {
        public static IEnumerable<AddContact> AddContactCommands(int count)
        {
            return Enumerable.Range(1, count).Select(i => new AddContact
            {
                EmailAddress = string.Format("fake{0:D4}@example.com", i),
                Name = string.Format("Fake Name {0:D4}", i)
            });
        }
    }
}
