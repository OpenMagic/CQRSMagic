﻿#region Designer generated code

using TechTalk.SpecFlow;

#pragma warning disable

namespace CQRSMagic.Specifications.Features
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class EventStoreFeature : Xunit.IUseFixture<EventStoreFeature.FixtureData>, System.IDisposable
    {
        private static TechTalk.SpecFlow.ITestRunner testRunner;

#line 1 "EventStore.feature"
#line hidden

        public EventStoreFeature()
        {
            this.TestInitialize();
        }

        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            var featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "EventStore", "", ProgrammingLanguage.CSharp, ((string[]) (null)));
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

        public virtual void SetFixture(EventStoreFeature.FixtureData fixtureData)
        {
        }

        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }

        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "EventStore")]
        [Xunit.TraitAttribute("Description", "SaveEventsFor when aggregate does not exists")]
        public virtual void SaveEventsForWhenAggregateDoesNotExists()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("SaveEventsFor when aggregate does not exists", ((string[]) (null)));
#line 3
            this.ScenarioSetup(scenarioInfo);
#line 4
            testRunner.Given("a new aggregate", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 5
            testRunner.And("there are existing events", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "And ");
#line 6
            testRunner.And("there are new events", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "And ");
#line 7
            testRunner.When("SaveEventForEvents is called", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 8
            testRunner.Then("the events are saved to the database", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "EventStore")]
        [Xunit.TraitAttribute("Description", "SaveEventsFor when aggregate exists")]
        public virtual void SaveEventsForWhenAggregateExists()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("SaveEventsFor when aggregate exists", ((string[]) (null)));
#line 10
            this.ScenarioSetup(scenarioInfo);
#line 11
            testRunner.Given("an existing aggregate", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 12
            testRunner.And("there are new events", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "And ");
#line 13
            testRunner.When("SaveEventForEvents is called", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 14
            testRunner.Then("the events are saved to the database", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "EventStore")]
        [Xunit.TraitAttribute("Description", "GetAggregate when aggregate does not exist")]
        public virtual void GetAggregateWhenAggregateDoesNotExist()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("GetAggregate when aggregate does not exist", ((string[]) (null)));
#line 16
            this.ScenarioSetup(scenarioInfo);
#line 17
            testRunner.Given("aggregate does not exist", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 18
            testRunner.When("GetAggregate is called", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 19
            testRunner.Then("AggregateNotFoundException is thrown", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "EventStore")]
        [Xunit.TraitAttribute("Description", "GetAggregate when aggregate exists")]
        public virtual void GetAggregateWhenAggregateExists()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("GetAggregate when aggregate exists", ((string[]) (null)));
#line 21
            this.ScenarioSetup(scenarioInfo);
#line 22
            testRunner.Given("an existing aggregate", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 23
            testRunner.And("there are existing events", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "And ");
#line 24
            testRunner.When("GetAggregate is called", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 25
            testRunner.Then("the aggregate is found", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line 26
            testRunner.And("the events are sent to the aggregate", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }

        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "EventStore")]
        [Xunit.TraitAttribute("Description", "GetEventsFor when aggregate does not exist")]
        public virtual void GetEventsForWhenAggregateDoesNotExist()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("GetEventsFor when aggregate does not exist", ((string[]) (null)));
#line 28
            this.ScenarioSetup(scenarioInfo);
#line 29
            testRunner.Given("aggregate does not exist", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 30
            testRunner.When("GetEventsFor is called", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 31
            testRunner.Then("EventsNotFoundException is thrown", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [Xunit.FactAttribute()]
        [Xunit.TraitAttribute("FeatureTitle", "EventStore")]
        [Xunit.TraitAttribute("Description", "GetEventsFor when aggregate exists")]
        public virtual void GetEventsForWhenAggregateExists()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("GetEventsFor when aggregate exists", ((string[]) (null)));
#line 33
            this.ScenarioSetup(scenarioInfo);
#line 34
            testRunner.Given("an existing aggregate", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Given ");
#line 35
            testRunner.And("there are existing events", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "And ");
#line 36
            testRunner.When("GetEventsFor is called", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "When ");
#line 37
            testRunner.Then("the events are found", ((string) (null)), ((TechTalk.SpecFlow.Table) (null)), "Then ");
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