using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoveryClassifier.BusinessObjects
{
    public class CategoryResult
    {
        [JsonProperty("categoryName")]
        public string name { get; set; }
        public double score { get; set; }
        public int numberOfFoundDocuments { get; set; }
    }
}
