using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SiteMapper.Output;
using SiteMapper.IO;

namespace SiteMapper
{
    public class ObjectiveMethod
    {
        private IWebDriver driver;
        private string siteUrl;
        private string savingDataPath = Paths.savingDataPath;
        public string siteTitle;

        public static int nodeNumberId = 0;

        public ObjectiveMethod(IWebDriver driver, string url)
        {
            this.driver = driver;
            siteUrl = url;
        }

        public List<SiteNode> Run()
        {
            SiteNode rootNode;
            List<SiteNode> nodes;
            List<SiteNode> nodes2;


            


            OpenUrl(siteUrl);

            rootNode = CreateRootNode();

            nodes = new List<SiteNode>(FindElementsFromSiteNode(rootNode));

            List<SiteNode> rootListed = new List<SiteNode>();
            rootListed.Add(rootNode);



            //obudować w jakąś metodę!! do iteracyjnego wywołania
            //moze podac jakas kolekcje na wejsciu(ta kolejnego itego poziomu)

            nodes2 = new List<SiteNode>();
            for (int i = 0; i < nodes.Count; i++)
            {
                var nodeListTemp = new List<SiteNode>();

                nodeListTemp.AddRange(FindElementsFromSiteNode(nodes[i], i));
                foreach (var node in nodeListTemp)
                {
                    nodes2.Add(node);
                }
            }

            NavigateToFirstElementOfList(nodes2);
            var nodes3 = new List<SiteNode>();
            for (int i = 0; i < nodes2.Count; i++)
            {
                int[] nOfElementsArray = new int[nodes2.Count];
                for (int y = 0; y < nodes2.Count; y++)
                {
                    nOfElementsArray[y] = nodes2[y].Links.Count - 1;
                }


                var nodeListTemp = new List<SiteNode>();

                nodeListTemp.AddRange(FindElementsFromSiteNode(nodes2[i], nOfElementsArray[i]));
                foreach (var node in nodeListTemp)
                {
                    nodes3.Add(node);
                }

            }


            NavigateToFirstElementOfList(nodes3);
            var nodes4 = new List<SiteNode>();
            for (int i = 0; i < nodes3.Count; i++)
            {
                int[] nOfElementsArray = new int[nodes3.Count];
                for (int y = 0; y < nodes3.Count; y++)
                {
                    nOfElementsArray[y] = nodes3[y].Links.Count - 1;
                }


                var nodeListTemp = new List<SiteNode>();

                nodeListTemp.AddRange(FindElementsFromSiteNode(nodes3[i], nOfElementsArray[i]));
                foreach (var node in nodeListTemp)
                {
                    nodes4.Add(node);
                }

            }


            var list = new List<SiteNode>();
            list.Add(rootNode);
            list.AddRange(nodes);
            list.AddRange(nodes2);
            list.AddRange(nodes3);
            list.AddRange(nodes4);


            int j = 0;

            

            //Run();
            
            return list;
        }

        public void Openurl(string siteUrl)
        {
            OpenUrl(siteUrl);
        }

        public List<SiteNode>RunRecursive(int nOfIterations)
        {
            var listToRetrun = new List<SiteNode>();
            List<SiteNode> root = new List<SiteNode>();
            root.Add(CreateRootNode());
            List<SiteNode> nodes = FindElementsFromSiteNode(root[0]);
            listToRetrun.AddRange(root);
            listToRetrun.AddRange(nodes);

            nOfIterations--;
            if (nOfIterations > 0)
                RunRecursive(nOfIterations);
            return listToRetrun;
        }


        private void NavigateToFirstElementOfList(List<SiteNode> siteNodes)
        {
            var singleNode = siteNodes[0];
            driver.Navigate().GoToUrl(singleNode.Url);
        }




        private List<IWebElement> FindElements()
        {
            try
            {
                return driver.FindElements(By.TagName("a")).Where(x => string.IsNullOrEmpty(x.Text) == false).ToList();
            } catch (StaleElementReferenceException ex)
            {
                Thread.Sleep(1000);
                return driver.FindElements(By.TagName("a")).Where(x => string.IsNullOrEmpty(x.Text) == false).ToList();
            }
            
        }


        private SiteNode CreateRootNode()
        {
            var rootNode = new SiteNode(driver.Title, FindElements(), 0, TakeScreenshoot(), driver.Url);
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
                    elementsToReturn.Add(CreateSiteNode(elements[i], rootNode.SiteNodeId));
                }
                catch(ArgumentOutOfRangeException ex)
                {
                    NewTabHandle();
                    elements = FindElements();
                    continue;
                }
                catch (InvalidOperationException ex2)    //Element generated by JS or being before element
                {
                    InvalidOperationHandle(elements[i], rootNode,elementsToReturn);
                }


            }
            return elementsToReturn;
        }

        //private List<SiteNode> FourIteration()
        //{

        //}



        private List<SiteNode> FindElementsFromSiteNode(SiteNode rootNode, int indexToClick)
        {
            

            if (indexToClick > 0)  //after goin inside-back to prev node site
            {
                driver.Navigate().Back();
            }

            var elementsPrevNode = FindElements();
            //var elements = nodeAbove.GetAllLinks();
            var elementsToReturn = new List<SiteNode>();

            try
            {
                elementsPrevNode[indexToClick].Click();
            }
            catch (InvalidOperationException ex2)    //Element generatet by JS or being before element
            {
                //InvalidOperationHandle(elementsPrevNode[nodeNumber], elementsToReturn);
            }

            //maybbn
            var elements = rootNode.Links;

            for (int i = 0; i < elements.Count; i++)
            {
                try
                {
                    elements = FindElements();
                    elementsToReturn.Add(CreateSiteNode(elements[i], rootNode.SiteNodeId));
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    NewTabHandle();
                    elements = FindElements();
                    continue;
                }
                catch (InvalidOperationException ex2)    //Element generated by JS or being before element
                {
                    InvalidOperationHandle(elements[i], rootNode, elementsToReturn);
                }


            }
            return elementsToReturn;
        }



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
                //InvalidOperationHandle(elementsPrevNode[nodeNumber], elementsToReturn);
            }

            for (int i = 0; i < elementsPrevNode.Count; i++)
            {
                try
                {
                    elementsPrevNode = FindElements();
                    elementsToReturn.Add(CreateSiteNode(elementsPrevNode[i], nodeNumber));
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    NewTabHandle();
                    elementsPrevNode = FindElements();
                    continue;
                }
                catch (InvalidOperationException ex2)    //Element generatet by JS or being before element
                {
                    //InvalidOperationHandle(elementsPrevNode[i], , elementsToReturn);
                }
                catch (StaleElementReferenceException ex3)   //Give the browser some time 4 refresh elements
                {
                    Thread.Sleep(1000);
                    continue;
                }

            }
            return elementsToReturn;
        }


        private SiteNode CreateSiteNode(IWebElement element, int parentId)
        {
            element.Click();
            Thread.Sleep(500);
            var siteNode = new SiteNode(driver.Title, FindElements(), parentId, TakeScreenshoot(), driver.Url);






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

        private void InvalidOperationHandle(IWebElement element, SiteNode node, List<SiteNode> listOfElements)
        {
            string prevNodeTitle = driver.Title;
            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
            ex.ExecuteScript("arguments[0].click();", element);

            SiteNode newNode;
            if (driver.Title == prevNodeTitle)  //if href is taking to the same site, then dont list it
            {
                return;
            }
            newNode = new SiteNode(driver.Title, FindElements(), node.ParentNodeId, TakeScreenshoot(), driver.Url);
            listOfElements.Add(newNode);    //?
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
