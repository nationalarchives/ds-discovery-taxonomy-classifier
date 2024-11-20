using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoveryClassifier.UI.Services.RESTParameter
{
    public class CategoryCheckRequest
    {
        public string catDocRef { get; set; }
        public string contextDescription { get; set; }
        public string coveringDates { get; set; }
        public string description { get; set; }
        public string docReference { get; set; }
        public string[] corpBodys { get; set; }
        public string[] subjects { get; set; }
        public string[] placeName { get; set; }
        public string[] personFullName { get; set; }
        public string title { get; set; }
    }
}
