using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DiscoveryClassifier.Data.DatabaseObjects
{
    public class CategoryData
    {
        public Object id { get; set; }
        [BsonElement("TAXONOMY_ID")]
        public string CIAID { get; set; }
        [BsonElement("TAXONOMY")]
        public string ttl { get; set; }
        [BsonElement("QUERY_TEXT")]
        public string qry { get; set; }
        public double SC { get; set; }
        public bool lck { get; set; }
    }
}
