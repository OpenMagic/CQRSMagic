@SubscribeEventHandlers
Feature: SubscribeEventHandlers
	As a developer
	I want CQRSMagic to auto-subscribe all event handlers to its event publisher
	So I want don't need to remember to do it when I add new event handlers

Background:
	Given I have a new AssemblyEventSubsciber

Scenario: Finds all events handlers in an assembly and subscribes them to an event publisher
	Given I have an application with event handlers
	And I have an EventPublisher
	When I call AssemblyEventSubscriber.SubscribeEventHandlers
	Then all event handlers are subscribed
