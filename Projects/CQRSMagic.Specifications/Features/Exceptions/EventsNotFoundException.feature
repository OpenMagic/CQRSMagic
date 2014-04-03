Feature: EventsNotFoundException

Scenario: Message
	When EventsNotFoundException is constructed with aggregate type 'SimpleFakeAggregate' and aggregate id is 'a790e081-d3a7-4b63-96c9-4d00bba7e9db'
	Then the exception message should be 'Cannot find events for SimpleFakeAggregate aggregate with a790e081-d3a7-4b63-96c9-4d00bba7e9db id.'
