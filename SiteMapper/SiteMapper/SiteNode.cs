using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMapper
{
    class SiteNode
    {
        public string Name { get; set; }
        private List<IWebElement> links;
        public List<IWebElement> Links
        {
            get { return links; }
            set { links = value; }
        }

        public SiteNode(string name, List<IWebElement> list)
        {
            this.Name = name;
            links = new List<IWebElement>(list);
        }

        



    }
}
