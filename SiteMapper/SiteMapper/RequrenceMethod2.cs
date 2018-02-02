//using OpenQA.Selenium;
//using SiteMapper.IO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using SiteMapper.Output;

//namespace SiteMapper
//{
//    class RequrenceMethod2
//    {
//        private IWebDriver driver;
//        private string siteUrl;
//        private string savingDataPath = Paths.savingDataPath;
//        public string siteTitle;
//        ScreenshotSaving screenshotSaving;
//        public List<SiteNode> wynik;


//        public RequrenceMethod2(IWebDriver driver, string url)
//        {
//            this.driver = driver;
//            siteUrl = url;
//            OpenUrl(siteUrl);
//            screenshotSaving = new ScreenshotSaving(savingDataPath, siteTitle);
//            wynik = new List<SiteNode>();
//        }

//        public void Run()
//        {

//            SiteNode rootNode;
            

//            //ConsoleOutputToTxt(screenshootsPath);

//            OpenUrl(siteUrl);
//            screenshotSaving = new ScreenshotSaving(savingDataPath, siteTitle);

//            rootNode = CreateRootNode();
//            ConsoleOutput.Print(rootNode);
//            ConsoleOutput.PrintAlsoToTxtFile(rootNode, savingDataPath);
//            screenshotSaving.SaveScreenshotAsJpg(rootNode);

//            var x = new List<SiteNode>();
//            x.Add(rootNode);

//            Iteration(x);


//            Console.ReadKey();
//        }


//        private void Iteration(List<SiteNode> rootNodes)
//        {
//            var list = new List<SiteNode>();
//            for (int i = 0; i < rootNodes.Count; i++)
//            {
//                //var elementsToClick = rootNodes[i].Links;
//                var elementsToClick = FindElements();
//                for (int j = 0; j < elementsToClick.Count; j++)
//                {
//                    elementsToClick = FindElements();
//                    elementsToClick[j].Click();
//                    list.Add(CreateRootNode());
//                    ConsoleOutput.Print(list[j]);
//                    ConsoleOutput.PrintAlsoToTxtFile(list[j], savingDataPath);
//                    screenshotSaving.SaveScreenshotAsJpg(list[j]);

//                    driver.Navigate().Back();
//                }
//                wynik.AddRange(list);

//            }
//            Iteration(list);
//        }




//        private void OpenUrl(string url)
//        {
//            driver.Navigate().GoToUrl(url);
//            siteTitle = driver.Title;

//        }

//        private List<IWebElement> FindElements()
//        {
//            return driver.FindElements(By.TagName("a")).Where(x => string.IsNullOrEmpty(x.Text) == false).ToList();
//        }


//        private SiteNode CreateRootNode()
//        {
//            var rootNode = new SiteNode(driver.Title, FindElements(), TakeScreenshoot());
//            return rootNode;
//        }



//        private List<SiteNode> FindElementsFromSiteNode(SiteNode rootNode)
//        {
//            var elements = rootNode.Links;
//            var elementsToReturn = new List<SiteNode>();


//            for (int i = 0; i < elements.Count; i++)
//            {
//                try
//                {
//                    elements = FindElements();
//                    elementsToReturn.Add(CreateSiteNode(elements[i]));
//                }
//                catch (ArgumentOutOfRangeException ex)
//                {
//                    Console.WriteLine(ex.StackTrace);
//                    NewTabHandle();
//                    elements = FindElements();
//                    continue;
//                }
//                catch (InvalidOperationException ex2)    //Element generatet by JS or being before element
//                {
//                    InvalidOperationHandle(elements[i], elementsToReturn);
//                }


//            }
//            return elementsToReturn;
//        }




        


//        private SiteNode CreateSiteNode(IWebElement element)
//        {

//            element.Click();
//            Thread.Sleep(500);
//            var siteNode = new SiteNode(driver.Title, FindElements(), TakeScreenshoot());

//            ConsoleOutput.Print(siteNode);
//            ConsoleOutput.PrintAlsoToTxtFile(siteNode, savingDataPath);
//            screenshotSaving.SaveScreenshotAsJpg(siteNode);

//            driver.Navigate().Back();
//            return siteNode;
//        }

//        private void NewTabHandle()
//        {
//            var handles = driver.WindowHandles.ToArray();
//            driver.SwitchTo().Window(handles[1]);
//            driver.Close();
//            driver.SwitchTo().Window(handles[0]);
//            Thread.Sleep(500);  //Browser time 4 close tab
//            driver.Navigate().Forward();

//        }

//        private void InvalidOperationHandle(IWebElement element, List<SiteNode> listOfElements)
//        {
//            string prevNodeTitle = driver.Title;
//            Console.WriteLine("Somethin before");
//            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
//            ex.ExecuteScript("arguments[0].click();", element);

//            SiteNode newNode;
//            if (driver.Title == prevNodeTitle)  //if href is taking to the same site, then dont list it
//            {
//                return;
//            }

//            newNode = new SiteNode(driver.Title, FindElements(), TakeScreenshoot());
//            listOfElements.Add(newNode);    //?
//            ConsoleOutput.Print(newNode);
//            ConsoleOutput.PrintAlsoToTxtFile(newNode, savingDataPath);
//            screenshotSaving.SaveScreenshotAsJpg(newNode);
//            driver.Navigate().Back();
//        }

//        public byte[] TakeScreenshoot()
//        {
//            var ss = ((ITakesScreenshot)driver).GetScreenshot();
//            byte[] screenAsByteArray = ss.AsByteArray;
//            return screenAsByteArray;
//        }



//        private void RemoveSameNodeNames(SiteNode siteNode)
//        {
//            var list = new List<IWebElement>(siteNode.Links);
//            list.Distinct().ToList();
//        }




//    }
//}
