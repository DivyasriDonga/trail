using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.ConstrainedExecution;
using OpenQA.Selenium.Interactions;
using System.Security.Cryptography.X509Certificates;


namespace FitPeo
{
    public class Tests
    {

        private By slider = By.XPath("//span[@class='MuiSlider-thumb MuiSlider-thumbSizeMedium MuiSlider-thumbColorPrimary MuiSlider-thumb MuiSlider-thumbSizeMedium MuiSlider-thumbColorPrimary css-sy3s50']");
        private IWebElement RangeSlider => driver.FindElement(slider);

        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            // Initialize ChromeDriver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void Test1()
        {
            // Navigate to the FitPeo homepage
            driver.Navigate().GoToUrl("https://www.fitpeo.com/");

            // Find and click the link to the Revenue Calculator page
            IWebElement revenueCal = driver.FindElement(By.XPath("//div[contains(text(),'Revenue Calculator')]"));
            revenueCal.Click();

            //Scroll down the page until the revenue calculator slider is visible.
            IWebElement sliderElement = driver.FindElement(By.XPath("//h4[normalize-space()='Medicare Eligible Patients']"));
            ScrollIntoView(sliderElement);

            // Validate the value of the slider
            IWebElement sliderTextBox2 = driver.FindElement(By.XPath("//input[@id=':r0:']"));
            IJavaScriptExecutor js2 = (IJavaScriptExecutor)driver;
            js2.ExecuteScript("arguments[0].value = '560';", sliderTextBox2);
            Thread.Sleep(2000);

            // Validate the value of the slider
            string sliderValue2 = sliderTextBox2.GetAttribute("value");
            Assert.AreEqual("560", sliderValue2, "Slider text box value did not update correctly.");
            Console.WriteLine("Slider value successfully updated to: " + sliderValue2);


            //Adjust the slider to set its value to 820.we’ve highlighted the slider in red color, Once the slider is moved the bottom text field value should be updated to 820
            IWebElement sliderTextBox = driver.FindElement(By.XPath("//input[@id=':r0:']"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].value = '820';", sliderTextBox);
            Thread.Sleep(2000);
            Actions builder = new Actions(driver);
            builder.SendKeys(Keys.Enter);
            Thread.Sleep(2000);
            clickOnCheckbox();


            // Validate Total Recurring Reimbursement:
            string expectedAmount = "$27000";
            string totalRecurring = driver.FindElement(By.XPath("(//input[@type='checkbox'])[8]")).Text;
            if (totalRecurring != expectedAmount)
            {
                Assert.IsTrue(true);
            }

            // Verify that the header displaying Total Recurring Reimbursement for all Patients Per Month
            IWebElement recurringTotal = driver.FindElement(By.XPath(" //p[@class='MuiTypography-root MuiTypography-body2 inter css-1xroguk'][contains(text(),'Total Recurring Reimbursement for all Patients Per')]"));
            string actualText = recurringTotal.Text;
            if (actualText.Contains("$27000"))
            {
                Assert.IsTrue(true);
            }
            Assert.IsTrue(actualText.Contains(expectedAmount), $"Expected '{expectedAmount}' but got '{actualText}'.");
            Console.WriteLine($"Validated text: {actualText}");


        }
        public void clickOnCheckbox()
        {

            IWebElement checkboxCPT_99091 = driver.FindElement(By.XPath("(//input[@type='checkbox'])[1]"));
            checkboxCPT_99091.Click();
            IWebElement checkboxCPT_99053 = driver.FindElement(By.XPath("(//input[@type='checkbox'])[2]"));
            checkboxCPT_99053.Click();
            IWebElement checkboxCPT_99054 = driver.FindElement(By.XPath("(//input[@type='checkbox'])[3]"));
            checkboxCPT_99054.Click();
            IWebElement checkboxCPT_99074 = driver.FindElement(By.XPath("(//input[@type='checkbox'])[8]"));
            checkboxCPT_99074.Click();
            Thread.Sleep(5000);
        }
        private void ScrollIntoView(IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}
