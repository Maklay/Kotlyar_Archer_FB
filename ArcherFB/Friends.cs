using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;


namespace ArcherFB
{
    public class Friends
    {
        IWebDriver driver;
        const string EMAIL = "mail@gmail.com";
        const string PASS = "pass";

        string baseURL = "https://www.facebook.com/100014305758238/friends";
        string friendsUrl = "https://www.facebook.com/100014305758238/friends";

        By emailLocator = By.Id("email");
        By passLocator = By.Id("pass");
        By authLocator = By.Id("u_0_2");
        By stopScrollLocator = By.XPath("/html/body/div[1]/div[3]/div[1]/div/div[2]/div[2]/div[2]/div/div[2]/div/div/div[10]/div[2]/div/a");

        int heignt = 0;
        int position1 = 0;
        int position2 = 1;

        ICollection<IWebElement> ListFriends = new List<IWebElement>();


        [SetUp]
        public void InitDriver()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(baseURL);
        }

        [TearDown]
        public void QuitDriver()
        {
            driver.Quit();
        }

        [Test]
        public void TestMethod2()
        {
            AuthInFB(EMAIL, PASS);
            GoToFriendsPage();
            Rec(position1, position2);
            int amountFriends = ListFriends.Count;
        }
        public void Rec(int position1, int position2)
        {
            position1++;
            if (position1 > 20)
            {
                position1 = 1;
                position2++;
            }
            Dot:
            try
            {
                ListFriends.Add(driver.FindElement(GetCellLocator(position2, position1)));
                Rec(position1, position2);
            }
            catch (Exception)
            {
                if (ScrollsToTheAndFriendsList() == false)
                {
                    goto Dot;
                }
            }
        }
        public By GetCellLocator(int position2, int position1)
        {
            return By.XPath("/html/body/div[1]/div[3]/div[1]/div/div[2]/div[2]/div[2]/div/div[2]/div/div/div/div[2]/div/ul[" + position2 + "]/li[" + position1 + "]");
        }

        private void GoToFriendsPage()
        {
            driver.Navigate().GoToUrl(friendsUrl);
        }

        private void AuthInFB(string email, string secret)
        {
            driver.FindElement(emailLocator).SendKeys(EMAIL);
            driver.FindElement(passLocator).SendKeys(PASS);
            driver.FindElement(authLocator).Click();
        }

        private bool ScrollsToTheAndFriendsList()
        {

            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            heignt = +100;
            try
            {
                driver.FindElement(stopScrollLocator);
                return true;
            }
            catch (Exception)
            {
                js.ExecuteScript("window.scrollTo(" + heignt + ", " + heignt + 100 + ")");
                Thread.Sleep(1000);
                return false;
            }
        }
    }
}
