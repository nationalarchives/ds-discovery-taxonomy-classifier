using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoveryClassifier.BusinessObjects
{
    public class Document
    {
        public string Title { get; set; }
        public string CatDocRef { get; set; }
        public string DocReference { get; set; }
        public string CoveringDates { get; set; }
        public string Description { get; set; }
        public string ContextDescription { get; set; }
        public string[] CorpBodys { get; set; }
        public string[] Subjects { get; set; }
        public string[] Place_Name { get; set; }
        public string[] Person_FullName { get; set; }
    }
}
