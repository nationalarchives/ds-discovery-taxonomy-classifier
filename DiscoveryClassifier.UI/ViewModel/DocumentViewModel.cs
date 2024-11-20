using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using NationalArchives.CommonUtilities;
using System.Windows;
using System.IO;
using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using DiscoveryClassifier.BusinessObjects;
using DiscoveryClassifier.UI.Services;
using DiscoveryClassifier.UI.Services.RESTParameter;
using System.Diagnostics;
using System.Configuration;

namespace DiscoveryClassifier.UI.ViewModel
{
    public class DocumentViewModel : ViewModelBase
    {
        private string m_FilePath;
        private Document m_Document;
        private List<Document> m_Documents;
        private List<CategoryResult> m_CategoryResults;
        private string m_Error;
        private string m_DocumentError;
        private string m_Title;
        private string m_CatDocRef;
        private string m_CoveringDates;
        private string m_Description;
        private string m_Query;
        private bool m_IsQuery;
        private string m_DocReference;
        private string m_ContextDescription;
        private string m_CorpBodys;
        private string m_Subjects;
        private string m_Place_Name;
        private string m_Person_FullName;
        private string m_ResultsMessage;
        private string m_RecordsMessage;
        private int m_Offset;
        private int m_Limit;

        private const double Score = 0.0;

        private const string DocumentErrorPropertyName = "DocumentError";
        private const string QueryPropertyName = "Query";
        private const string IsQueryPropertyName = "IsQuery";
        private const string FilePathPropertyName = "FilePath";
        private const string TitlePropertyName = "Title";
        private const string CatDocRefPropertyName = "CatDocRef";
        private const string CoveringDatesPropertyName = "CoveringDates";
        private const string DescriptionPropertyName = "Description";
        private const string DocReferencePropertyName = "DocReference";
        private const string ContextDescriptionPropertyName = "ContextDescription";
        private const string CorpBodysPropertyName = "CorpBodys";
        private const string SubjectsPropertyName = "Subjects";
        private const string Place_NamePropertyName = "Place_Name";
        private const string Person_FullNamePropertyName = "Person_FullName";
        private const string ErrorPropertyName = "ErrorMessage";
        private const string ResultsPropertyName = "ResultsMessage";
        private const string RecordsPropertyName = "RecordsMessage";
        private const string DocumentsPropertyName = "Documents";
        private const string DocumentPropertyName = "SelectedDocument";
        private const string LimitPropertyName = "Limit";
        private const string OffsetPropertyName = "Offset";
        private const string CategoryResultsPropertyName = "ResultCategoris";
        private const string ShowResultPropertyName = "ShowResult";
        private const string ShowFileUploadPropertyName = "ShowFileUpload";
        private const string ShowQueryPropertyName = "ShowQuery";
        private const string ShowErrorPropertyName = "ShowError";
        private const string ShowDocumentErrorPropertyName = "ShowDocumentError";
        private const string ShowDocumentResultPropertyName = "ShowDocumentResult";
        private const string StandardErrorMessage = "An unexpected error occured, please contact your administrator.";

        public DocumentViewModel()
        {
            IsQuery = true;
            CreateBrowseCommand();
            CreateLoadCommand();
            CreateRunCommand();
            CreateLegacyNavigateCommand();
            CreateNavigateCommand();
            Offset = 0;
            Limit = 100;
        }

        public string FilePath
        {
            get { return m_FilePath; }
            set
            {
                if (m_FilePath == value)
                    return;

                m_FilePath = value;
                RaisePropertyChanged(FilePathPropertyName);
            }
        }

        public string Title
        {
            get { return m_Title; }
            set
            {
                if (m_Title == value)
                    return;

                m_Title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        public string CatDocRef
        {
            get { return m_CatDocRef; }
            set
            {
                if (m_CatDocRef == value)
                    return;

                m_CatDocRef = value;
                RaisePropertyChanged(CatDocRefPropertyName);
            }
        }

        public string CoveringDates
        {
            get { return m_CoveringDates; }
            set
            {
                if (m_CoveringDates == value)
                    return;

                m_CoveringDates = value;
                RaisePropertyChanged(CoveringDatesPropertyName);
            }
        }

        public string Description
        {
            get { return m_Description; }
            set
            {
                if (m_Description == value)
                    return;

                m_Description = value;
                RaisePropertyChanged(DescriptionPropertyName);
            }
        }

        public string DocReference
        {
            get { return m_DocReference; }
            set
            {
                if (m_DocReference == value)
                    return;

                m_DocReference = value;
                RaisePropertyChanged(DocReferencePropertyName);
            }
        }

        public string ContextDescription
        {
            get { return m_ContextDescription; }
            set
            {
                if (m_ContextDescription == value)
                    return;

                m_ContextDescription = value;
                RaisePropertyChanged(ContextDescriptionPropertyName);
            }
        }

        public string CorpBodys
        {
            get { return m_CorpBodys; }
            set
            {
                if (m_CorpBodys == value)
                    return;

                m_CorpBodys = value;
                RaisePropertyChanged(CorpBodysPropertyName);
            }
        }

        public string Subjects
        {
            get { return m_Subjects; }
            set
            {
                if (m_Subjects == value)
                    return;

                m_Subjects = value;
                RaisePropertyChanged(SubjectsPropertyName);
            }
        }

        public string Place_Name
        {
            get { return m_Place_Name; }
            set
            {
                if (m_Place_Name == value)
                    return;

                m_Place_Name = value;
                RaisePropertyChanged(Place_NamePropertyName);
            }
        }

        public string Person_FullName
        {
            get { return m_Person_FullName; }
            set
            {
                if (m_Person_FullName == value)
                    return;

                m_Person_FullName = value;
                RaisePropertyChanged(Person_FullNamePropertyName);
            }
        }

        public string Query
        {
            get { return m_Query; }
            set
            {
                if (m_Query == value)
                    return;

                m_Query = value;
                RaisePropertyChanged(QueryPropertyName);
            }
        }

        public bool IsQuery
        {
            get { return m_IsQuery; }
            set
            {
                if (m_IsQuery == value)
                    return;

                m_IsQuery = value;
                RaisePropertyChanged(IsQueryPropertyName);
                RaisePropertyChanged(ShowFileUploadPropertyName);
                RaisePropertyChanged(ShowQueryPropertyName);
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

        public List<Document> Documents
        {
            get { return m_Documents; }
            set
            {
                m_Documents = value;
                RaisePropertyChanged(DocumentsPropertyName);
            }
        }

        public Document SelectedDocument
        {
            get { return m_Document; }
            set
            {
                if (m_Document == value)
                    return;

                m_Document = value;
                PriviewExecute(value);
                RaisePropertyChanged(DocumentPropertyName);
            }
        }

        public List<CategoryResult> ResultCategoris
        {
            get { return m_CategoryResults; }
            set
            {
                m_CategoryResults = value;
                RaisePropertyChanged(CategoryResultsPropertyName);
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

        public string RecordsMessage
        {
            get { return m_RecordsMessage; }
            set
            {
                if (m_RecordsMessage == value)
                    return;

                m_RecordsMessage = value;
                RaisePropertyChanged(RecordsPropertyName);
            }
        }

        public string DocumentError
        {
            get { return m_DocumentError; }
            set
            {
                if (m_DocumentError == value)
                    return;

                m_DocumentError = value;
                RaisePropertyChanged(DocumentErrorPropertyName);
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

        public Visibility ShowDocumentError
        {
            get
            {
                if (string.IsNullOrEmpty(DocumentError))
                    return Visibility.Hidden;

                return Visibility.Visible;
            }
        }

        public Visibility ShowDocumentResult
        {
            get
            {
                if (!string.IsNullOrEmpty(DocumentError))
                    return Visibility.Hidden;

                return Visibility.Visible;
            }
        }

        public Visibility ShowFileUpload
        {
            get
            {
                if (IsQuery)
                    return Visibility.Hidden;

                return Visibility.Visible;
            }
        }

        public Visibility ShowQuery
        {
            get
            {
                if (IsQuery)
                    return Visibility.Visible;

                return Visibility.Hidden;
            }
        }

        public ICommand BrowseCommand { get; internal set; }

        private void CreateBrowseCommand()
        {
            BrowseCommand = new RelayCommand(BrowseExecute);
        }

        public void BrowseExecute()
        {
            try
            {
                // Create OpenFileDialog 
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.DefaultExt = ".json";
                dlg.Filter = "JSON Files (*.json)|*.json";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                    FilePath = dlg.FileName;
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                MessageBox.Show(StandardErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public ICommand RunCommand { get; internal set; }

        private void CreateRunCommand()
        {
            RunCommand = new RelayCommand(RunExecute);
        }

        public void RunExecute()
        {
            try
            {
                DocumentError = "";
                SelectedDocument = null;
                var restServiceClient = new RESTServiceClient();
                var searchRequest = new SearchRequest()
                {
                    categoryQuery = Query,
                    limit = Limit,
                    offset = Offset,
                    score = Score
                };
                if (restServiceClient.Search(searchRequest)) 
                {
                    var searchResults = restServiceClient.SearchResults;

                    if (searchResults.numberOfResults > Limit)
                        ResultsMessage = string.Format("From {0:N0} to {1:N0} documents shown, out of {2:N0} documents returned.", Offset, Offset + Limit, searchResults.numberOfResults);
                    else
                        ResultsMessage = string.Format("{0:N0} documents returned.", searchResults.numberOfResults);

                    Documents = (from doc in searchResults.results
                                     select new Document()
                                     {
                                         Title = doc.title,
                                         CatDocRef = doc.catDocRef,
                                         DocReference = doc.docReference,
                                         CoveringDates = doc.coveringDates,
                                         Description = doc.description,
                                         ContextDescription = doc.contextDescription,
                                         CorpBodys = doc.corpBodys,
                                         Subjects = doc.subjects,
                                         Person_FullName = doc.personFullName,
                                         Place_Name = doc.placeName
                                     }).ToList<Document>();

                }
                else
                    DocumentError = GetError(restServiceClient.Error);

                RaisePropertyChanged(ShowDocumentErrorPropertyName);
                RaisePropertyChanged(ShowDocumentResultPropertyName);
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
            LoadCommand = new RelayCommand(LoadExecute);
        }

        public void LoadExecute()
        {
            try
            {
                DocumentError = "";
                string jsonDocuments = "";
                using (StreamReader fileReader = new StreamReader(FilePath))
                {
                    jsonDocuments = fileReader.ReadToEnd();
                }
                dynamic documents = JArray.Parse(jsonDocuments);

                Documents = new List<Document>();

                foreach (var document in documents)
                {
                    Documents.Add(new Document() {
                        Title = document.TITLE,
                        CatDocRef = document.CATDOCREF,
                        DocReference = document.DOCREFERENCE,
                        CoveringDates = document.COVERINGDATES,
                        Description = document.DESCRIPTION,
                        ContextDescription = document.CONTEXTDESCRIPTION,
                        CorpBodys = document.CORPBODYS.ToObject<string[]>(),
                        Subjects = document.SUBJECTS.ToObject<string[]>(),
                        Person_FullName = document.PERSON_FULLNAME.ToObject<string[]>(),
                        Place_Name = document.PLACE_NAME.ToObject<string[]>()
                    });
                }

                RecordsMessage = string.Format("{0:N0} documents found.", Documents.Count);

                RaisePropertyChanged(ShowDocumentErrorPropertyName);
                RaisePropertyChanged(ShowDocumentResultPropertyName);
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                MessageBox.Show(StandardErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetError(ErrorResponse error)
        {
            if (error == null)
                return StandardErrorMessage;

            return string.Format("{0} - {1}", error.error, error.message);
        }

        private void PriviewExecute(Document document)
        {
            try
            {
                ErrorMessage = "";

                if (document == null)
                {
                    Title = string.Empty;
                    CatDocRef = string.Empty;
                    CoveringDates = string.Empty;
                    Description = string.Empty;
                    DocReference = string.Empty;
                    ContextDescription = string.Empty;
                    CorpBodys = string.Empty;
                    Subjects = string.Empty;
                    Place_Name = string.Empty;
                    Person_FullName = string.Empty;
                    ResultCategoris = null;
                }
                else
                {
                    Title = document.Title;
                    CatDocRef = document.CatDocRef;
                    CoveringDates = document.CoveringDates;
                    Description = document.Description;
                    DocReference = document.DocReference;
                    ContextDescription = document.ContextDescription;
                    CorpBodys = string.Join(",", document.CorpBodys);
                    Subjects = string.Join(",", document.Subjects);
                    Place_Name = string.Join(",", document.Place_Name);
                    Person_FullName = string.Join(",", document.Person_FullName);

                    var restServiceClient = new RESTServiceClient();
                    var categoryCheckRequest = new CategoryCheckRequest()
                    {
                        catDocRef = CatDocRef,
                        contextDescription = document.ContextDescription,
                        coveringDates = CoveringDates,
                        description = Description,
                        docReference = document.DocReference,
                        corpBodys = document.CorpBodys,
                        subjects = document.Subjects,
                        placeName = document.Place_Name,
                        personFullName = document.Person_FullName,
                        title = Title
                    };
                    if (restServiceClient.CheckCategory(categoryCheckRequest))
                        ResultCategoris = restServiceClient.CategoryResults;
                    else
                        ErrorMessage = GetError(restServiceClient.Error);

                    RaisePropertyChanged(ShowResultPropertyName);
                    RaisePropertyChanged(ShowErrorPropertyName);
                }
            }
            catch (Exception ex)
            {
                NALogger.Instance.LogException(this.GetType(), ex);
                MessageBox.Show(StandardErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand LegacyNavigateCommand { get; internal set; }

        private void CreateLegacyNavigateCommand()
        {
            LegacyNavigateCommand = new RelayCommand<string>(LegacyNavigateExecute);
        }

        public void LegacyNavigateExecute(string CatDocRef)
        {
            string url = ConfigurationManager.AppSettings["LegacyDiscoveryURL"] + CatDocRef;
            Process.Start(new ProcessStartInfo(url));
        }

        public ICommand NavigateCommand { get; internal set; }

        private void CreateNavigateCommand()
        {
            NavigateCommand = new RelayCommand<string>(NavigateExecute);
        }

        public void NavigateExecute(string DocReference)
        {
            string url = ConfigurationManager.AppSettings["DiscoveryURL"] + DocReference;
            Process.Start(new ProcessStartInfo(url));
        }
    }
}
