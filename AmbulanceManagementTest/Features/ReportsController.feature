Feature: Manage Medical Reports
  As a doctor or admin
  I want to view, create, edit, delete, and print medical reports
  So that I can manage patient treatment history

  Scenario: View the list of reports
    Given I navigate to the Reports Index page
    Then I should see a table of reports

  Scenario: View details for a specific report
    Given I navigate to the Reports Index page
    When I click on the visit date of the first report
    Then I should see the report detail page
    And I should see the patient name, doctor name, and diagnosis

  Scenario: Create a new report
    Given I navigate to the Create Report page
    When I select an appointment and a doctor
      And I enter visit date "2020202222222"
      And I enter symptoms "High fever"
      And I enter diagnosis "Flu"
      And I enter prescriptions "Rest and hydration"
      And I submit the create form
    Then I should be redirected to the Reports Index

  Scenario: Edit an existing report
    Given I navigate to the Reports Index page
    When I click the Edit button for the first report
      And I change the diagnosis to "Common cold"
      And I submit the edit form
    Then I should be redirected to the Reports Index

  Scenario: Delete a report
    Given I navigate to the Reports Index page
    When I click the Delete button for the first report
      And I confirm the deletion
    Then I should be redirected to the Reports Index

  Scenario: Print a report
    Given I navigate to the Reports Index page
    When I click on the visit date of the first report
    When I click the Print button for the first report
    Then a new tab should open with the printable report
