using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using MongoDB.Driver;
using DiscoveryClassifier.Data.DatabaseObjects;
using DiscoveryClassifier.BusinessObjects;
using NationalArchives.CommonUtilities;

namespace DiscoveryClassifier.Data
{
    [Obsolete]
    public class CategoryRepository : ICategoryRepository
    {
        private IMongoCollection<CategoryData> m_MongoCollection = null;

        public CategoryRepository()
        {
            try
            {
                string serverName = ConfigurationManager.AppSettings["CategoryDatabaseConnectionString"];
                string databaseName = ConfigurationManager.AppSettings["CategoryDatabaseName"]; ;
                string collectionName = ConfigurationManager.AppSettings["CategoryDatabaseCollectionName"];

                var client = new MongoClient(serverName);
                var database = client.GetDatabase(databaseName);
                m_MongoCollection = database.GetCollection<CategoryData>(collectionName);
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                throw ex;
            }
        }

        public List<Category> GetCategories(string searchText)
        {
            try
            {
                // BNO : Added  [c.id != null] as there's a doc with a null id in the collection which causes a BSON serialisation exception (and which
                // can't readily be deleted!)
                return m_MongoCollection.Find(c => c.ttl.ToLower().Contains(searchText.ToLower()) && c.id != null).SortBy(x => x.ttl).ToList()
                                        .Select(y => new Category() { CategoryId = y.CIAID, Title = y.ttl, Query = y.qry, Score = y.SC, Lock = y.lck }).ToList();
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                throw ex;
            }
        }

        public Category GetCategory(string categoryId)
        {
            try
            {
                var category = m_MongoCollection.Find(c => c.CIAID == categoryId).FirstOrDefault();

                return new Category()
                {
                    CategoryId = category.CIAID,
                    Title = category.ttl,
                    Query = category.qry,
                    Score = category.SC,
                    Lock = category.lck
                };
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                throw ex;
            }
        }

        public void SaveCategory(Category category, bool isNew)
        {
            try
            {
                var categoryData = m_MongoCollection.Find(c => c.CIAID == category.CategoryId).FirstOrDefault();
                if (isNew)
                {
                    categoryData = new CategoryData() { CIAID = category.CategoryId, ttl = category.Title, qry = category.Query, SC = category.Score, lck = category.Lock };
                    m_MongoCollection.InsertOne(categoryData);
                }
                else
                {
                    categoryData.ttl = category.Title;
                    categoryData.qry = category.Query;
                    categoryData.SC = category.Score;
                    categoryData.lck = category.Lock;
                    m_MongoCollection.ReplaceOne(c => c.CIAID == category.CategoryId, categoryData);
                }
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                throw ex;
            }
        }

        public bool ContainsCategory(Category category)
        {
            try
            {
                return m_MongoCollection.Count(c => c.CIAID == category.CategoryId) > 0;
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                throw ex;
            }
        }
    }
}
