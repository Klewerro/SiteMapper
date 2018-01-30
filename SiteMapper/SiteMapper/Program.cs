using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SiteMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SiteMapper.Output;

namespace SeleniumProjInz2_v2
{
    class Program
    {
        static void Main(string[] args)
        {
            var driver = new ChromeDriver();
            var objectiveMethod = new ObjectiveMethod(driver, Paths.siteToMapAddress);

            objectiveMethod.Run();
        }

    }
}
