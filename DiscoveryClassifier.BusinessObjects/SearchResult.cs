using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoveryClassifier.BusinessObjects
{
    public class SearchResult
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public double minimumScore { get; set; }
        public int numberOfResults { get; set; }
        public List<IAView> results { get; set; }
    }
}
