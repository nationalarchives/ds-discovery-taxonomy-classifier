using DiscoveryClassifier.BusinessObjects;
using System.Collections.Generic;

namespace DiscoveryClassifier.ServiceClient
{
    public interface IRestServiceClientCategories
    {
        List<Category> GetCategories(string searchText);
        Category GetCategoryById(string categoryId);
        void SaveCategory(Category category, bool isNew);
    }
}