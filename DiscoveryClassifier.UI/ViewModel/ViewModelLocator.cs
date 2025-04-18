/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:DiscoveryClassifier.UI"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using DiscoveryClassifier.Services;
using DiscoveryClassifier.UI.Services;
using DiscoveryClassifier.ServiceClient;

namespace DiscoveryClassifier.UI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //SimpleIoc.Default.Register<ICategoryService, CategoryServiceClient>();
            SimpleIoc.Default.Register<IRestServiceClientCategories, RestServiceClientCategories>();
            SimpleIoc.Default.Register<IRestServiceClient, RESTServiceClient>();
            
            SimpleIoc.Default.Register<MainViewModel>();

            var restServiceClientCategories = ServiceLocator.Current.GetInstance<IRestServiceClientCategories>();
            var restServiceClient = ServiceLocator.Current.GetInstance<IRestServiceClient>();
            SimpleIoc.Default.Register<CategoriesViewModel>(() => new CategoriesViewModel(restServiceClientCategories, restServiceClient));

            SimpleIoc.Default.Register<DocumentViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public CategoriesViewModel CategoryView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CategoriesViewModel>();
            }
        }

        public DocumentViewModel DocumentView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DocumentViewModel>();
            }
        }
        
        public static void Cleanup()
        {
        }
    }
}