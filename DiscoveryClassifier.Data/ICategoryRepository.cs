using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscoveryClassifier.BusinessObjects;

namespace DiscoveryClassifier.Data
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories(string searchText);
        void SaveCategory(Category category, bool isNew);
        Category GetCategory(string categoryId);
        bool ContainsCategory(Category category);
    }
}
