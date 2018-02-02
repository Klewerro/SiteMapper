using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMapper
{
    class NodesCollection
    {
        public Dictionary<int, ICollection<SiteNode>> SiteNodeDictionary { get; private set; }

        public NodesCollection()
        {
            SiteNodeDictionary = new Dictionary<int, ICollection<SiteNode>>();
        }

        public void AddNodeCollectionToCollection(ICollection<SiteNode> siteNodeCollection, int siteNodeId)
        {
            SiteNodeDictionary.Add(siteNodeId, siteNodeCollection);
        }

        



    }
}
