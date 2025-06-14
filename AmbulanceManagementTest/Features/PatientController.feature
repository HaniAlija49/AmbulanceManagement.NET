  Feature: Patient Management
    As an authorized user
    I want to manage patient records
    So that I can maintain accurate and up-to-date patient information

    Background:
      Given I am logged in as a user with the "Nurse" role

    Scenario: View all patients
      When I navigate to the "Patients" page
      Then I should see a list of all patients

    Scenario: Search for patients by name
      When I navigate to the "Patients" page
      And I search for a patient with the name "Azem"
      Then I should see a list of patients containing "Azem"

   Scenario: Create a new patient
      When I submit a new patient with the following details:
        | Name | LastName | EmailAddress       | PhoneNumber | Age | Adress          |
        | Azem | Jovani  | azem.doe@test.com   | 1234567891  | 31  | Maple Street 2  |
      Then the patient "Azem Jovani" should be visible in the patient list


    Scenario: View patient details
      Given a patient named "Azem Jovani" exists
      When I view the details of patient "Azem Jovani"
      Then I should see the reports linked to their appointments

    Scenario: Edit an existing patient's age
       Given a patient named "Azem Jovani" exists
       When I update the age of "Azem Jovani" to "40"
       Then the patient details should reflect the new age

    Scenario: X_Delete a patient from the system
      When I navigate to the "Patients" page
      And I delete the patient with name "Azem Jovani"
      And I confirm the patient deletion
      Then the patient "Azem Jovani" should not appear in the list

