using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
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
            Assert.Contains("/Home", _driver.Url);
        }
        [Given(@"I am on the register page")]
        public void GivenIAmOnTheRegisterPage()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Account/Register");
        }

        [When("I fill in the registration form with test data")]
        public void WhenIFillRegistrationWithTestData()
        {
            string testEmail = $"testuser_{DateTime.Now.Ticks}@example.com";
            string testPassword = "Test123@";

            _driver.FindElement(By.Id("Name")).SendKeys("Test User");
            _driver.FindElement(By.Id("Email")).SendKeys(testEmail);
            _driver.FindElement(By.Id("Password")).SendKeys(testPassword);
            _driver.FindElement(By.Id("ConfirmPassword")).SendKeys(testPassword);
            _driver.FindElement(By.Id("Number")).SendKeys("123456");
            _driver.FindElement(By.Id("DateOfBirth")).SendKeys("01/01/2000");
            new OpenQA.Selenium.Support.UI.SelectElement(_driver.FindElement(By.Id("Gender"))).SelectByText("Male");
            _driver.FindElement(By.Id("Education")).SendKeys("Test Degree");
            _driver.FindElement(By.Id("Type")).SendKeys("Doctor");
            _driver.FindElement(By.Id("Biography")).SendKeys("Bio...");
            new OpenQA.Selenium.Support.UI.SelectElement(_driver.FindElement(By.Id("RoleName"))).SelectByText("Doctor");
        }

        [When("I upload a profile picture")]
        public void WhenIUploadProfilePicture()
        {
            var fileInput = _driver.FindElement(By.Id("ProfilePicture"));

            var path = Path.Combine(Directory.GetCurrentDirectory(), "test_image.png");

            if (!File.Exists(path))
            {
                File.WriteAllBytes(path, new byte[10]);
            }

            fileInput.SendKeys(path);
        }
        [When("I submit the registration form")]
        public void WhenISubmitTheRegistrationForm()
        {
            _driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        }

        [Then("I should be redirected to the user list")]
        public void ThenIShouldBeRedirectedToTheUserList()
        {
            Assert.Contains("/Account/ListAll", _driver.Url);
        }

        [Given("I am logged in as admin")]
        public void GivenIAmLoggedInAsAdmin()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Account/Login");

            _driver.FindElement(By.Id("Email")).SendKeys("admin@gmail.com");
            _driver.FindElement(By.Id("Password")).SendKeys("Admin123@");
            _driver.FindElement(By.CssSelector("button.account-btn")).Click();
            
        }

        [Given("I am on the user list page")]
        public void GivenIAmOnTheUserListPage()
        {
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl($"{_baseUrl}/Account/ListAll");
        }

        [When("I click the Delete button for the test user")]
        public void WhenIClickDeleteButtonForTestUser()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Account/ListAll");

            var row = _driver.FindElements(By.CssSelector("tbody tr"))
                             .FirstOrDefault(r => r.Text.Contains("Test User"));

            Assert.NotNull(row); 

            var dropdownToggle = row.FindElement(By.CssSelector(".action-icon.dropdown-toggle"));
            dropdownToggle.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            wait.Until(driver => row.FindElement(By.LinkText("Delete")).Displayed);


            var deleteBtn = row.FindElement(By.LinkText("Delete"));
            deleteBtn.Click();
        }

        [When("I confirm the deletion as admin")]
        public void WhenIConfirmTheDeletion()
        {
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }

        [Then("I should not see the test user in the list")]
        public void ThenIShouldNotSeeTheTestUserInTheList()
        {
            var rows = _driver.FindElements(By.CssSelector("tbody tr"));
            bool found = rows.Any(r => r.Text.Contains("Test User"));
            Assert.False(found, "Test user was found in the list after deletion.");
        }


        public void Dispose()
        {
            _driver.Quit();
        }
    }
}
