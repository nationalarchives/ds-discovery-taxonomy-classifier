using DiscoveryClassifier.BusinessObjects;
using DiscoveryClassifier.UI.Services.RESTParameter;
using System.Collections.Generic;

namespace DiscoveryClassifier.UI.Services
{
    public interface IRestServiceClient
    {
        List<CategoryResult> CategoryResults { get; set; }
        SearchResult SearchResults { get; set; }

        bool CheckCategory(CategoryCheckRequest parmeter);
        bool Publish(PublishRequest parmeter);
        bool Search(SearchRequest parmeter);

        ErrorResponse Error { get; set; }
    }
}