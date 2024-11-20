using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscoveryClassifier.Data;
using DiscoveryClassifier.Data.DatabaseObjects;
using DiscoveryClassifier.BusinessObjects;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace DiscoveryClassifier.Tests
{
    public class MockCategoryRepository
    {
        private MongoCollection m_MongoCollection = null;

        public MockCategoryRepository()
        {
            string serverName = ConfigurationManager.AppSettings["CategoryDatabaseConnectionString"];
            string databaseName = ConfigurationManager.AppSettings["CategoryDatabaseName"]; ;
            string collectionName = ConfigurationManager.AppSettings["CategoryDatabaseCollectionName"];

            var mongoServer = new MongoClient(serverName).GetServer();
            var mongoDatabase = mongoServer.GetDatabase(databaseName);
            this.m_MongoCollection = mongoDatabase.GetCollection(collectionName);

            RemoveTestEntries();
            AddTestEntries();
        }


        private void RemoveTestEntries()
        {
            var query = Query<CategoryData>.EQ(e => e.CIAID, "Test_C10002");
            m_MongoCollection.Remove(query);

            query = Query<CategoryData>.EQ(e => e.CIAID, "Test_C10008");
            m_MongoCollection.Remove(query);

            query = Query<CategoryData>.EQ(e => e.CIAID, "Test_C10032");
            m_MongoCollection.Remove(query);

            query = Query<CategoryData>.EQ(e => e.CIAID, "Test_C10098");
            m_MongoCollection.Remove(query);
        }

        private void AddTestEntries()
        {
            BsonDocument categoryDocument = new BsonDocument();

            categoryDocument.Add("CIAID", "Test_C10002");
            categoryDocument.Add("ttl", "Test_Air Force");
            categoryDocument.Add("qry", "\"Air Force\" OR \"air forces\" OR \"Air Ministry\" OR \"Air Historical Branch\" OR \"Air Department\" OR \"Air Board\" OR \"Air Council\"");
            categoryDocument.Add("SC", 1.25);
            categoryDocument.Add("lck", false);
            m_MongoCollection.Insert(categoryDocument);
            
            categoryDocument = new BsonDocument();

            categoryDocument.Add("CIAID", "Test_C10008");
            categoryDocument.Add("ttl", "Test_Canals and river transport");
            categoryDocument.Add("qry", "\"canal\" OR \"canals\" OR \"British Waterways Board\" OR \"Narrow Boat\" OR \"National rivers authority\" OR \"british board of waterways\" OR (\"River\" AND \"dredging\") OR (\"River boat\"~5) OR (\"River cargo\"~5)");
            categoryDocument.Add("SC", 0.01);
            categoryDocument.Add("lck", false);
            m_MongoCollection.Insert(categoryDocument);

            categoryDocument = new BsonDocument();

            categoryDocument.Add("CIAID", "Test_C10032");
            categoryDocument.Add("ttl", "Test_Disease");
            categoryDocument.Add("qry", "\"disease\" OR \"tropical disease\"");
            categoryDocument.Add("SC", 0);
            categoryDocument.Add("lck", false);
            m_MongoCollection.Insert(categoryDocument);

            m_MongoCollection.Save(categoryDocument);
        }
    }
}
