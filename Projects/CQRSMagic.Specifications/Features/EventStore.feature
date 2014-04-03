Feature: EventStore

Scenario: SaveEventsFor when aggregate does not exists
	Given a new aggregate
	And there are existing events
	And there are new events
	When SaveEventForEvents is called
	Then the events are saved to the database

Scenario: SaveEventsFor when aggregate exists
	Given an existing aggregate
	And there are new events
	When SaveEventForEvents is called
	Then the events are saved to the database

Scenario: GetAggregate when aggregate does not exist
	Given aggregate does not exist
	When GetAggregate is called
	Then AggregateNotFoundException is thrown

Scenario: GetAggregate when aggregate exists
	Given an existing aggregate
	And there are existing events
	When GetAggregate is called
	Then the aggregate is found
	And the events are sent to the aggregate

Scenario: GetEventsFor when aggregate does not exist
	Given aggregate does not exist
	When GetEventsFor is called
	Then EventsNotFoundException is thrown

Scenario: GetEventsFor when aggregate exists
	Given an existing aggregate
	And there are existing events
	When GetEventsFor is called
	Then the events are found

