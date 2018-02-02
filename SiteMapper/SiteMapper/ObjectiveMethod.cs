﻿using OpenQA.Selenium;
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

        public List<SiteNode> Run(int n)
        {
            
            if (n < 1) return null;

            SiteNode rootNode;
            List<SiteNode> nodes;
            var resultList = new List<SiteNode>();
            OpenUrl(siteUrl);

            rootNode = CreateRootNode();
            var rootListed = new List<SiteNode>();
            rootListed.Add(rootNode);
            resultList.Add(rootNode);
            n--;
            if(n < 1)  return resultList;

            nodes = new List<SiteNode>(FindElementsFromSiteNode(rootNode));
            resultList.AddRange(nodes);
            n--;
            if (n < 1) return resultList;

//_______________________REQURENCE________________________________

            //var nodes2 = FindElementsFromBiggestNode(nodes);
            //resultList.AddRange(nodes2);
            //n--;
            //if (n < 1) return resultList;

            //var nodes3 = FindElementsFromBiggestNode(nodes2);
            //resultList.AddRange(nodes3);
            //n--;
            //if (n < 1) return resultList;

            //var nodes4 = FindElementsFromBiggestNode(nodes3);
            //resultList.AddRange(nodes4);
            //n--;
            //if (n < 1) return resultList;


            for (int i=n; i > 0 ; i--)
            {
                var tempNode = Recursion(nodes);
                resultList.AddRange(tempNode);
            }



            int j = 0;

            

            //Run();
            
            return resultList;
        }




        public void Openurl(string siteUrl)
        {
            OpenUrl(siteUrl);
        }

        


        private void NavigateToFirstElementOfList(List<SiteNode> siteNodes)
        {
            var singleNode = siteNodes[0];
            driver.Navigate().GoToUrl(singleNode.Url);
        }

        private List<SiteNode> Recursion(List<SiteNode> prevNode)
        {
            return FindElementsFromBiggestNode(prevNode);
            
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

        private List<SiteNode> FindElementsFromBiggestNode(List<SiteNode> prevNodeList)
        {
            NavigateToFirstElementOfList(prevNodeList);

            var listToReturn = new List<SiteNode>();
            int[] nOfElementsArray = CreateArrayOfNodesLength(prevNodeList);

            for (int i = 0; i < prevNodeList.Count; i++)
            {
                var nodeListTemp = new List<SiteNode>();
                nodeListTemp.AddRange(FindElementsFromSiteNode(prevNodeList[i], nOfElementsArray[i]));
                foreach (var node in nodeListTemp)
                {
                    listToReturn.Add(node);
                }
            }
            return listToReturn;
        }

        private int[] CreateArrayOfNodesLength(List<SiteNode> nodeList)
        {
            int[] nOfElementsArray = new int[nodeList.Count];
            for (int i = 0; i < nodeList.Count; i++)
            {
                nOfElementsArray[i] = nodeList[i].Links.Count() - 1;    
                //-1 couse of lists iteration from 0
            }
            return nOfElementsArray;
        }



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
