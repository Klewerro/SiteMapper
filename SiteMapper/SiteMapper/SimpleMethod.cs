using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMapper
{
    class SimpleMethod
    {

        private IWebDriver driver;
        string url;
        int zaglebienie;
        private List<string> visitedNodes = new List<string>();

        public SimpleMethod(IWebDriver driver, string address, int zaglebienie) 
        {
            this.driver = driver;
            url = address;
            this.zaglebienie = zaglebienie;
            driver.Navigate().GoToUrl(url);
        }


        public void Run(int nOfNodes)
        {

            





            //Console.ReadKey();
        }


        private void VisitNode(int number)
        {
            var elements = FindElements();
            elements[number].Click();
            Console.WriteLine(driver.Title);
            driver.Navigate().Back();

        }






        private void Print(List<IWebElement> node)
        {
            foreach (var item in node)
            {
                Console.WriteLine(item.Text);
            }
        }


        private List<IWebElement> FindElements()
        {
            return driver.FindElements(By.TagName("a")).Where(x => string.IsNullOrEmpty(x.Text) == false).ToList();
        }


    }
}
