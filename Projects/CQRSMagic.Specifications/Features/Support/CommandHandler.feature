Feature: CommandHandler

Scenario: Constructor
	When I create a new CommandHandler with valid parameters
	Then AggregateType is set
	And CommandType is set
	And SendCommand works
