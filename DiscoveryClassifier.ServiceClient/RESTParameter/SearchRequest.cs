using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoveryClassifier.UI.Services.RESTParameter
{
    public class SearchRequest
    {
        public string categoryQuery { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public double score { get; set; }
    }
}
