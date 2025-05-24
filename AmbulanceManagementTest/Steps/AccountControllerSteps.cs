using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using Xunit;

namespace AmbulanceManagement.UITests.StepDefinitions
{
    [Binding]
    public class AccountControllerSteps : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "https://localhost:7287"; // Update this if your port is different

        public AccountControllerSteps()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [Given("I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Account/Login");
        }

        [When(@"I enter email ""(.*)"" and password ""(.*)""")]
        public void WhenIEnterEmailAndPassword(string email, string password)
        {
            _driver.FindElement(By.Id("Email")).SendKeys(email);
            _driver.FindElement(By.Id("Password")).SendKeys(password);
        }

        [When("I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            _driver.FindElement(By.CssSelector("button.account-btn[type='submit']")).Click();
        }

        [Then("I should be redirected to the dashboard")]
        public void ThenIShouldBeRedirectedToDashboard()
        {
            Assert.Contains("/Home", _driver.Url); // Adjust if landing page is different
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}
