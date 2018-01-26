using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SiteMapper.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;

namespace SiteMapper
{
    class ObjectiveMethod
    {
        private IWebDriver driver;
        private string siteUrl;
        private string savingDataPath = @"C:\Users\polsz\Desktop\";
        public string siteTitle;
        ScreenshotSaving screenshotSaving;
        int counterOfSites = 0;


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
            Print(rootNode);
            screenshotSaving.SaveScreenshotAsJpg(rootNode);
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
            siteTitle = driver.Title;
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
                    var newNode = new SiteNode(driver.Title, FindElements(), TakeScreenshoot());
                    elementsToReturn.Add(newNode);
                    Print(newNode);
                    screenshotSaving.SaveScreenshotAsJpg(newNode);
                    driver.Navigate().Back();
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
                    Console.WriteLine("Somethin before");
                    IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
                    ex.ExecuteScript("arguments[0].click();", elements[i]);

                    var newNode = new SiteNode(driver.Title, FindElements(), TakeScreenshoot());
                    elementsToReturn.Add(newNode);
                    Print(newNode);
                    screenshotSaving.SaveScreenshotAsJpg(newNode);
                    driver.Navigate().Back();

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
                    var newNode = new SiteNode(driver.Title, FindElements(), TakeScreenshoot());
                    elementsToReturn.Add(newNode);
                    Print(newNode);
                    screenshotSaving.SaveScreenshotAsJpg(newNode);
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
            counterOfSites++;
            Console.WriteLine($"!Current site tittle {node.Name} |{counterOfSites}");
            foreach (var element in node.Links)
            {
                Console.WriteLine(element.Text);
            }
            Console.WriteLine();

            PrintAlsoToTxtFile(node, savingDataPath);
        }

        public void PrintAlsoToTxtFile(SiteNode node, string pathToFile)
        {
            string pathWithFilename = pathToFile + "consoleLog.txt";

            if(counterOfSites == 1)
            {
                File.WriteAllText(pathWithFilename, null);
            }
           

            using (StreamWriter writer = File.AppendText(pathWithFilename))
            {
                writer.AutoFlush = true;
                writer.WriteLine($"!Current site tittle {node.Name} |{counterOfSites}");
                foreach (var element in node.Links)
                {
                    writer.WriteLine(element.Text);
                }
                writer.WriteLine();
                writer.Flush();
            }
        }

        public void ConsoleOutputToTxt(string path)
        {
            Console.WriteLine($@"Entire output will be saved in location: {path}, and file: consoleLog.txt");

            var streamwriter = new StreamWriter(path + "consoleLog.txt");
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
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




    }
}
