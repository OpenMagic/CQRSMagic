using System.Linq;
using System.Web.Mvc;
using ExampleDomain.Contacts.Queries.Repositories;
using ExampleMVCApplication.ViewModels.Home;

namespace ExampleMVCApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContactRepository ContactRepository;

        public HomeController(IContactRepository contactRepository)
        {
            ContactRepository = contactRepository;
        }

        public ViewResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Contacts = ContactRepository.FindAllContactsAsync().Result.OrderBy(c => c.Name)
            };

            return View(viewModel);
        }
    }
}