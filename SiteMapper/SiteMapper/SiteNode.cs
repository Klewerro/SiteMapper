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

        public SiteNode(string name, List<IWebElement> list)
        {
            this.Name = name;
            links = new List<IWebElement>(list);
        }

        public void AddLink(IWebElement link)
        {
            links.Add(link);
        }

        public void AddRangeLinks(List<IWebElement> list)
        {
            links.AddRange(list);
        }

        public IWebElement GetLink(int id)
        {
            return links[id];
        }

        public List<IWebElement> GetAllLinks()
        {
            return links;
        }


    }
}
