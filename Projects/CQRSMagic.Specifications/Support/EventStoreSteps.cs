using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Support
{
    [Binding]
    public class ToDoSteps
    {
        [When(@"todo")]
        public void WhenTodo()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"todo")]
        public void ThenTodo()
        {
            ScenarioContext.Current.Pending();
        }
    }
}