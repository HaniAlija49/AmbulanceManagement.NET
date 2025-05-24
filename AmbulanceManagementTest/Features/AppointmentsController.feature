Feature: Appointments Management
  As an authorized user
  I want to manage patient appointments
  So that I can schedule, view, and update appointments for patients

  Background:
    Given I am logged in as an "Admin" user for appointments

  Scenario: View all appointments
    When I navigate to the "Appointments" page for appointments
    Then I should see a list of all appointments

Scenario: Add a new appointment for a patient
  When I submit a new appointment with the following details:
    | PatientId | AppointmentDate      | DoctorId |
    | Uvejs     | 05-15-2025 1200      | Adem     |
  Then the appointment with details "Uvejs, Adem" should be visible in the appointment list

Scenario: View details of an appointment
  Given an appointment with ID "9" exists
  When I view the details of appointment with ID "9"
  Then I should see the appointment details

Scenario: Update appointment details
  Given an appointment with ID "11" is present
  When I update the appointment details to change the "Doctor" to "John"
  Then the appointment details should reflect the new doctor "John"
