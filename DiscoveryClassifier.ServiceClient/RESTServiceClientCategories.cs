using DiscoveryClassifier.BusinessObjects;
using DiscoveryClassifier.Services;
using NationalArchives.CommonUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace DiscoveryClassifier.ServiceClient
{
    public class RestServiceClientCategories : RESTServiceClientBase, IRestServiceClientCategories
    {
        private string m_CategoriesURL;

        public RestServiceClientCategories() : base()
        {
            m_CategoriesURL = ConfigurationManager.AppSettings.Get("RESTServiceAddressCategories");
        }

        /// <summary>
        /// Retrives a list of categories from the Taxonomy API
        /// </summary>
        /// <param name="searchText">if supplied, only categories whose title contains this text are returned.  Otherwise all categories are returned.</param>
        /// <returns></returns>
        public List<Category> GetCategories(string searchText)
        {
            try
            {
                string jsonResponse = null;

                if (!String.IsNullOrEmpty(searchText))
                {
                    jsonResponse = GetHttpRequest(m_CategoriesURL + "/Search?searchText=" + searchText + "&forceRefresh=true");
                }
                else
                {
                    jsonResponse = GetHttpRequest(m_CategoriesURL + "/GetCategories?forceRefresh=true");
                }


                if (HasError)
                {
                    SetErrorFromResponse(jsonResponse);
                    //return false;
                }

                List<Category> listOfCategories = JsonConvert.DeserializeObject<List<Category>>(jsonResponse);
                return listOfCategories;
            }
            catch (Exception ex)
            {
                HasError = true;
                NALogger.Instance.LogException(this.GetType(), ex);
                throw;
            }
        }

         /// <summary>
         /// Rerieves a single category by ID from the taxonomy API
         /// </summary>
         /// <param name="categoryId">ID of the category to be returned e.g. C10101</param>
         /// <returns>Details of the category</returns>
        public Category GetCategoryById(string categoryId)
        {
            string jsonResponse = null;

            try
            {
                jsonResponse = GetHttpRequest(m_CategoriesURL + "/GetCategoryById?categoryId=" + categoryId);

                if (HasError)
                {
                    SetErrorFromResponse(jsonResponse);
                    //return false;
                }

                Category category = JsonConvert.DeserializeObject<Category>(jsonResponse);
                return category;
            }
            catch (Exception ex)
            {
                HasError = true;
                NALogger.Instance.LogException(this.GetType(), ex);
                throw;
            }
        }

        /// <summary>
        /// Saves a new or updated category definition to the Mongo database via the Taxonomy API
        /// </summary>
        /// <param name="category">Category definition</param>
        /// <param name="isNew">True for a newly added category with a new ID and Title, otherwise false</param>
        /// <exception cref="ArgumentException">Since adding new categories is not currently supported.</exception>
        public void SaveCategory(Category category, bool isNew)
        {
            if (isNew)
            {
                throw new ArgumentException("Adding new categories is not currently supported.  Please contact your administrator.");
            }

            try
            {
                string jsonParameters = JsonConvert.SerializeObject(category);

                string jsonResponse = PostHTTPRequest(m_CategoriesURL + "/SaveCategory", jsonParameters);

                if (HasError)
                {
                    SetErrorFromResponse(jsonResponse);
                    //return false;
                }


                //using (var channelFactory = new ChannelFactory<ICategoryService>(m_Binding, m_EndpointAddress))
                //{
                //    channelFactory.CreateChannel().SaveCategory(category, isNew);
                //}
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
