using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoveryClassifier.BusinessObjects
{
    public class IAView
    {
        public object _id { get; set; }
        public double score { get; set; }
        public string categories { get; set; }
        public string coveringDates { get; set; }
        public string[] corpBodys { get; set; }
        public string[] subjects { get; set; }
        [JsonProperty("Place_Name")]
        public string[] placeName { get; set; }
        [JsonProperty("Person_FullName")]
        public string[] personFullName { get; set; }
        public string catDocRef { get; set; }
        public string docReference { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string contextDescription { get; set; }
    }
}
