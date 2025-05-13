Feature: Appointment Booking

Scenario: Make an appointment from the homepage
  Given I open the Welcome page
  When I click the Make Appointment button
    And I fill in name "John Doe" and email "johndoe@mail.com"
    And I choose appointment hour "H08_00"
    And I select date "20202022222222"
    And I choose a doctor
    And I submit the appointment form
  Then I should return to the Welcome page
    And I should not see an error message
