Feature: Adding To Queue
	In order to build my movie list
	As MovieMon Member
	I want to Add movies to my queue

@mytag
Scenario: Add Movie
	Given I have a BitterBidder Account
	And the movie The Godfather is in my search results
	When I press Add To Queue
	Then the movie The Godfather should be in my queue


