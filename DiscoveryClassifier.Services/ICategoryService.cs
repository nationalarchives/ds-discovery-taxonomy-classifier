using System;
using System.Collections.Generic;
using System.ServiceModel;
using DiscoveryClassifier.BusinessObjects;

namespace DiscoveryClassifier.Services
{
    /// <summary>
    /// This interface sets contracts for WCF service
    /// to work with Categories through repository
    /// </summary>
    [Obsolete]
    [ServiceContract]
    public interface ICategoryService
    {
        [OperationContract]
        CategoryList GetCategories(string searchText);

        [OperationContract]
        Category GetCategory(string categoryId);

        [OperationContract]
        void SaveCategory(Category category, bool isNew);

        [OperationContract]
        bool ContainsCategory(Category category);
    }
}
