using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Xunit;
using System;

namespace AmbulanceManagement.UITests.StepDefinitions
{
    [Binding]
    public class ReportsStep : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _baseUrl = "https://localhost:7287";

        public ReportsStep()
        {
            var options = new ChromeOptions();
            options.AddArgument("start-maximized");
            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Given(@"I navigate to the Reports Index page")]
        public void GivenINavigateToReportsIndexPage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Reports");
        }

        [Then(@"I should see a table of reports")]
        public void ThenIShouldSeeTable()
        {
            var table = _driver.FindElement(By.CssSelector("table.table"));
            Assert.True(table.Displayed);
        }

        [When(@"I click on the visit date of the first report")]
        public void WhenIClickFirstReport()
        {
            var link = _driver.FindElement(By.CssSelector("tbody tr td a"));
            link.Click();
        }

        [Then(@"I should see the report detail page")]
        public void ThenIShouldSeeDetails()
        {
            Assert.Contains("/Reports/Details", _driver.Url);
        }

        [Then(@"I should see the patient name, doctor name, and diagnosis")]
        public void ThenIShouldSeePatientAndDoctor()
        {
            Assert.Contains("Patient Name", _driver.PageSource);
            Assert.Contains("Doctor", _driver.PageSource);
            Assert.Contains("Diagnosis", _driver.PageSource);
        }

        [Given(@"I navigate to the Create Report page")]
        public void GivenINavigateToCreateReport()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Reports/Create");
        }

        [When(@"I select an appointment and a doctor")]
        public void WhenISelectAppointmentAndDoctor()
        {
            new SelectElement(_driver.FindElement(By.Id("AppointmentId"))).SelectByIndex(1);
            new SelectElement(_driver.FindElement(By.Id("DoctorId"))).SelectByIndex(1);
        }

        [When(@"I enter visit date ""(.*)""")]
        public void WhenIEnterVisitDate(string date)
        {
            var input = _driver.FindElement(By.Id("VisitDate"));
            input.Clear();

            // Split the input on known markers and send keys accordingly
            string[] parts = date.Split(new[] { "[TAB]", "[RIGHT]" }, StringSplitOptions.None);

            for (int i = 0; i < parts.Length; i++)
            {
                input.SendKeys(parts[i]);

                // Send the special key between segments
                if (i < parts.Length - 1)
                {
                    if (date.Contains("[TAB]"))
                        input.SendKeys(Keys.Tab);
                    else if (date.Contains("[RIGHT]"))
                        input.SendKeys(Keys.ArrowRight);
                }
            }
        }


        [When(@"I enter symptoms ""(.*)""")]
        public void WhenIEnterSymptoms(string symptoms)
        {
            _driver.FindElement(By.Id("Symptoms")).SendKeys(symptoms);
        }

        [When(@"I enter diagnosis ""(.*)""")]
        public void WhenIEnterDiagnosis(string diagnosis)
        {
            _driver.FindElement(By.Id("Diagnosis")).SendKeys(diagnosis);
        }

        [When(@"I enter prescriptions ""(.*)""")]
        public void WhenIEnterPrescriptions(string prescriptions)
        {
            _driver.FindElement(By.Id("Prescriptions")).SendKeys(prescriptions);
        }

        [When(@"I submit the create form")]
        public void WhenISubmitCreateForm()
        {
            _driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        }

        [Then(@"I should be redirected to the Reports Index")]
        public void ThenIShouldBeRedirectedToIndex()
        {
            Assert.Contains("/Reports", _driver.Url);
        }

        [When(@"I click the Edit button for the first report")]
        public void WhenIClickEdit()
        {
            _driver.FindElement(By.CssSelector("a.action-icon.dropdown-toggle")).Click();
            var editBtn = _driver.FindElement(By.CssSelector("a[href*='/Reports/Edit']"));
            editBtn.Click();
        }

        [When(@"I change the diagnosis to ""(.*)""")]
        public void WhenIChangeDiagnosis(string newDiagnosis)
        {
            var diagnosis = _driver.FindElement(By.Id("Diagnosis"));
            diagnosis.Clear();
            diagnosis.SendKeys(newDiagnosis);
        }

        [When(@"I submit the edit form")]
        public void WhenISubmitEditForm()
        {
            _driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        }

        [When(@"I click the Delete button for the first report")]
        public void WhenIClickDelete()
        {
            _driver.FindElement(By.CssSelector("a.action-icon.dropdown-toggle")).Click();
            var deleteBtn = _driver.FindElement(By.CssSelector("a[href*='/Reports/Delete']"));
            deleteBtn.Click();
        }

        [When(@"I confirm the deletion")]
        public void WhenIConfirmDelete()
        {
            _driver.FindElement(By.CssSelector("input[type='submit'][value='Delete']")).Click();
        }

        [When(@"I click the Print button for the first report")]
        public void WhenIClickPrint()
        {
            _driver.FindElement(By.CssSelector("a[href*='/Reports/Print/']")).Click();
        }

        [Then(@"a new tab should open with the printable report")]
        public void ThenNewTabShouldOpen()
        {
            var tabs = _driver.WindowHandles;
            Assert.True(tabs.Count > 1);
            _driver.SwitchTo().Window(tabs[1]);
            Assert.Contains("/Reports/Print", _driver.Url);
        }
        [Given(@"I am logged in as a user with the Doctor role")]
        public void GivenIAmLoggedInAsAUserWithTheRole()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Identity/Account/Login");

            _wait.Until(d => d.FindElement(By.Id("Email"))).SendKeys("Adem@gmail.com");
            _driver.FindElement(By.Id("Password")).SendKeys("Adem123@");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            _wait.Until(d => d.Url.Contains("Home")); 
        }
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
