using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiscoveryClassifier.UI.ViewModel;
using DiscoveryClassifier.Services;
using DiscoveryClassifier.BusinessObjects;

namespace DiscoveryClassifier.Tests
{
    [TestClass]
    public class CategoriesViewModelTest
    {
        [TestMethod]
        public void SearchTest()
        {
            var testCategoryService = new MockCategoryService();
            var testCategoryViewModel = new CategoriesViewModel(testCategoryService);

            testCategoryViewModel.SearchText = "Air";
            testCategoryViewModel.SearchExecute();

            Assert.AreEqual(testCategoryViewModel.Categories.Count, 1);
        }

        [TestMethod]
        public void AddTest()
        {
            var testCategoryService = new MockCategoryService();
            var testCategoryViewModel = new CategoriesViewModel(testCategoryService);

            testCategoryViewModel.SearchText = "Pay";
            testCategoryViewModel.SearchExecute();

            Assert.AreEqual(testCategoryViewModel.Categories.Count, 0);

            testCategoryViewModel.AddExecute();

            testCategoryViewModel.CategoryId = "C10077";
            testCategoryViewModel.CategoryName = "Pay and pensions";
            testCategoryViewModel.Score = 1.25;
            testCategoryViewModel.Lock = false;
            testCategoryViewModel.Query = "\"pension\" OR \"pensions\" OR \"pensionable\" OR \"supperannuation\" OR \"supperannuations\" OR \"wage\" OR \"wages\" OR \"salary\" OR \"salaries\"";

            testCategoryViewModel.SaveExecute();

            testCategoryViewModel.SearchText = "Pay";
            testCategoryViewModel.SearchExecute();

            Assert.AreEqual(testCategoryViewModel.Categories.Count, 1);
        }

        [TestMethod]
        public void EditTest()
        {
            var testCategoryService = new MockCategoryService();
            var testCategoryViewModel = new CategoriesViewModel(testCategoryService);

            testCategoryViewModel.SearchText = "";
            testCategoryViewModel.SearchExecute();

            Assert.AreEqual(testCategoryViewModel.Categories.Count, 5);

            //testCategoryViewModel.EditExecute("C10032");

            Assert.AreEqual(testCategoryViewModel.CanSave, false);
            Assert.AreEqual(testCategoryViewModel.CanPublish, true);
            Assert.AreEqual(testCategoryViewModel.CanRun, true);
            Assert.AreEqual(testCategoryViewModel.CanCancel, true);

            testCategoryViewModel.CategoryId = "C10032";
            testCategoryViewModel.CategoryName = "Air Ambulance";
            testCategoryViewModel.Score = 2.25;
            testCategoryViewModel.Lock = false;
            testCategoryViewModel.Query = "test query";

            Assert.AreEqual(testCategoryViewModel.CanSave, true);

            testCategoryViewModel.SaveExecute();

            testCategoryViewModel.SearchText = "Air";
            testCategoryViewModel.SearchExecute();

            Assert.AreEqual(testCategoryViewModel.Categories.Count, 2);
        }

        [TestMethod]
        public void ValidationTest()
        {
            var testCategoryService = new MockCategoryService();
            var testCategoryViewModel = new CategoriesViewModel(testCategoryService);

            testCategoryViewModel.SearchText = "";
            testCategoryViewModel.SearchExecute();

            Assert.AreEqual(testCategoryViewModel.Categories.Count, 5);

            //Edit Validations
            //testCategoryViewModel.EditExecute("C10002");

            testCategoryViewModel.CategoryName = "";
            string error = testCategoryViewModel["CategoryName"];
            Assert.AreEqual("Category name is required", error);

            testCategoryViewModel.CategoryName = "Disease";
            error = testCategoryViewModel["CategoryName"];
            Assert.AreEqual("Category name should be unique", error);

            testCategoryViewModel.Score = -0.2;
            error = testCategoryViewModel["Score"];
            Assert.AreEqual("Valid score is required", error);

            testCategoryViewModel.Query = "";
            error = testCategoryViewModel["Query"];
            Assert.AreEqual("Query is required", error);

            //Add Validations
            testCategoryViewModel.AddExecute();

            testCategoryViewModel.CategoryId = "";
            error = testCategoryViewModel["CategoryId"];
            Assert.AreEqual("Category id is required", error);

            testCategoryViewModel.CategoryId = "C10002";
            error = testCategoryViewModel["CategoryId"];
            Assert.AreEqual("Category id should be unique", error);

            testCategoryViewModel.CategoryName = "";
            error = testCategoryViewModel["CategoryName"];
            Assert.AreEqual("Category name is required", error);

            testCategoryViewModel.CategoryName = "Disease";
            error = testCategoryViewModel["CategoryName"];
            Assert.AreEqual("Category name should be unique", error);

            testCategoryViewModel.Score = -0.2;
            error = testCategoryViewModel["Score"];
            Assert.AreEqual("Valid score is required", error);

            testCategoryViewModel.Query = "";
            error = testCategoryViewModel["Query"];
            Assert.AreEqual("Query is required", error);
        }
    }
}
