using OpenQA.Selenium;
using SiteMapper.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SiteMapper.Output;
using System.Windows.Forms;

namespace SiteMapper
{
    public class ObjectiveMethod
    {
        private IWebDriver driver;
        private string siteUrl;
        private string savingDataPath = Paths.savingDataPath;
        public string siteTitle;
        ScreenshotSaving screenshotSaving;
        public List<SiteNode> listOfNodes;

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

            //ConsoleOutputToTxt(screenshootsPath);

            OpenUrl(siteUrl);
            screenshotSaving = new ScreenshotSaving(savingDataPath, siteTitle);

            rootNode = CreateRootNode();
            ConsoleOutput.Print(rootNode);
            ConsoleOutput.PrintAlsoToTxtFile(rootNode, savingDataPath);
            screenshotSaving.SaveScreenshotAsJpg(rootNode);
            nodes = new List<SiteNode>(FindElementsFromSiteNode(rootNode));



            //obudować w jakąś metodę!! do iteracyjnego wywołania
            //moze podac jakas kolekcje na wejsciu (ta kolejnego itego poziomu)


            nodes2 = new List<SiteNode>();
            for (int i = 0; i < nodes.Count; i++)
            {
                var nodeListTemp = new List<SiteNode>();
                nodeListTemp.AddRange(FindElementsFromSiteNode(i));
                foreach (var node in nodeListTemp)
                {
                    nodes2.Add(node);
                }
            }


            Run2();




            Console.WriteLine(nodes2[2].LinksString[2].ToString());

            Console.WriteLine("After readkey: FindElementsFromSiteNode(1)");
            Console.ReadKey();



          


            listOfNodes = nodes;

           // Console.ReadKey();
        }


        public void Run2()
        {
            
        }






        private List<SiteNode> Iterate(List<SiteNode> listOfNodes)
        {
            var foundNodes = new List<SiteNode>();
            List<SiteNode> nodeListTemp;

            for (int i = 0; i < listOfNodes.Count; i++)
            {
                nodeListTemp = new List<SiteNode>();
                nodeListTemp.AddRange(FindElementsFromSiteNode(i));
                foreach (var node in nodeListTemp)
                {
                    foundNodes.Add(node);
                }
            }
            return foundNodes;
        }

        private List<SiteNode> Iterate2(List<SiteNode> listOfNodes)
        {
            var foundNodes = new List<SiteNode>();
            List<SiteNode> nodeListTemp;

            foreach (var singleNode in listOfNodes)
            {
                nodeListTemp = new List<SiteNode>();
                nodeListTemp.AddRange(FindElementsFromSiteNode(singleNode));
                foreach (var node in nodeListTemp)
                {
                    foundNodes.Add(node);
                }
            }


            return foundNodes;
        }

        

        private List<IWebElement> FindElements()
        {
            return driver.FindElements(By.TagName("a")).Where(x => string.IsNullOrEmpty(x.Text) == false).ToList();
        }


        private SiteNode CreateRootNode()
        {
            var rootNode = new SiteNode(driver.Title, FindElements(), TakeScreenshoot());
            return rootNode;
        }

        private List<List<SiteNode>> GroupSiteNodes(List<SiteNode> listOfNodes)
        {
            var listsToReturn = new List<List<SiteNode>>();

            listsToReturn.Add(listOfNodes);

            return listsToReturn;
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
                    elementsToReturn.Add(CreateSiteNode(elements[i]));
                }
                catch(ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    NewTabHandle();
                    elements = FindElements();
                    continue;
                }
                catch (InvalidOperationException ex2)    //Element generatet by JS or being before element
                {
                    InvalidOperationHandle(elements[i], elementsToReturn);
                }


            }
            return elementsToReturn;
        }


       // private SiteNode FindElementsFromSiteNode2(SiteNode)



        private List<SiteNode> FindElementsFromSiteNode(int nodeNumber)
        {
            if(nodeNumber > 0)  //after goin inside-back to prev node site
            {
                driver.Navigate().Back();
            }

            var elementsPrevNode = FindElements();
            //var elements = nodeAbove.GetAllLinks();
            var elementsToReturn = new List<SiteNode>();

            try
            {
                elementsPrevNode[nodeNumber].Click();
            }
            catch (InvalidOperationException ex2)    //Element generatet by JS or being before element
            {
                InvalidOperationHandle(elementsPrevNode[nodeNumber], elementsToReturn);
            }

            for (int i = 0; i < elementsPrevNode.Count; i++)
            {
                try
                {
                    elementsPrevNode = FindElements();
                    elementsToReturn.Add(CreateSiteNode(elementsPrevNode[i]));
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    NewTabHandle();
                    elementsPrevNode = FindElements();
                    continue;
                }
                catch (InvalidOperationException ex2)    //Element generatet by JS or being before element
                {
                    InvalidOperationHandle(elementsPrevNode[i], elementsToReturn);
                }
                catch (StaleElementReferenceException ex3)   //Give the browser some time 4 refresh elements
                {
                    Thread.Sleep(1000);
                    continue;
                }

            }
            return elementsToReturn;
        }


        private SiteNode CreateSiteNode(IWebElement element)
        {

            element.Click();
            Thread.Sleep(500);
            var siteNode = new SiteNode(driver.Title, FindElements(), TakeScreenshoot());

            ConsoleOutput.Print(siteNode);
            ConsoleOutput.PrintAlsoToTxtFile(siteNode, savingDataPath);



            screenshotSaving.SaveScreenshotAsJpg(siteNode);

            driver.Navigate().Back();
            return siteNode;
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

        private void InvalidOperationHandle(IWebElement element, List<SiteNode> listOfElements)
        {
            string prevNodeTitle = driver.Title;
            Console.WriteLine("Somethin before");
            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
            ex.ExecuteScript("arguments[0].click();", element);

            SiteNode newNode;
            if (driver.Title == prevNodeTitle)  //if href is taking to the same site, then dont list it
            {
                return;
            }

            newNode = new SiteNode(driver.Title, FindElements(), TakeScreenshoot());
            listOfElements.Add(newNode);    //?
            ConsoleOutput.Print(newNode);
            ConsoleOutput.PrintAlsoToTxtFile(newNode, savingDataPath);
            screenshotSaving.SaveScreenshotAsJpg(newNode);
            driver.Navigate().Back();
        }

        public byte[] TakeScreenshoot()
        {
            var ss = ((ITakesScreenshot)driver).GetScreenshot();
            byte[] screenAsByteArray = ss.AsByteArray;
            return screenAsByteArray;
        }



        private void RemoveSameNodeNames(SiteNode siteNode)
        {
            var list = new List<IWebElement>(siteNode.Links);
            list.Distinct().ToList();
        }

        private void OpenUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
            siteTitle = driver.Title;

        }



    }
}
