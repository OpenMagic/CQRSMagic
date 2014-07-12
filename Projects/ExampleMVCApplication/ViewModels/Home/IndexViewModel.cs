using System.Collections.Generic;
using System.ComponentModel;
using EmptyStringGuard;
using EmptyStringGuardValidationFlags = EmptyStringGuard.ValidationFlags;
using ExampleDomain.Contacts.Queries.Models;
using NullGuard;
using NullGuardValidationFlags = NullGuard.ValidationFlags;

namespace ExampleMVCApplication.ViewModels.Home
{
    [NullGuard(NullGuardValidationFlags.Methods)]
    [EmptyStringGuard(EmptyStringGuardValidationFlags.Methods)]
    public class IndexViewModel
    {
        public string Name { get; set; }

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        public IEnumerable<ContactReadModel> Contacts { get; set; }
    }
}