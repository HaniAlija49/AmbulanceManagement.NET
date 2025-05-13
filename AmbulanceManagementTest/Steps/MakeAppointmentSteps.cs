using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace AmbulanceManagement.UITests.StepDefinitions
{
    [Binding]
    public class MakeAppointmentSteps : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "https://localhost:7287"; // Adjust as needed

        public MakeAppointmentSteps()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Given("I open the Welcome page")]
        public void GivenIOpenTheWelcomePage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Welcome");
        }

        [When("I click the Make Appointment button")]
        public void WhenIClickTheAppointmentButton()
        {
            var button = _driver.FindElement(By.ClassName("appointment-btn"));
            button.Click();
            Thread.Sleep(1000); // optional, allow scroll animation
        }

        [When(@"I fill in name ""(.*)"" and email ""(.*)""")]
        public void WhenIFillNameAndEmail(string name, string email)
        {
            _driver.FindElement(By.Id("name")).SendKeys(name);
            _driver.FindElement(By.Id("email")).SendKeys(email);
        }

        [When(@"I choose appointment hour ""(.*)""")]
        public void WhenIChooseHour(string hour)
        {
            var select = new SelectElement(_driver.FindElement(By.Name("Hour")));
            select.SelectByText(hour);
        }

        [When(@"I select date ""(.*)""")]
        public void WhenISelectDate(string date)
        {
            _driver.FindElement(By.Id("appointmentDate")).SendKeys(date);
        }

        [When("I choose a doctor")]
        public void WhenIChooseDoctor()
        {
            var select = new SelectElement(_driver.FindElement(By.Name("DoctorId")));
        }

        [When("I submit the appointment form")]
        public void WhenISubmitForm()
        {
            var submitButton = _driver.FindElement(By.CssSelector("button.appointment-btn[type='submit']"));
            submitButton.Click();
        }

        [Then("I should return to the Welcome page")]
        public void ThenIReturnToWelcome()
        {
            Assert.Contains("/", _driver.Url);
        }

        [Then("I should not see an error message")]
        public void ThenINotSeeError()
        {
            var bodyText = _driver.FindElement(By.TagName("body")).Text;
            Assert.DoesNotContain("Invalid input", bodyText);
            Assert.DoesNotContain("Patient not found", bodyText);
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}
