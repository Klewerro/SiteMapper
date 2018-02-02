using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SiteMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SiteMapper.Output;
using System.Windows.Forms;

namespace SeleniumProjInz2_v2
{
    class Program
    {
        static void Main(string[] args)
        {

            var driver = new ChromeDriver();
            var objectiveMethod = new ObjectiveMethod(driver, Paths.siteToMapAddress);
            objectiveMethod.Run(4);
            //objectiveMethod.Openurl(Paths.siteToMapAddress);
            //objectiveMethod.RunRecursive(3);

            //var requrenceMethod = new RequrenceMethod(driver, Paths.siteToMapAddress);
            //requrenceMethod.Run(null);

            //var requrenceMethod2 = new RequrenceMethod2(driver, Paths.siteToMapAddress);
            //requrenceMethod2.Run(4);

            //var simpleMethod = new SimpleMethod(driver, Paths.siteToMapAddress, 3);
            //simpleMethod.Run(1);

        }

    }
}
