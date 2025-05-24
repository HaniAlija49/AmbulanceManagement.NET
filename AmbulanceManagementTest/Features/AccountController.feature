Feature: Login

Scenario: Admin logs into the system
  Given I am on the login page
  When I enter email "admin@gmail.com" and password "Admin123@"
    And I click the login button
  Then I should be redirected to the dashboard