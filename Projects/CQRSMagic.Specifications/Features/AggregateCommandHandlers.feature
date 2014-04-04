Feature: AggregateCommandHandlers

Scenario: Constructor
	When I create new AggregateCommandHandlers
	Then AggregateType equals typeof(TAggregate)
