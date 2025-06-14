using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace AmbulanceManagement.Tests.StepDefinitions
{
    [Binding]
    public class AppointmentsControllerStepDefinitions : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _baseUrl = "https://localhost:7287";

        public AppointmentsControllerStepDefinitions()
        {
            var options = new ChromeOptions();
            options.AddArgument("start-maximized");
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Given(@"I am logged in as an ""(.*)"" user for appointments")]
        public void GivenIAmLoggedInAsAUserForAppointments(string role)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Identity/Account/Login");

            if (role == "Admin")
            {
                _wait.Until(d => d.FindElement(By.Id("Email"))).SendKeys("admin@gmail.com");
                _driver.FindElement(By.Id("Password")).SendKeys("Admin123@");
            }
            else if (role == "User")
            {
                _wait.Until(d => d.FindElement(By.Id("Email"))).SendKeys("user@example.com");
                _driver.FindElement(By.Id("Password")).SendKeys("User123@");
            }

            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            _wait.Until(d => d.Url.Contains("Home"));
        }


        [When(@"I navigate to the ""(.*)"" page for appointments")]
        public void WhenINavigateToTheAppointmentsPage(string page)
        {
            if (page == "Appointments")
            {
                _driver.Navigate().GoToUrl($"{_baseUrl}/Appointments");
            }
            else
            {
                _driver.Navigate().GoToUrl($"{_baseUrl}/{page}");
            }

            _wait.Until(d => d.FindElement(By.CssSelector("h4.page-title")));
        }

        [Then(@"I should see a list of all appointments")]
        public void ThenIShouldSeeAListOfAllAppointments()
        {
            var appointmentRows = _wait.Until(d =>
                d.FindElements(By.CssSelector("table.table tbody tr"))
            );

            Assert.True(appointmentRows.Count > 0, "No appointment rows found in the appointment list table.");
        }


        [Given(@"an appointment with ID ""(.*)"" exists")]
        public void GivenAnAppointmentExistsWithId(int appointmentId)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Appointments");
            var appointmentLink = _wait.Until(d => d.FindElement(By.XPath($"//a[contains(@href, '/Appointments/Details/{appointmentId}')]")));
            Assert.NotNull(appointmentLink); 
        }

        [When(@"I view the details of appointment with ID ""(.*)""")]
        public void WhenIViewTheDetailsOfAppointmentWithId(int appointmentId)
        {
            var link = _wait.Until(d => d.FindElement(By.XPath($"//a[contains(@href, '/Appointments/Details/{appointmentId}')]")));
            link.Click(); 
        }

        [Then(@"I should see the appointment details")]
        public void ThenIShouldSeeTheAppointmentDetails()
        {
            var appointmentDetails = _wait.Until(d =>
                d.FindElement(By.CssSelector("dl.card-box")) != null 
            );

            Assert.True(appointmentDetails != null, "Appointment details not found.");
        }

        [When(@"I submit a new appointment with the following details:")]
        public void WhenISubmitANewAppointmentWithTheFollowingDetails(Table table)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Appointments/Create");

            var row = table.Rows.First();

            var patientDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id("PatientId"))));
            patientDropdown.SelectByText(row["PatientId"]);

            var doctorDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id("DoctorId"))));
            doctorDropdown.SelectByText(row["DoctorId"]);

            var appointmentDateField = _wait.Until(d => d.FindElement(By.Id("appointmentDate")));
            appointmentDateField.Clear(); 
            appointmentDateField.SendKeys(row["AppointmentDate"]); 

            string appointmentHour = "1200"; 
            var hourDropdown = new SelectElement(_wait.Until(d => d.FindElement(By.Id("appointmentHour"))));
            hourDropdown.SelectByValue(appointmentHour);

            var checkbox = _wait.Until(d => d.FindElement(By.Id("IsApproved")));
            if (!checkbox.Selected)
            {
                checkbox.Click();
            }

            var submitButton = _wait.Until(d => d.FindElement(By.CssSelector("input.btn-primary")));
            submitButton.Click();

            _wait.Until(d => d.FindElements(By.CssSelector(".table tbody tr")).Count > 0);
        }
        [Then(@"the appointment with details ""(.*), (.*)"" should be visible in the appointment list")]
        public void ThenTheAppointmentWithDetailsShouldBeVisibleInTheAppointmentList(string patientId, string doctorId)
        {

            var rows = _wait.Until(d => d.FindElements(By.CssSelector(".table tbody tr")));

            bool matchFound = rows.Any(row =>
            {
                var cells = row.FindElements(By.TagName("td"));
                return cells.Any(c => c.Text.Contains(patientId)) &&
                       cells.Any(c => c.Text.Contains(doctorId));
            });

            Assert.True(matchFound, $"Appointment with PatientId '{patientId}' and DoctorId '{doctorId}' not found in the UI.");
        }



        [Given(@"an appointment with ID ""(.*)"" is present")]
        public void GivenAnAppointmentWithIdIsPresent(int appointmentId)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Appointments/Edit/{appointmentId}");

            _wait.Until(d => d.FindElement(By.Id("DoctorId"))); 

            Assert.True(_driver.Url.Contains($"/Appointments/Edit/{appointmentId}"));
        }


        [When(@"I update the appointment details to change the ""(.*)"" to ""(.*)""")]
        public void WhenIUpdateTheAppointmentDetailsToChangeDoctorToJohn(string field, string newValue)
        {
            if (field.Equals("Doctor", StringComparison.OrdinalIgnoreCase))
            {

                var doctorSelect = _wait.Until(d => d.FindElement(By.Id("DoctorId")));
                var selectDoctor = new SelectElement(doctorSelect);

                selectDoctor.SelectByText(newValue); 
            }

            var saveButton = _wait.Until(d => d.FindElement(By.CssSelector("input[type='submit']")));
            saveButton.Click();
        }



        [Then(@"the appointment details should reflect the new doctor ""(.*)""")]
        public void ThenTheAppointmentDetailsShouldReflectTheNewDoctor(string expectedDoctor)
        {
            var doctorCell = _wait.Until(d =>
                d.FindElement(By.XPath($"//table//td[contains(text(), '{expectedDoctor}')]")));

            Assert.NotNull(doctorCell);
            Assert.Contains(expectedDoctor, doctorCell.Text);
        }
        [When(@"I delete the first appointment with patient name ""(.*)""")]
        public void WhenIDeleteTheFirstAppointmentWithPatientName(string patientName)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Appointments");

            // Find all rows in the appointment table
            var rows = _wait.Until(d => d.FindElements(By.CssSelector("table.table tbody tr")));

            // Find the first row that contains the patient name
            var row = rows.FirstOrDefault(r => r.Text.Contains(patientName));
            Assert.NotNull(row);

            // Open the dropdown menu
            var dropdownToggle = row.FindElement(By.CssSelector(".dropdown-toggle"));
            dropdownToggle.Click();

            // Wait for the delete link to appear and click it
            var deleteLink = _wait.Until(driver =>
                row.FindElements(By.CssSelector("a.dropdown-item"))
                    .FirstOrDefault(a => a.Text.Trim().Contains("Delete"))
            );
            Assert.NotNull(deleteLink);

            deleteLink.Click();
        }

        [When("I confirm the appointment deletion")]
        public void WhenIConfirmTheAppointmentDeletion()
        {
            var confirmButton = _wait.Until(d => d.FindElement(By.CssSelector("form input[type='submit']")));
            confirmButton.Click();
        }
        [Then(@"the appointment with patient ""(.*)"" should not appear in the list")]
        public void ThenTheAppointmentWithPatientShouldNotAppearInTheList(string patientName)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Appointments");

            Thread.Sleep(500); // Small wait to allow page refresh

            var rows = _driver.FindElements(By.CssSelector("table.table tbody tr"));
            bool found = rows.Any(r => r.Text.Contains(patientName));

            Assert.False(found, $"Appointment for patient '{patientName}' was still found after deletion.");
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
