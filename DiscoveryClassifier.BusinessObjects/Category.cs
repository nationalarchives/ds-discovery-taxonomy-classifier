using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoveryClassifier.BusinessObjects
{
    public class Category
    {
        [JsonProperty("id")]
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Query { get; set; }
        public double Score { get; set; }
        public bool Lock { get; set; }
    }
}
