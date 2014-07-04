Feature: Example
	In order to learn CQRSMagic
	As a programmer
	I want an "end to end" example

Scenario: Add contact
	Given contact's name is Tim
	And their email address is tim@example.org
	When I send AddContact command
	Then ContactAdded event is added to event store
	And ContactAggregate can be retrieved from event store
	And ContactQueryModel is added to Contacts table
