using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public IEnumerable<ContactReadModel> Contacts { get; set; }
    }
}