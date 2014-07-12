using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CQRSMagic;
using ExampleDomain.Contacts.Commands;
using ExampleDomain.Contacts.Queries.Models;
using ExampleDomain.Contacts.Queries.Repositories;
using ExampleMVCApplication.ViewModels.Home;

namespace ExampleMVCApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageBus MessageBus;
        private readonly IContactRepository ContactRepository;

        static HomeController()
        {
            Mapper.CreateMap<IndexViewModel, AddContact>();
        }

        public HomeController(IMessageBus messageBus, IContactRepository contactRepository)
        {
            MessageBus = messageBus;
            ContactRepository = contactRepository;
        }

        public ViewResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Contacts = GetContacts()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var command = Mapper.Map<AddContact>(viewModel);
                MessageBus.SendCommandAsync(command).Wait();

                return RedirectToAction("Index");
            }

            viewModel.Contacts = GetContacts();

            return View(viewModel);
        }

        private IEnumerable<ContactReadModel> GetContacts()
        {
            return ContactRepository.FindAllContactsAsync().Result.OrderBy(c => c.Name);
        }
    }
}