using DiscoveryClassifier.BusinessObjects;
using DiscoveryClassifier.ServiceClient;
using DiscoveryClassifier.UI.Services.RESTParameter;
using NationalArchives.CommonUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DiscoveryClassifier.UI.Services
{
    public class RESTServiceClient : RESTServiceClientBase
    {
        private string m_ServiceURL;

        public SearchResult SearchResults { get; set; }
        public List<CategoryResult> CategoryResults { get; set; }

        public RESTServiceClient() : base()
        {
            m_ServiceURL = ConfigurationManager.AppSettings.Get("RESTServiceAddress");
        }

        public bool Search(SearchRequest parmeter)
        {
            try
            {
                string jsonParameters = JsonConvert.SerializeObject(parmeter);

                string jsonResponse = PostHTTPRequest(m_ServiceURL + "/search", jsonParameters);

                if (HasError)
                {
                    SetErrorFromResponse(jsonResponse);
                    return false;
                }

                SearchResults = JsonConvert.DeserializeObject<SearchResult>(jsonResponse);
                return true;
            }
            catch (Exception ex)
            {
                HasError = true;
                NALogger.Instance.LogException(this.GetType(), ex);
                throw ex;
            }
        }

        public bool Publish(PublishRequest parmeter)
        {
            try
            {
                string jsonParameters = JsonConvert.SerializeObject(parmeter);

                string jsonResponse = PostHTTPRequest(m_ServiceURL + "/publish", jsonParameters);

                if (HasError)
                {
                    SetErrorFromResponse(jsonResponse);
                    return false;
                }

                if (JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse)["status"] == "OK")
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                HasError = true;
                NALogger.Instance.LogException(this.GetType(), ex);
                throw ex;
            }
        }

        public bool CheckCategory(CategoryCheckRequest parmeter)
        {
            try
            {
                string jsonParameters = JsonConvert.SerializeObject(parmeter);

                string jsonResponse = PostHTTPRequest(m_ServiceURL + "/testCategoriseSingle", jsonParameters);

                if (HasError)
                {
                    SetErrorFromResponse(jsonResponse);
                    return false;
                }

                CategoryResults = JsonConvert.DeserializeObject<List<CategoryResult>>(jsonResponse);
                return true;
            }
            catch (Exception ex)
            {
                HasError = true;
                NALogger.Instance.LogException(this.GetType(), ex);
                throw;
            }
        }
    }
}
