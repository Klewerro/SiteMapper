using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SiteMapper
{
    class ObjectiveMethod
    {
        private IWebDriver driver;
        private string siteUrl;


        public ObjectiveMethod(IWebDriver driver, string url)
        {
            this.driver = driver;
            siteUrl = url;
        }

        public void Run()
        {
            SiteNode rootNode;
            List<SiteNode> nodes;
            List<SiteNode> nodes2;

            OpenUrl(siteUrl);
            rootNode = CreateRootNode();
            Print(rootNode);
            nodes = new List<SiteNode>(FindElementsFromSiteNode(rootNode));

            //nodes2 = new List<SiteNode>(FindElementsFromSiteNode(nodes[5]));

            nodes2 = new List<SiteNode>();
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes2 = FindElementsFromSiteNode(i);
            }
            

            

            Console.ReadKey();
        }

        private void OpenUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        private List<IWebElement> FindElements()
        {
            return driver.FindElements(By.TagName("a")).Where(x => string.IsNullOrEmpty(x.Text) == false).ToList();
        }


        private SiteNode CreateRootNode()
        {
            var rootNode = new SiteNode(driver.Title, FindElements());
            return rootNode;
        }

        private List<SiteNode> FindElementsFromSiteNode(SiteNode rootNode)
        {
            var elements = rootNode.Links;
            var elementsToReturn = new List<SiteNode>();


            for (int i = 0; i < elements.Count; i++)
            {
                try
                {
                    elements = FindElements();
                    elements[i].Click();
                    Thread.Sleep(1000);
                    var newNode = new SiteNode(driver.Title, FindElements());
                    elementsToReturn.Add(newNode);
                    Print(newNode);
                    driver.Navigate().Back();
                }
                catch(ArgumentOutOfRangeException ex)
                {
                    NewTabHandle();
                    elements = FindElements();
                    continue;
                }
                catch(InvalidOperationException ex2)    //Element generatet by JS or being before element
                {
                    Console.WriteLine();
                    Console.WriteLine("Pominięto");
                    Console.ReadKey();
                    continue;
                }
                
            }
            return elementsToReturn;
        }

        private List<SiteNode> FindElementsFromSiteNode(int nodeNumber)
        {
            var elementsPrevNode = FindElements();
            //var elements = nodeAbove.GetAllLinks();
            var elementsToReturn = new List<SiteNode>();


            elementsPrevNode[nodeNumber].Click();

            for (int i = 0; i < elementsPrevNode.Count; i++)
            {
                try
                {
                    elementsPrevNode = FindElements();
                    elementsPrevNode[i].Click();
                    Thread.Sleep(1000);
                    var newNode = new SiteNode(driver.Title, FindElements());
                    elementsToReturn.Add(newNode);
                    Print(newNode);
                    driver.Navigate().Back();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    NewTabHandle();
                    elementsPrevNode = FindElements();
                    continue;
                }
                catch (InvalidOperationException ex2)    //Element generatet by JS or being before element
                {
                    //scroll 250 UP
                    //IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                    //jse.ExecuteScript("scroll(0, -50);");
                    //Thread.Sleep(500);
                    Console.WriteLine();
                    Console.WriteLine("Pominięto");
                    Console.ReadKey();
                    continue;
                }
                catch(StaleElementReferenceException ex3)   //Give the browser some time 4 refresh elements
                {
                    Thread.Sleep(1000);
                    continue;
                }

            }
            return elementsToReturn;
        }


        private void NewTabHandle()
        {
            var handles = driver.WindowHandles.ToArray();
            driver.SwitchTo().Window(handles[1]);
            driver.Close();
            driver.SwitchTo().Window(handles[0]);
            Thread.Sleep(500);  //Browser time 4 close tab
            driver.Navigate().Forward();
        }

        public void Print(SiteNode node)
        {
            Console.WriteLine("!Tytuł obecnej strony: " + node.Name);
            foreach (var element in node.Links)
            {
                Console.WriteLine(element.Text);
            }
            Console.WriteLine();
        }

        private void RemoveSameNodeNames(SiteNode siteNode)
        {
            var list = new List<IWebElement>(siteNode.Links);
            list.Distinct().ToList();
        }




    }
}
