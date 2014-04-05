using System.Reflection;
using CQRSMagic.Support;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace CQRSMagic.Specifications.Features.Support.Steps
{
    [Binding]
    public class StaticReflectionSteps
    {
        private MemberInfo MemberInfo;

        [When(@"I call GetMemberInfo")]
        public void WhenICallGetMemberInfo()
        {
            MemberInfo = StaticReflection.GetMemberInfo<IHandleCommand<ICommand>>(x => x.HandleCommand((ICommand)null, (IEventStore)null));
        }

        [Then(@"I should get the MemberInfo")]
        public void ThenIShouldGetTheMemberInfo()
        {
            MemberInfo.Name.Should().Be("HandleCommand");
        }
    }
}
