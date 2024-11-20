using DiscoveryClassifier.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DiscoveryClassifier.BusinessObjects;
using NationalArchives.CommonUtilities;

namespace DiscoveryClassifier.Services
{

    /// <summary>
    /// WCF service which wraps Category repository
    /// </summary>
    public class CategoryService : ICategoryService
    {
        ICategoryRepository _categoryRepository = new CategoryRepository();

        /// <summary>
        /// Mock Repository. Uncomment and use instead real Repository (above) for 
        /// integration testing without MongoDB database
        /// </summary>
        ///
        ///ICategoryRepository _categoryRepository = new MockCategoryRepository();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CategoryService()
        { }

        /// <summary>
        /// Dependency injection. ICategoryRepository type
        /// injected into constructor
        /// </summary>
        /// <param name="categoryRepository"></param>
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public CategoryList GetCategories(string searchText)
        {
            try
            {
                CategoryList categories = new CategoryList();
                categories.List = _categoryRepository.GetCategories(searchText);
                return categories;
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
                return _categoryRepository.GetCategory(categoryId);
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
                _categoryRepository.SaveCategory(category, isNew);
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
                return _categoryRepository.ContainsCategory(category);
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                throw ex;
            }
        }
    }
}
