Feature: Contacts

Scenario: Add contact
	Given name is Tim
	And emailAddress is tim@example.com
	When AddContact command is sent
	Then ContactAdded event is added to the event store
	And ContactAggregate can be read from the event store
	And Contact is added to ContactRepository