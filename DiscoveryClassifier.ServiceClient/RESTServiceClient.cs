using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DiscoveryClassifier.BusinessObjects;
using DiscoveryClassifier.UI.Services.RESTParameter;
using Newtonsoft.Json;
using NationalArchives.CommonUtilities;

namespace DiscoveryClassifier.UI.Services
{
    public class RESTServiceClient
    {
        private string m_ServiceURL;
        private int m_Timeout = 30000;

        public SearchResult SearchResults { get; set; }
        public List<CategoryResult> CategoryResults { get; set; }
        public ErrorResponse Error { get; set; }
        public bool HasError { get; set; }

        public RESTServiceClient()
        {
            m_ServiceURL = ConfigurationManager.AppSettings.Get("RESTServiceAddress");
            int.TryParse(ConfigurationManager.AppSettings.Get("RESTServiceTimeout"), out m_Timeout);
        }

        public bool Search(SearchRequest parmeter)
        {
            try
            {
                string jsonParameters = JsonConvert.SerializeObject(parmeter);

                string jsonResponse = PostHTTPRequest(m_ServiceURL + "/search", jsonParameters);

                if (HasError)
                {
                    if (!string.IsNullOrEmpty(jsonResponse))
                        Error = JsonConvert.DeserializeObject<ErrorResponse>(jsonResponse);

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
                    if (!string.IsNullOrEmpty(jsonResponse))
                        Error = JsonConvert.DeserializeObject<ErrorResponse>(jsonResponse);

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
                    if (!string.IsNullOrEmpty(jsonResponse))
                        Error = JsonConvert.DeserializeObject<ErrorResponse>(jsonResponse);

                    return false;
                }

                CategoryResults = JsonConvert.DeserializeObject<List<CategoryResult>>(jsonResponse);
                return true;
            }
            catch (Exception ex)
            {
                HasError = true;
                NALogger.Instance.LogException(this.GetType(), ex);
                throw ex;
            }
        }

        private string PostHTTPRequest(string url, string postData)
        {
            string responseFromServer = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = m_Timeout;

                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                responseFromServer = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();
                HasError = false;
            }
            catch (WebException webEx)
            {
                HasError = true;
                if (webEx.Status == WebExceptionStatus.Timeout)
                {
                    Error = new ErrorResponse()
                    {
                        error = "Timeout",
                        message = webEx.Message
                    };
                    NALogger.Instance.LogException(this.GetType(), webEx);
                    return responseFromServer;
                }
                else
                {
                    using (WebResponse response = webEx.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            responseFromServer = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                NALogger.Instance.LogException(this.GetType(), ex);
            }
            return responseFromServer;
        }
    }
}
