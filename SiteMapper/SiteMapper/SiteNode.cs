using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMapper
{
    public class SiteNode
    {
        public string Name { get; set; }
        public List<IWebElement> Links { get; set; }     
        public List<string> LinksString { get; set; }
        public byte[] Screenshot { get; set; }
        public int SiteNodeId { get; private set; }
        public int ParentNodeId { get; private set; }

        public SiteNode(string name, List<IWebElement> list, int parentNodeId, byte[] screenshot)
        {
            Name = name;
            Links = new List<IWebElement>(list);
            Screenshot = screenshot;
            LinksString = ParseIWebElemetsNamesToString(Links);
            ObjectiveMethod.nodeNumberId++;
            SiteNodeId = ObjectiveMethod.nodeNumberId;
            ParentNodeId = parentNodeId;
        }



        private List<string> ParseIWebElemetsNamesToString(List<IWebElement> webElements)
        {

            var listOfStrings = new List<string>();
            foreach (var element in webElements)
            {
                listOfStrings.Add(element.Text.ToString());
            }
            return listOfStrings;
        }

        

        



    }
}
