Feature: Command

Scenario: Constructor
	Given AggregateId is "d1e836de-d969-4fd8-a946-1c00217259c0"
	When I create a new Command
	Then Command.AggregateId should equal AggregateId
