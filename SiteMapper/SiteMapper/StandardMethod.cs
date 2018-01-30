using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SiteMapper
{
    class StandardMethod
    {
        public void Run()
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://trello.com/");


            List<IWebElement> links = FindElements(driver);
            var siteDictionary = new Dictionary<IWebElement, List<IWebElement>>();
            foreach (var item in links)
            {
                siteDictionary.Add(item, new List<IWebElement>());
            }

            PrintElements(links, driver);
            Console.WriteLine("_____________________");

            var listLevel2 = DriveIWebElementsFromList(links, driver);
            Console.WriteLine("=====================");
            foreach (var item in listLevel2)
            {
                var listLevel3 = DriveIWebElementsFromList(item, driver);
                Thread.Sleep(5000);
            }
        }

        public static List<IWebElement> FindElements(IWebDriver driver)
        {

            return driver.FindElements(By.TagName("a")).Where(x => string.IsNullOrEmpty(x.Text) == false).ToList();
        }


        public static List<List<IWebElement>> DriveIWebElementsFromList(List<IWebElement> links, IWebDriver driver)
        {
            var elementsRepository = new List<List<IWebElement>>();
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
                    elementsRepository.Add(links2); //lista list webElementów
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
                catch (InvalidOperationException ex2)
                {
                    Console.WriteLine("!Pominieto!");
                    Console.WriteLine();
                    Console.ReadKey();
                    continue;
                }

            }
            iterator = 0;
            return elementsRepository;
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

        private static void JoinToDictionaryNode(Dictionary<IWebElement, List<IWebElement>> dictionary, IWebElement webElement, List<IWebElement> listOfElements)
        {
            dictionary[webElement] = listOfElements;
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
