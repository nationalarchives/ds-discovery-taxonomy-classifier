using DiscoveryClassifier.UI.Services.RESTParameter;
using NationalArchives.CommonUtilities;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace DiscoveryClassifier.ServiceClient
{
    public abstract class RESTServiceClientBase
    {
        public ErrorResponse Error { get; set; }
        public bool HasError { get; set; }

        protected int m_Timeout = 30000;

        protected RESTServiceClientBase()
        {
            int.TryParse(ConfigurationManager.AppSettings.Get("RESTServiceTimeout"), out m_Timeout);
        }
        protected string PostHTTPRequest(string url, string postData)
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

        protected string GetHttpRequest(string url)
        {
            string responseFromServer = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = m_Timeout;

                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
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
        protected void SetErrorFromResponse(string jsonResponse)
        {
            if (!string.IsNullOrEmpty(jsonResponse))
                Error = JsonConvert.DeserializeObject<ErrorResponse>(jsonResponse);
        }
    }
}
