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


        public void Run()
        {
            

            Console.WriteLine(driver.Title);
            Console.WriteLine();
            //List<IWebElement> links = FindElements();

            //Level1(links);
            //for (int i = 0; i < zaglebienie; i++)
            //{

            //    //PrepareForNext(i);
            //}

            VisitNode();




            Console.ReadKey();
        }


        private void VisitNode()
        {
            var nodes = FindElements();

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes = FindElements();
                Print(nodes);
                nodes[i].Click();
                Console.WriteLine(driver.Title);

                VisitNode2();
            }


        }

        private void VisitNode2()
        {
            Console.WriteLine(driver.Title);
        }


        private void Print(List<IWebElement> node)
        {
            foreach (var item in node)
            {
                Console.WriteLine(item.Text);
            }
        }






        private List<IWebElement> Level1(List<IWebElement> links)
        {
            var linksToReturn = new List<IWebElement>();

            for (int k = 0; k < links.Count; k++)
            {
                links = FindElements();
                links[k].Click();
                Console.WriteLine(driver.Title);
                driver.Navigate().Back();
            }

            return linksToReturn;
        }

        private void PrepareForNext(int linkNumber)
        {
            var links = FindElements();
            links[linkNumber].Click();
            links = FindElements();
            Level1(links);
        }









        private List<IWebElement> FindElements()
        {
            return driver.FindElements(By.TagName("a")).Where(x => string.IsNullOrEmpty(x.Text) == false).ToList();
        }


    }
}
