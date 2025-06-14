using AmbulanceManagement.Controllers;
using AmbulanceManagement.Data;
using AmbulanceManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace AmbulanceManagement.Tests.StepDefinitions
{
    [Binding]
    public class PatientsControllerStepDefinitions : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _baseUrl = "https://localhost:7287"; 

        public PatientsControllerStepDefinitions()
        {
            var options = new ChromeOptions();
            options.AddArgument("start-maximized");
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Given(@"I am logged in as a user with the ""(.*)"" role")]
        public void GivenIAmLoggedInAsAUserWithTheRole(string role)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Identity/Account/Login");

            _wait.Until(d => d.FindElement(By.Id("Email"))).SendKeys("laura@gmail.com");
            _driver.FindElement(By.Id("Password")).SendKeys("Laura123@");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            _wait.Until(d => d.Url.Contains("Home")); 
        }
        [When(@"I navigate to the ""(.*)"" page")]
        public void WhenINavigateToThePage(string page)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/{page}");
            Thread.Sleep(500);
        }

        [Then(@"I should see a list of all patients")]
        public void ThenIShouldSeeAListOfAllPatients()
        {
            var patientRows = _wait.Until(d =>
                d.FindElements(By.CssSelector("table.table tbody tr"))
            );

            Assert.True(patientRows.Count > 0, "No patient rows found in the patient list table.");
        }



        [When(@"I search for a patient with the name ""(.*)""")]
        public void WhenISearchForAPatientWithTheName(string name)
        {
            var searchInput = _wait.Until(d => d.FindElement(By.CssSelector("input[name='searchQuery']")));
            searchInput.Clear();
            searchInput.SendKeys(name);

            var searchButton = _wait.Until(d => d.FindElement(By.CssSelector(".input-group-append button[type='submit']")));
            searchButton.Click();
        }

        [Then(@"I should see a list of patients containing ""(.*)""")]
        public void ThenIShouldSeeAListOfPatientsContaining(string name)
        {
            var tableBody = _wait.Until(d => d.FindElement(By.TagName("body")));
            Assert.Contains(name, tableBody.Text);
        }


        [When(@"I submit a new patient with the following details:")]
        public void WhenISubmitANewPatientWithTheFollowingDetails(Table table)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Patients/Create");

            var row = table.Rows.First();  

            // Fill in the patient form
            _wait.Until(d => d.FindElement(By.Id("Name"))).SendKeys(row["Name"]);
            _driver.FindElement(By.Id("LastName")).SendKeys(row["LastName"]);
            _driver.FindElement(By.Id("EmailAddress")).SendKeys(row["EmailAddress"]);
            _driver.FindElement(By.Id("PhoneNumber")).SendKeys(row["PhoneNumber"]);
            _driver.FindElement(By.Id("Age")).SendKeys(row["Age"]);
            _driver.FindElement(By.Id("Adress")).SendKeys(row["Adress"]);

            // Wait for the submit button to be clickable
            var submitButton = _wait.Until(d => d.FindElement(By.CssSelector("input.btn-primary")));
            _wait.Until(d => submitButton.Displayed && submitButton.Enabled);  
            submitButton.Click();
        }



        [Then(@"the patient ""(.*)"" should be visible in the patient list")]
        public void ThenThePatientShouldBeVisibleInThePatientList(string fullName)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Patients");

            _wait.Until(d => d.PageSource.Contains(fullName));

            Assert.Contains(fullName, _driver.PageSource);
        }


        [Given(@"a patient named ""(.*)"" exists")]
        public void GivenAPatientNamedExists(string fullName)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Patients");
            var patientLink = _wait.Until(d => d.FindElement(By.XPath($"//a[contains(text(),'{fullName}')]")));
            Assert.NotNull(patientLink);
        }

        [When(@"I view the details of patient ""(.*)""")]
        public void WhenIViewTheDetailsOfPatient(string fullName)
        {
            var link = _wait.Until(d => d.FindElement(By.XPath($"//a[contains(text(),'{fullName}')]")));
            link.Click();
        }

        [Then(@"I should see the reports linked to their appointments")]
        public void ThenIShouldSeeTheReportsLinkedToTheirAppointments()
        {
            var reportsLink = _wait.Until(d =>
                d.FindElements(By.XPath("//a[contains(text(),'Report')]")).Count > 0
            );
            Assert.True(reportsLink, "Reports link not found for the patient.");
        }


        [When(@"I update the age of ""(.*)"" to ""(.*)""")]
        public void WhenIUpdateTheAgeOfTo(string fullName, string newAge)
        {
            // Step 1: Navigate to the Patients page
            _driver.Navigate().GoToUrl($"{_baseUrl}/Patients");

            // Step 2: Click the patient's name to go to the details page
            var patientLink = _wait.Until(d =>
                d.FindElement(By.XPath($"//a[text()='{fullName}']"))
            );
            patientLink.Click();

            // Step 3: Click the "Edit Patient" button on the details page
            var editButton = _wait.Until(d =>
                d.FindElement(By.XPath("//a[contains(text(),'Edit Patient')]"))
            );
            editButton.Click();

            // Step 4: Update the age
            var ageInput = _wait.Until(d => d.FindElement(By.Id("Age")));
            ageInput.Clear();
            ageInput.SendKeys(newAge);
            ageInput.SendKeys(Keys.Tab);

            // Step 5: Submit the form
            var submitButton = _wait.Until(d =>
                d.FindElement(By.CssSelector("input[type='submit'][value='Save']"))
            );
            submitButton.Click();
        }

        [Then(@"the patient details should reflect the new age")]
        public void ThenThePatientDetailsShouldReflectTheNewAge()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Patients");

            var patientLink = _wait.Until(d =>
                d.FindElement(By.XPath("//a[text()='Azem Jovani']"))
            );
            patientLink.Click();

            var ageField = _wait.Until(d =>
                d.FindElement(By.XPath("//li[span[contains(text(),'Age:')]]/span[@class='text']"))
            );

            Assert.Equal("40", ageField.Text);
        }

        [When(@"I delete the patient with name ""(.*)""")]
        public void WhenIDeleteTheFirstPatientWithName(string fullName)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Patients");

            // Find all patient rows
            var rows = _wait.Until(d => d.FindElements(By.CssSelector("tbody tr")));

            // Find the first matching row containing the patient's name
            var row = rows.FirstOrDefault(r => r.FindElement(By.CssSelector("td a")).Text.Trim() == fullName);
            Assert.NotNull(row);

            // Open the dropdown menu
            var dropdownToggle = row.FindElement(By.CssSelector(".dropdown-toggle"));
            dropdownToggle.Click();

            // Wait until the delete option appears in the dropdown
            var deleteLink = _wait.Until(d => row.FindElement(By.XPath(".//a[contains(@href, '/Patients/Delete')]")));
            deleteLink.Click();
        }
        [When(@"I confirm the patient deletion")]
        public void WhenIConfirmThePatientDeletion()
        {
            var submitButton = _wait.Until(driver => driver.FindElement(By.CssSelector("form input[type='submit']")));
            submitButton.Click();
        }
        [Then(@"the patient ""(.*)"" should not appear in the list")]
        public void ThenThePatientShouldNotAppearInTheList(string fullName)
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Patients");

            Thread.Sleep(500); // small wait to ensure reload

            var rows = _driver.FindElements(By.CssSelector("tbody tr"));
            bool patientExists = rows.Any(r => r.Text.Contains(fullName));

            Assert.False(patientExists, $"Patient '{fullName}' was found in the list after deletion.");
        }


        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
