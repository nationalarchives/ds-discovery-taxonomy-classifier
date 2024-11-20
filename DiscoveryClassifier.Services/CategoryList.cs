using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using DiscoveryClassifier.BusinessObjects;

namespace DiscoveryClassifier.Services
{
    [KnownType(typeof(List<Category>))]
    [DataContract]
    public class CategoryList
    {
        [DataMember]
        public List<Category> List { get; set; }

        public CategoryList()
        {
            List = new List<Category>();
        }
    }
}