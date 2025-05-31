Feature: User lifecycle

Scenario: Admin logs into the system
  Given I am on the login page
  When I enter email "admin@gmail.com" and password "Admin123@"
    And I click the login button
  Then I should be redirected to the dashboard

Scenario: Register a new user
  Given I am logged in as admin
  And I am on the register page
  When I fill in the registration form with test data
  And I upload a profile picture
  And I submit the registration form
  Then I should be redirected to the user list

  Scenario: Delete the newly registered user
  Given I am logged in as admin
  And I am on the user list page
  When I click the Delete button for the test user
  And I confirm the deletion as admin
  Then I should be redirected to the user list
  And I should not see the test user in the list