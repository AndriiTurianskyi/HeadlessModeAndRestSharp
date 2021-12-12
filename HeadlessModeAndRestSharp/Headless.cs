using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessModeAndRestSharp
{
    [TestFixture]
    public class Headless
    {
        IWebDriver driver;

        [SetUp]
        public void beforeTest()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            driver = new ChromeDriver(options);
            driver.Url = "https://www.olx.ua/";
        }

        [TearDown]
        public void shutDown()
        {
            driver.Quit();
        }

        [Test]
        public void test()
        {
            IWebElement cityField = driver.FindElement(By.XPath("//*[@id='cityField']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", cityField);
            cityField.Click();
            IWebElement regions = driver.FindElement(By.Id("regions-layer"));
            Assert.IsTrue(regions.Displayed);
        }
    }
}
