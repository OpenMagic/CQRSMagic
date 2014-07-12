using System;
using System.Collections.Generic;
using System.Linq;
using ExampleDomain.Contacts.Queries.Models;

namespace CQRSMagic.Specifications.Support
{
    public static class FakeModels
    {
        public static IEnumerable<ContactReadModel> Contacts(int count)
        {
            return Enumerable.Range(1, count).Select(i => new ContactReadModel(Guid.NewGuid(), string.Format("Fake Name {0:D4}", i), string.Format("fake{0:D4}@example.org", i)));
        }
    }
}