using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Controls;
using DiscoveryClassifier.Services;
using DiscoveryClassifier.BusinessObjects;
using DiscoveryClassifier.UI.Services;
using DiscoveryClassifier.UI.Services.RESTParameter;
using System.ComponentModel;
using System.Windows;
using NationalArchives.CommonUtilities;
using System.Threading;
using DiscoveryClassifier.ServiceClient;

namespace DiscoveryClassifier.UI.ViewModel
{
    public class CategoriesViewModel : ViewModelBase, IDataErrorInfo
    {
        private IRestServiceClientCategories m_CategoryServiceClient;
        private IRestServiceClient m_DocumentServiceClient;
        private Category m_Category;
        private Category m_SelectedCategory;
        private List<Category> m_Categories;
        private List<IAView> m_IAViewResults;
        private int m_Offset;
        private int m_Limit;
        private string  m_Error;
        protected CategoryStatus m_CategoryCurrentStatus;
        private string m_ResultsMessage;
        private bool m_ScoreIsEnabled;
        private bool m_NameIsEnabled;

        private const string CategoryIdPropertyName = "CategoryId";
        private const string ScoreIsEnabledPropertyName = "ScoreIsEnabled";
        private const string NameIsEnabledPropertyName = "NameIsEnabled";
        private const string CategoryNamePropertyName = "CategoryName";
        private const string QueryPropertyName = "Query";
        private const string ScorePropertyName = "Score";
        private const string LockPropertyName = "Lock";
        private const string IsNewPropertyName = "IsNew";
        private const string ShowResultPropertyName = "ShowResult";
        private const string ShowErrorPropertyName = "ShowError";
        private const string CanSavePropertyName = "CanSave";
        private const string CanRunPropertyName = "CanRun";
        private const string CanPublishPropertyName = "CanPublish";
        private const string CategoriesPropertyName = "Categories";
        private const string CategoryPropertyName = "SelectedCategory";
        private const string ResultPropertyName = "IAViewResults";
        private const string LimitPropertyName = "Limit";
        private const string OffsetPropertyName = "Offset";
        private const string ErrorPropertyName = "ErrorMessage";
        private const string ResultsPropertyName = "ResultsMessage";
        private const string StandardErrorMessage = "An unexpected error occured, please contact your administrator.";

        public CategoriesViewModel(IRestServiceClientCategories categoryService, IRestServiceClient documentServiceClient)
        {
            m_CategoryServiceClient = categoryService;
            m_DocumentServiceClient = documentServiceClient;

            m_Category = new Category();

            SearchText = "";
            Offset = 0;
            Limit = 100;
            ScoreIsEnabled = false;
            NameIsEnabled = false;
            CreateSearchCommand();
            CreateLoadCommand();
            CreateAddCommand();
            CreateRunCommand();
            CreateSaveCommand();
            CreatePublishCommand();
            CreateRefreshCommand();
            CreateNameEnableCommand();
            CreateScoreEnableCommand();
            m_DocumentServiceClient = documentServiceClient;
        }

        public string SearchText { get; set; }

        public CategoryStatus CategoryCurrentStatus
        {
            get { return m_CategoryCurrentStatus; }
            set
            {
                if (m_CategoryCurrentStatus == value)
                    return;

                m_CategoryCurrentStatus = value;

                RaisePropertyChanged(IsNewPropertyName);
                RaisePropertyChanged(CanSavePropertyName);
                RaisePropertyChanged(CanRunPropertyName);
                RaisePropertyChanged(CanPublishPropertyName);
                //RaisePropertyChanged(CanCancelPropertyName);
            }
        }

        public bool IsNew
        {
            get
            {
                return (CategoryCurrentStatus == CategoryStatus.Add);
            }
        }

        public bool NameIsEnabled
        {
            get { return m_NameIsEnabled; }
            set
            {
                if (m_NameIsEnabled == value)
                    return;

                m_NameIsEnabled = value;
                RaisePropertyChanged(NameIsEnabledPropertyName);
            }
        }

        public bool ScoreIsEnabled
        {
            get { return m_ScoreIsEnabled; }
            set
            {
                if (m_ScoreIsEnabled == value)
                    return;

                m_ScoreIsEnabled = value;
                RaisePropertyChanged(ScoreIsEnabledPropertyName);
            }
        }

        public Visibility ShowError
        {
            get
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                    return Visibility.Hidden;

                return Visibility.Visible;
            }
        }

        public Visibility ShowResult
        {
            get
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                    return Visibility.Hidden;

                return Visibility.Visible;
            }
        }

        public string CategoryId
        {
            get { return m_Category.CategoryId; }
            set
            {
                if (m_Category.CategoryId == value)
                    return;

                m_Category.CategoryId = value;
                RaisePropertyChanged(CategoryIdPropertyName);
            }
        }

        public string CategoryName
        {
            get { return m_Category.Title; }
            set
            {
                if (m_Category.Title == value)
                    return;

                m_Category.Title = value;
                RaisePropertyChanged(CategoryNamePropertyName);
                RaisePropertyChanged(CanSavePropertyName);
            }
        }

        public string Query
        {
            get { return m_Category.Query; }
            set
            {
                if (m_Category.Query == value)
                    return;

                m_Category.Query = value;
                RaisePropertyChanged(QueryPropertyName);
                RaisePropertyChanged(CanRunPropertyName);
                RaisePropertyChanged(CanSavePropertyName);
            }
        }

        public Double Score
        {
            get { return m_Category.Score; }
            set
            {
                if (m_Category.Score == value)
                    return;

                m_Category.Score = value;
                RaisePropertyChanged(ScorePropertyName);
                RaisePropertyChanged(CanSavePropertyName);
            }
        }

        public bool Lock
        {
            get 
            { 
                return m_Category.Lock; 
            }
            set
            {
                m_Category.Lock = value;
            }
        }

        public List<Category> Categories
        {
            get { return m_Categories; }
            set
            {
                m_Categories = value;
                RaisePropertyChanged(CategoriesPropertyName);
            }
        }

        public Category SelectedCategory
        {
            get { return m_SelectedCategory; }
            set
            {
                if (m_SelectedCategory == value)
                    return;

                if (value != null)
                    EditExecute(value);

                m_SelectedCategory = value;
                RaisePropertyChanged(CategoryPropertyName);
            }
        }

        public List<IAView> IAViewResults
        {
            get { return m_IAViewResults; }
            set
            {
                m_IAViewResults = value;
                RaisePropertyChanged(ResultPropertyName);
            }
        }       

        public bool CanSave
        {
            get 
            {
                //if entry is Locked can't save
                if (Lock) return false;

                if (this[CategoryIdPropertyName] != null)
                    return false;

                if (this[CategoryNamePropertyName] != null)
                    return false;

                if (this[ScorePropertyName] != null)
                    return false;

                if (this[QueryPropertyName] != null)
                    return false;

                //if add new allow to save
                if (CategoryCurrentStatus == CategoryStatus.Add) return true;

                ////if not edit return false
                if (!(CategoryCurrentStatus == CategoryStatus.Edit || CategoryCurrentStatus == CategoryStatus.Saved))
                    return false;

                var originalCategory = from category in Categories
                                       where category.CategoryId == CategoryId &&
                                             category.Title == CategoryName &&   
                                             category.Score == Score &&   
                                             category.Query == Query
                                        select category;

                //when its edit and user didn't change any data no need to Save again
                if (originalCategory.Count() > 0) return false;

                return true;
            }
        }

        public bool CanCancel
        {
            get
            {
                return (CategoryCurrentStatus == CategoryStatus.Add || CategoryCurrentStatus == CategoryStatus.Edit);
            }
        }

        public bool CanRun
        {
            get 
            {
                if (Lock || CategoryCurrentStatus == CategoryStatus.None) 
                    return false;

                return (!string.IsNullOrWhiteSpace(Query)); 
            }
        }

        public bool CanPublish
        {
            get
            {
                if (Lock) return false;

                if (CategoryCurrentStatus == CategoryStatus.Saved || 
                    CategoryCurrentStatus == CategoryStatus.Edit) 
                    return true;

                return false;
            }
        }

        public int Limit
        {
            get { return m_Limit; }
            set
            {
                if (m_Limit == value)
                    return;

                m_Limit = value;
                RaisePropertyChanged(LimitPropertyName);
            }
        }

        public int Offset
        {
            get { return m_Offset; }
            set
            {
                if (m_Offset == value)
                    return;

                m_Offset = value;
                RaisePropertyChanged(OffsetPropertyName);
            }
        }

        public string ErrorMessage
        {
            get { return m_Error; }
            set
            {
                if (m_Error == value)
                    return;

                m_Error = value;
                RaisePropertyChanged(ErrorPropertyName);
            }
        }

        public string ResultsMessage
        {
            get { return m_ResultsMessage; }
            set
            {
                if (m_ResultsMessage == value)
                    return;

                m_ResultsMessage = value;
                RaisePropertyChanged(ResultsPropertyName);
            }
        }

        #region "Event Commands"

        public ICommand ScoreEnableCommand { get; internal set; }

        private void CreateScoreEnableCommand()
        {
            ScoreEnableCommand = new RelayCommand(ScoreEnableExecute);
        }

        public void ScoreEnableExecute()
        {
            if (MessageBox.Show("Use of a threshold on score has a strong impact on the speed of categorisation. Only use if really needed, Do you want to continue?", "Edit Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                ScoreIsEnabled = true; 
            else
                ScoreIsEnabled = false;
        }

        public ICommand NameEnableCommand { get; internal set; }

        private void CreateNameEnableCommand()
        {
            NameEnableCommand = new RelayCommand(NameEnableExecute);
        }

        public void NameEnableExecute()
        {
            if (MessageBox.Show("Changing the category name will be taken into account for all new documents or updates but not on already categorised documents." + Environment.NewLine + Environment.NewLine + "Please modify it only if updates are frozen and if a whole re-categorisation is planned, Do you want to continue?", "Edit Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                NameIsEnabled = true;
            else
                NameIsEnabled = false;
        }

        public ICommand SearchCommand { get; internal set;}

        private void CreateSearchCommand()
        {
            SearchCommand = new RelayCommand(SearchExecute);
        }

        public void SearchExecute()
        {
            try
            {
                
                if (String.IsNullOrEmpty(SearchText))
                {
                    Categories = m_CategoryServiceClient.GetCategories(String.Empty); 
                }
                else
                {
                    Categories = m_CategoryServiceClient.GetCategories(SearchText);
                }
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                MessageBox.Show(StandardErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
         }

        public ICommand LoadCommand { get; internal set; }

        private void CreateLoadCommand()
        {
            LoadCommand = new RelayCommand(SearchExecute);
        }

        private bool CanExecuteEditCommand()
        {
            if (!CanSave)
                return true;

            if (MessageBox.Show("Current changes not saved, Do you want to edit a different category?", "Confirmation",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                return true;

            return false;
        }

        private void EditExecute(Category category)
        {
            if (CanExecuteEditCommand())
            {
                CategoryCurrentStatus = CategoryStatus.Edit;
                LoadCategory(category.CategoryId);
                NameIsEnabled = false;
                ScoreIsEnabled = false;
            }
        }

        private void LoadCategory(string categoryId)
        {
            ErrorMessage = "";
            try
            {
                Category editItem = m_CategoryServiceClient.GetCategoryById(categoryId);

                CategoryId = editItem.CategoryId;
                CategoryName = editItem.Title;
                Score = editItem.Score;
                Query = editItem.Query;
                Lock = editItem.Lock;
                IAViewResults = null;

                RaisePropertyChanged(CanSavePropertyName);
                RaisePropertyChanged(CanRunPropertyName);
                RaisePropertyChanged(CanPublishPropertyName);
                //RaisePropertyChanged(CanCancelPropertyName);

                if (Lock)
                    ErrorMessage = string.Format("Update training set is in progress for the category '{0}' and currently locked for editing.", CategoryName);
                else
                    ErrorMessage = string.Empty;

                RaisePropertyChanged(ShowResultPropertyName);
                RaisePropertyChanged(ShowErrorPropertyName);
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                MessageBox.Show(StandardErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand AddCommand { get; internal set;}

        private void CreateAddCommand()
        {
            AddCommand = new RelayCommand(AddExecute);
        }

        public void AddExecute()
        {
            MessageBox.Show(Resources.AddNewCategoryErrorMsg, "Categories");
            
            // CategoryCurrentStatus = CategoryStatus.Add;
            Clear();
            return;
        }

        public ICommand SaveCommand { get; internal set;}

        private void CreateSaveCommand()
        {
            SaveCommand = new RelayCommand(SaveExecute);
        }

        public void SaveExecute()
        {
            try
            {
                m_CategoryServiceClient.SaveCategory(m_Category, IsNew);

                SearchExecute();
                CategoryCurrentStatus = CategoryStatus.Saved;
                RaisePropertyChanged(CanSavePropertyName);
                MessageBox.Show(Resources.CategorySavedSuccessfully, "Save Confirmation", MessageBoxButton.OK, 
                                        MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                MessageBox.Show(StandardErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand RunCommand { get; internal set;}

        private void CreateRunCommand()
        {
            RunCommand = new RelayCommand(RunExecute);
        }

        public void RunExecute()
        {
            try
            {
                IAViewResults = null;
                ErrorMessage = string.Empty;

                var searchRequest = new SearchRequest()
                {
                    categoryQuery = Query,
                    limit = Limit,
                    offset = Offset,
                    score = Score
                };
                if (m_DocumentServiceClient.Search(searchRequest))
                {
                    int numberOfResults = m_DocumentServiceClient.SearchResults.numberOfResults;
                    IAViewResults = m_DocumentServiceClient.SearchResults.results;
                    ResultsMessage = string.Format("{0:N0} documents returned.", numberOfResults);
                    //Limit = Limit > numberOfResults ? numberOfResults : Limit;
                }
                else
                    ErrorMessage = GetError(m_DocumentServiceClient.Error);

                RaisePropertyChanged(ShowResultPropertyName);
                RaisePropertyChanged(ShowErrorPropertyName);
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                MessageBox.Show(StandardErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand PublishCommand { get; internal set; }

        private void CreatePublishCommand()
        {
            PublishCommand = new RelayCommand(PublishExecute);
        }

        private bool CanExecutePublishCommand()
        {
            //if its Saved can publish
            if (CategoryCurrentStatus == CategoryStatus.Saved) return true;

            var originalCategory = from category in Categories
                                   where category.CategoryId == CategoryId &&
                                         category.Title == CategoryName &&
                                         category.Score == Score &&
                                         category.Query == Query
                                   select category;

            //when its edit and user didn't change any data can publish
            if (originalCategory.Count() > 0) return true;

            MessageBox.Show("Current changes not saved, please save to publish!", "Save Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            return false;
        }

        public void PublishExecute()
        {
            if (CanExecutePublishCommand())
            {
                try
                {
                    ErrorMessage = string.Empty;

                    var publishRequest = new PublishRequest()
                    {
                        CIAID = CategoryId
                    };
                    if (m_DocumentServiceClient.Publish(publishRequest))
                    {
                        CategoryCurrentStatus = CategoryStatus.Saved;
                        MessageBox.Show("Category publish initiated successfully!", "Publish Initiated", MessageBoxButton.OK,
                                            MessageBoxImage.Information);
                        LoadCategory(CategoryId);
                    }
                    else
                        ErrorMessage = GetError(m_DocumentServiceClient.Error);

                    RaisePropertyChanged(ShowResultPropertyName);
                    RaisePropertyChanged(ShowErrorPropertyName);
                }
                catch (Exception ex)
                {
                    NALogger.Instance.LogException(this.GetType(), ex);
                    MessageBox.Show(StandardErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public ICommand RefreshCommand { get; internal set; }

        private void CreateRefreshCommand()
        {
            RefreshCommand = new RelayCommand(RefreshExecute);
        }

        public void RefreshExecute()
        {
            if (!string.IsNullOrEmpty(CategoryId) && CategoryCurrentStatus != CategoryStatus.Add)
            {
                LoadCategory(CategoryId);
            }
        }

        private void Clear()
        {
            CategoryId = string.Empty;
            CategoryName = string.Empty;
            Score = 0;
            Query = string.Empty;
            SearchText = string.Empty;
            ErrorMessage = string.Empty;
            IAViewResults = null;
        }

        private string GetError(ErrorResponse error)
        {
            if (error == null)
                return StandardErrorMessage;

            return string.Format("{0} - {1}", error.error, error.message);
        }

        #endregion

        public string Error
        {
            get { return null; }
        }

        public string this[string propertyName]
        {
            get {
                //check only for Add & Edit.
                if (!(CategoryCurrentStatus == CategoryStatus.Add || CategoryCurrentStatus == CategoryStatus.Edit))
                    return null;

                switch (propertyName)
                {
                    case CategoryIdPropertyName:
                        //if its edit no need to check for unique entry
                        if (CategoryCurrentStatus == CategoryStatus.Edit) break;

                        //if its new entry first perform required field validation
                        if (string.IsNullOrWhiteSpace(CategoryId))
                            return "Category id is required";

                        //then validate for unique entry
                        if (Categories.FindAll(c => c.CategoryId.ToLower() == CategoryId.ToLower()).Count > 0)
                            return "Category id should be unique";

                        break;
                    case CategoryNamePropertyName:
                        //First perform required field validation
                        if (string.IsNullOrWhiteSpace(CategoryName))
                            return "Category name is required";

                        //Category name also should be unique
                        //Therefore validate for unique entry when its new 
                        if (CategoryCurrentStatus == CategoryStatus.Add && 
                                Categories.FindAll(c => c.Title.ToLower() == CategoryName.ToLower()).Count() > 0)
                            return "Category name should be unique";

                        //if its edit only proceed.
                        if (CategoryCurrentStatus == CategoryStatus.Add) break;

                        //Validate for unique entry when its edit
                        //Identify the category name using the category id
                        //when its changed it should not be available in the category list
                        //var categories= m_CategoryServiceClinet.GetCategories(string.Empty).List;
                        var categories = m_CategoryServiceClient.GetCategories(String.Empty);
                        var categoryName = categories.Find(c => c.CategoryId.ToLower() == CategoryId.ToLower()).Title.ToLower();
                        if (categoryName != CategoryName.ToLower() && 
                            Categories.FindAll(c => c.Title.ToLower() == CategoryName.ToLower()).Count() > 0)
                            return "Category name should be unique";
                        break;
                    case ScorePropertyName:
                        if (Score < 0)
                            return "Valid score is required";
                        break;
                    case QueryPropertyName:
                        if (string.IsNullOrWhiteSpace(Query))
                            return "Query is required";
                        break;
                }
                return null;
            }
        }

        public enum CategoryStatus
        {
            None = 0,
            Add = 1,
            Edit = 2,
            Saved = 3
        }
    }
}