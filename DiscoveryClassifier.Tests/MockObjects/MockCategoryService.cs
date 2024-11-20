using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscoveryClassifier.Services;
using DiscoveryClassifier.BusinessObjects;

namespace DiscoveryClassifier.Tests
{
    public class MockCategoryService : ICategoryService
    {
        private List<Category> m_Categories;

        public MockCategoryService()
        {
            m_Categories = new List<Category>();

            m_Categories.Add(new Category()
            {
                CategoryId = "C10002",
                Title = "Air Force",
                Score = 1.25,
                Lock = false,
                Query = "\"Air Force\" OR \"air forces\" OR \"Air Ministry\" OR \"Air Historical Branch\" OR \"Air Department\" OR \"Air Board\" OR \"Air Council\""
            });
            m_Categories.Add(new Category()
            {
                CategoryId = "C10008",
                Title = "Canals and river transport",
                Score = 0.01,
                Lock = false,
                Query = "\"canal\" OR \"canals\" OR \"British Waterways Board\" OR \"Narrow Boat\" OR \"National rivers authority\" OR \"british board of waterways\" OR (\"River\" AND \"dredging\") OR (\"River boat\"~5) OR (\"River cargo\"~5)"
            });
            m_Categories.Add(new Category()
            {
                CategoryId = "C10032",
                Title = "Disease",
                Score = 0,
                Lock = false,
                Query = "\"disease\" OR \"tropical disease\""
            });
            m_Categories.Add(new Category()
            {
                CategoryId = "C10098",
                Title = "Road transport",
                Score = 0.578,
                Lock = false,
                Query = "\"motorway\" OR \"highways\" OR \"driving license\" OR \"driving licenses\" OR \"dvla\" OR \"Driver and Vehicle Licensing Agency\" OR \"motor car\" OR \"speed limit\" OR (\"road car\"~5) OR (\"roads car\"~5) OR (\"highway car\"~5) OR (\"highways car\"~5) OR (\"road motorcycle\"~5)"
            });
            m_Categories.Add(new Category()
            {
                CategoryId = "C10125",
                Title = "Americas",
                Score = 5.87,
                Lock = false,
                Query = "\"Argentina\" OR \"Bolivia\" OR \"Brazil\" OR \"Chile\" OR \"Colombia\" OR \"Ecuador\" OR \"Falkland Islands\" OR \"Guyana\" OR \"Paraguay\" OR \"Peru\" OR \"Surinam\" OR \"Suriname\" OR \"Uruguay\" OR \"Venezuela\" OR \"French Guiana\" OR \"Belize\" OR \"Costa Rica\""
            });
        }

        public CategoryList GetCategories(string searchText)
        {
            var categories = new CategoryList();

            categories.List = m_Categories.FindAll(c => c.Title.Contains(searchText));

            return categories;
        }

        public Category GetCategory(string categoryId)
        {
            return m_Categories.Find(c => c.CategoryId == categoryId);
        }

        public void SaveCategory(Category category, bool isNew)
        {
            if (isNew)
                m_Categories.Add(category);
            else
            {
                var editCategory = m_Categories.Find(c => c.CategoryId == category.CategoryId);

                editCategory.Title = category.Title;
                editCategory.Score = category.Score;
                editCategory.Query = category.Query;
            }
        }

        public bool ContainsCategory(Category category)
        {
            return (m_Categories.Find(c => c.CategoryId == category.CategoryId) != null);
        }
    }
}
