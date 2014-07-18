Feature: EventHandlers

Background: 
	Given DependencyResolver
	And EventHandlers

Scenario: RegisterEventHandlers(Assembly searchAssembly)
	Given an assembly with ISubscribeTo handlers
	When RegisterEventHandlers is called
	Then one handler is created for each ISubscribeTo handler
