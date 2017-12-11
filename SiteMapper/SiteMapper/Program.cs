using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SeleniumProjInz2_v2
{
    class Program
    {
        static void Main(string[] args)
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://trello.com/");


            List<IWebElement> links = FindElements(driver);
            

            PrintElements(links, driver);
            Console.WriteLine("_____________________");

            DriveIWebElementsFromList(links, driver);



        }

        public static List<IWebElement> FindElements(IWebDriver driver)
        {
            return driver.FindElements(By.TagName("a")).Where(x => string.IsNullOrEmpty(x.Text) == false).ToList();
        }

        


        public static void DriveIWebElementsFromList(List<IWebElement> links, IWebDriver driver)
        {
            int iterator = 0;
            for (int i = 0; i < links.Count; i++)
            {
                try
                {
                    string current_window = driver.CurrentWindowHandle;
                    links = FindElements(driver);
                    links[i].Click();
                    Thread.Sleep(1000);
                    var links2 = FindElements(driver);
                    PrintElements(links2, driver);
                    driver.Navigate().Back();
                    iterator++;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    NewTabHandle(driver);
                    links = FindElements(driver);
                    continue;
                }

            }
            iterator = 0;
        }

        public static void PrintElements(List<IWebElement> elements, IWebDriver driver)
        {
            Console.WriteLine("!Tytuł obecnej strony: " + driver.Title);
            foreach (var item in elements)
            {
                Console.WriteLine(item.Text);
            }
            Console.WriteLine("");
        }

        public static void NewTabHandle(IWebDriver driver)
        {
            var handles = driver.WindowHandles.ToArray();
            driver.SwitchTo().Window(handles[1]);
            driver.Close();
            driver.SwitchTo().Window(handles[0]);
            driver.Navigate().Forward();
        }


    }
}
