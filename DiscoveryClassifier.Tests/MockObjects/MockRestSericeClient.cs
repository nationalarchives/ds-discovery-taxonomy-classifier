using DiscoveryClassifier.BusinessObjects;
using DiscoveryClassifier.UI.Services;
using DiscoveryClassifier.UI.Services.RESTParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscoveryClassifier.Tests.MockObjects
{
    internal class MockRestServiceClient : IRestServiceClient
    {
        public List<CategoryResult> CategoryResults { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public SearchResult SearchResults { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ErrorResponse Error { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CheckCategory(CategoryCheckRequest parmeter)
        {
            throw new NotImplementedException();
        }

        public bool Publish(PublishRequest parmeter)
        {
            throw new NotImplementedException();
        }

        public bool Search(SearchRequest parmeter)
        {
            throw new NotImplementedException();
        }
    }
}
