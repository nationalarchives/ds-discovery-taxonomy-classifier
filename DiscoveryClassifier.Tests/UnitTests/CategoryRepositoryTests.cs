using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiscoveryClassifier.Data;
using System.Collections.Generic;
using DiscoveryClassifier.BusinessObjects;
using DiscoveryClassifier.Services;
using DiscoveryClassifier.UI.Services;

namespace DiscoveryClassifier.Tests
{
    // BNOR 2025-04-14.  All tests here currently failing anyway  in main branch.  Need refactoring, these are actually 
    // integration tests and the MockCategoryRepository isn't actually used in the tests !
    [Ignore]
    [TestClass]
    public class CategoryRepositoryTests
    {
        ICategoryRepository repository;

        public CategoryRepositoryTests()
        {
            //Initialise the Mongo DB
            MockCategoryRepository testDB = new MockCategoryRepository();
            repository = new CategoryRepository();
        }

        [TestMethod]
        public void GetCategoriesTest()
        {
            var searchResults = repository.GetCategories("Test");

            Assert.AreEqual(searchResults.Count, 3);
        }

        [TestMethod]
        public void GetCategoryTest()
        {
            var categoryResults = repository.GetCategory("Test_C10008");

            Assert.AreEqual(categoryResults.Title, "Test_Canals and river transport");
        }

        [TestMethod]
        public void SaveCategoryTest()
        {
            //Add
            var category = new Category()
            {
                CategoryId = "Test_C10098",
                Title = "Test_Road transport",
                Score = 2.25,
                Query = "\"motorway\" OR \"highways\" OR \"driving license\"",
                Lock = false
            };

            repository.SaveCategory(category, true);

            var addedNewly = repository.ContainsCategory(category);

            Assert.AreEqual(addedNewly, true);

            var categoryResults = repository.GetCategory("Test_C10008");

            categoryResults.Title = "Test_River Transport";

            repository.SaveCategory(categoryResults, false);

            categoryResults = repository.GetCategory("Test_C10008");

            Assert.AreEqual(categoryResults.Title, "Test_River Transport");
        }
    }
}
