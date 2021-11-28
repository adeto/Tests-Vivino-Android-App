using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;

namespace Appium_Tests_Of_Vivino_Android_App
{
    public class Vivino_Automation_Tests
    {
        private const string AppiumServerUrl = "http://[::1]:4723/wd/hub";
        private AndroidDriver<AndroidElement> driver;
        private const string VivinoAppPath = @"C:\Adi\Automation docs\Appium\vivino.web.app_8.18.11-8181199.apk";
        private const string VivinoAppPackage = "vivino.web.app";
        private const string VivinoAppStartActivity = "com.sphinx_solution.activities.SplashActivity";
        private const string VivinoTestAccountEmail = "aditest@gmail.com";
        private const string VivioTestAccoundPassword = "VivinoTest2021";

        [OneTimeSetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions() { PlatformName = "Android" };
            appiumOptions.AddAdditionalCapability("app", VivinoAppPath);
            appiumOptions.AddAdditionalCapability("appPackage", VivinoAppPackage);
            appiumOptions.AddAdditionalCapability("appActivity", VivinoAppStartActivity);
            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumServerUrl), appiumOptions);

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        [Test]
        public void Appium_Test_Vivino_App()
        {
            // Login in VivinoApp with existing account
            var linkExistingAccount = driver.FindElementById("vivino.web.app:id/txthaveaccount");
            linkExistingAccount.Click();

            var textboxEmail = driver.FindElementById("vivino.web.app:id/edtEmail");
            textboxEmail.SendKeys(VivinoTestAccountEmail);

            var textboxPassword = driver.FindElementById("vivino.web.app:id/edtPassword");
            textboxPassword.SendKeys(VivioTestAccoundPassword);

            var buttonSignIn = driver.FindElementById("vivino.web.app:id/action_signin");
            buttonSignIn.Click();

            // Search for word "Katarzhyna Reserve Red 2006"
            var exploreTab = driver.FindElementById("vivino.web.app:id/wine_explorer_tab");
            exploreTab.Click();

            var searchVivinoBox = driver.FindElementById("vivino.web.app:id/search_vivino");
            searchVivinoBox.Click();

            var searchTextInput = driver.FindElementById("vivino.web.app:id/editText_input");
            searchTextInput.SendKeys("Katarzhyna Reserve Red 2006");

            //Search and open the first result, and assert that it holds correct data
            var listSearchResults = driver.FindElementById("vivino.web.app:id/listviewWineListActivity");
            var firstResult = listSearchResults.FindElementByClassName("android.widget.FrameLayout");
            firstResult.Click();

             var elementRating = driver.FindElementById("vivino.web.app:id/rating");
            string ratingText = elementRating.Text;
            double rating = double.Parse(ratingText);
            Assert.IsTrue(rating >= 3.00 && rating <= 5.00);


            var tabsSummary = driver.FindElementById("vivino.web.app:id/tabs");
            var tabHighlights = tabsSummary.FindElementByXPath("//android.widget.TextView[1]");
            var tabFacts = tabsSummary.FindElementByXPath("//android.widget.TextView[2]");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

            tabHighlights.Click();
            var highlightsDescription = driver.FindElementByAndroidUIAutomator(
                "new UiScrollable(new UiSelector().scrollable(true))" +
                ".scrollIntoView(new UiSelector().resourceIdMatches(" +
                "\"vivino.web.app:id/highlight_description\"))");
            Assert.AreEqual("Among top 1% of all wines in the world", highlightsDescription.Text);

            tabFacts.Click();
            var factTitle = driver.FindElementById("vivino.web.app:id/wine_fact_title");
            Assert.AreEqual("Grapes", factTitle.Text);
            var factText = driver.FindElementById("vivino.web.app:id/wine_fact_text");
            Assert.AreEqual("Cabernet Sauvignon,Merlot", factText.Text);
        }

        [OneTimeTearDown]
        public void ShutDown()
        {
            driver.Quit();
        }
    }
}