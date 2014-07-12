using System.Collections.Generic;
using ExampleDomain.Contacts.Queries.Models;

namespace ExampleMVCApplication.ViewModels.Home
{
    public class IndexViewModel
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public IEnumerable<ContactReadModel> Contacts { get; set; }
    }
}