Feature: StaticReflection

Scenario: GetMemberInfo
	When I call GetMemberInfo
	Then I should get the MemberInfo
