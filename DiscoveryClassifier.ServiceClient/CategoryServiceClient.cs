using DiscoveryClassifier.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using DiscoveryClassifier.BusinessObjects;
using NationalArchives.CommonUtilities;

namespace DiscoveryClassifier.UI.Services
{
    public class CategoryServiceClient : ICategoryService
    {
        private const int MaxInt = 2147483647;

        private BasicHttpBinding m_Binding = null;
        private EndpointAddress m_EndpointAddress = null;

        public CategoryServiceClient()
        {
            m_Binding = new BasicHttpBinding() 
            {
                MaxBufferSize = MaxInt,
                MaxReceivedMessageSize = MaxInt,
                MaxBufferPoolSize = 524288,
                OpenTimeout = new TimeSpan(0, 10, 0),
                CloseTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 10, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                ReaderQuotas = new XmlDictionaryReaderQuotas() 
                { 
                    MaxStringContentLength = MaxInt, MaxArrayLength = MaxInt, MaxBytesPerRead = MaxInt 
                },
            };

            m_EndpointAddress = new EndpointAddress(ConfigurationManager.AppSettings.Get("DiscoveryClassifierAddress"));
        }

        public CategoryList GetCategories(string searchText)
        {
            try
            {
                using (var channelFactory = new ChannelFactory<ICategoryService>(m_Binding, m_EndpointAddress))
                {
                    return channelFactory.CreateChannel().GetCategories(searchText);
                }
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
                using (var channelFactory = new ChannelFactory<ICategoryService>(m_Binding, m_EndpointAddress))
                {
                    return channelFactory.CreateChannel().GetCategory(categoryId);
                }
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
                using (var channelFactory = new ChannelFactory<ICategoryService>(m_Binding, m_EndpointAddress))
                {
                    channelFactory.CreateChannel().SaveCategory(category, isNew);
                }
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
                using (var channelFactory = new ChannelFactory<ICategoryService>(m_Binding, m_EndpointAddress))
                {
                    return channelFactory.CreateChannel().ContainsCategory(category);
                }
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                throw ex;
            }
        }
    }
}
