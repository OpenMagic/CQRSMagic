Feature: SubscribeEventHandlersIn
	As a developer using CQRSMagic
	I want to subscribe all event handlers I have created in my application

Background:
	Given I have a new EventPublisher
	
Scenario: Assembly with event handlers
	Given I have an application with event handlers
	When I call EventPublisher.SubscribeEventHandlersIn
	Then all event handlers are subscribed
