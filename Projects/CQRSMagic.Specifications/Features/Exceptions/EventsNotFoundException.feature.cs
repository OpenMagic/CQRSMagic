#region Designer generated code

using TechTalk.SpecFlow;

#pragma warning disable

namespace CQRSMagic.Specifications.Features.Exceptions
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class EventsNotFoundExceptionFeature : Xunit.IUseFixture<EventsNotFoundExceptionFeature.FixtureData>, System.IDisposable
    {
        private static TechTalk.SpecFlow.ITestRunner testRunner;

#line 1 "EventsNotFoundException.feature"
#line hidden

        public EventsNotFoundExceptionFeature()
        {
            this.TestInitialize();
        }

        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            var featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "EventsNotFoundException", "", ProgrammingLanguage.CSharp, ((string[]) (null)));
            testRunner.OnFeatureStart(featureInfo);
        }

        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }

        public virtual void TestInitialize()
        {
        }

        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }

        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }

        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }

        public virtual void SetFixture(EventsNotFoundExceptionFeature.FixtureData fixtureData)
        {
        }

        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }

        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "EventsNotFoundException")]
        [Xunit.TraitAttribute("Description", "Message")]
        public virtual void Message()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Message", ((string[]) (null)));
#line 3
            this.ScenarioSetup(scenarioInfo);
#line 4
            testRunner.When("EventsNotFoundException is constructed with aggregate type \'SimpleFakeAggregate\' " +
                            "and aggregate id is \'a790e081-d3a7-4b63-96c9-4d00bba7e9db\'", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 5
            testRunner.Then("the exception message should be \'Cannot find events for SimpleFakeAggregate aggre" +
                            "gate with a790e081-d3a7-4b63-96c9-4d00bba7e9db id.\'", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            public FixtureData()
            {
                FeatureSetup();
            }

            void System.IDisposable.Dispose()
            {
                FeatureTearDown();
            }
        }
    }
}

#pragma warning restore

#endregion