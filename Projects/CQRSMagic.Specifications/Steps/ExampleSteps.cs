using OpenMagic.Exceptions;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Steps
{
    [Binding]
    public class ExampleSteps
    {
        [Given(@"contact's name is Tim")]
        public void GivenContactSNameIsTim()
        {
            throw new ToDoException();
        }

        [Given(@"their email address is tim@example\.org")]
        public void GivenTheirEmailAddressIsTimExample_Org()
        {
            throw new ToDoException();
        }

        [When(@"I send AddContact command")]
        public void WhenISendAddContactCommand()
        {
            throw new ToDoException();
        }

        [Then(@"ContactAdded event is added to the event store")]
        public void ThenContactAddedEventIsAddedToTheEventStore()
        {
            throw new ToDoException();
        }

        [Then(@"contact is added to Contacts table")]
        public void ThenContactIsAddedToContactsTable()
        {
            throw new ToDoException();
        }
    }
}