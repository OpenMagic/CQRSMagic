Feature: Example
	In order to learn CQRSMagic
	As a programmer
	I want an "end to end" example

Scenario: Add contact
	Given contact's name is Tim
	And their email address is tim@example.org
	When I send AddContact command
	Then ContactAdded event is added to the event store
	And contact is added to Contacts table
