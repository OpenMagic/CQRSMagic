using System.Reflection;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Support.Common
{
    [Binding]
    public class CommonSteps
    {
        private readonly CommonData _commonData;

        public CommonSteps(CommonData commonData)
        {
            _commonData = commonData;
        }

        [Given(@"I have an application with event handlers")]
        public void GivenIHaveAnApplicationWithEventHandlers()
        {
            _commonData.Assembly = Assembly.GetExecutingAssembly();
        }
    }
}
