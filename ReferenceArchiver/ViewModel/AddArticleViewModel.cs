using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ReferenceArchiver.Model;
using System.Windows.Data;
using ReferenceArchiver.ViewModel.Helpers;

namespace ReferenceArchiver.ViewModel
{
    /// <summary>
    /// A ViewModel for AddArticleView view.
    /// </summary>
    class AddArticleViewModel : WizardPageViewModelBase
    {
        #region Fields

        private Article _editedArticle;
        private string _articleTitle;
        private string _articlePolishTitle;
        private int? _startPageNumber;
        private int? _endPageNumber;
        IEnumerable<Language> _languages;
        private ObservableCollection<AuthorshipData> _authorships = new ObservableCollection<AuthorshipData>();
        private ObservableCollection<Author> _authorsToChooseFrom = new ObservableCollection<Author>();

        #endregion

        #region Properties

        public override string Title { get { return "Podaj dane artykułu"; } }

        /// <summary>
        /// Gets or sets the article title.
        /// </summary>
        /// <value>
        /// The article title.
        /// </value>
        public string ArticleTitle
        {
            get { return _articleTitle; }
            set
            {
                _articleTitle = value;
                NotifyPropertyChanged("ArticleTitle");
            }
        }

        /// <summary>
        /// Gets or sets the polish title for the article.
        /// </summary>
        /// <value>
        /// The article polish title.
        /// </value>
        public string ArticlePolishTitle
        {
            get
            {
                return _articlePolishTitle;
            }
            set
            {
                _articlePolishTitle = value;
                NotifyPropertyChanged("ArticlePolishTitle");
            }
        }
        
        /// <summary>
        /// Gets or sets the start page number.
        /// </summary>
        /// <value>
        /// The start page number.
        /// </value>
        public int? StartPageNumber
        {
            get
            {
                return _startPageNumber;
            }
            set
            {
                _startPageNumber = value;
                NotifyPropertyChanged("StartPageNumber");
            }
        }
        
        /// <summary>
        /// Gets or sets the end page number.
        /// </summary>
        /// <value>
        /// The end page number.
        /// </value>
        public int? EndPageNumber
        {
            get
            {
                return _endPageNumber;
            }
            set
            {
                _endPageNumber = value;
                NotifyPropertyChanged("EndPageNumber");
            }
        }

        /// <summary>
        /// Gets an ICollectionView for the languages the user may choose from.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        public ICollectionView Languages { get; private set; }

        /// <summary>
        /// Gets an ICollectionView for the authors the user may choose from.
        /// </summary>
        /// <value>
        /// The authors to choose from.
        /// </value>
        public ICollectionView AuthorsToChooseFrom { get; private set; }

        /// <summary>
        /// Gets an ICollectionView for the institutuions the user may choose from.
        /// </summary>
        /// <value>
        /// The institutions to choose from.
        /// </value>
        public ICollectionView InstitutionsToChooseFrom { get; private set; }

        /// <summary>
        /// Gets an ICollectionView for the authorships which should be displayed for the current
        /// article.
        /// </summary>
        /// <value>
        /// The authorships.
        /// </value>
        public ICollectionView Authorships { get; private set; }

        /// <summary>
        /// Gets the command which should be executed when the user requests the currenlty selected
        /// authorship to be moved up.
        /// </summary>
        /// <value>
        /// The move authorship up command.
        /// </value>
        public ICommand MoveAuthorshipUpCommand { get; private set; }

        /// <summary>
        /// Gets the command which should be executed when the user requests the currenlty selected
        /// authorship to be moved down.
        /// </summary>
        /// <value>
        /// The move authorship down command.
        /// </value>
        public ICommand MoveAuthorshipDownCommand { get; private set; }

        /// <summary>
        /// Gets the command which should be executed when the user requests the currenlty selected
        /// authorship to be removed.
        /// </summary>
        /// <value>
        /// The remove authorship command.
        /// </value>
        public ICommand RemoveAuthorshipCommand { get; private set; }

        #endregion

        #region Events

        public event EventHandler<EditedArticleEventArgs> EditedArticleChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddArticleViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public AddArticleViewModel(WizardViewModel parent)
            : base(parent)
        {
            _languages = CentralRepository.Instance.GetLanguages();
            Languages = CollectionViewSource.GetDefaultView(_languages);
            Languages.MoveCurrentToFirst();

            _authorsToChooseFrom = new ObservableCollection<Author>(CentralRepository.Instance.GetAuthors());
            AuthorsToChooseFrom = CollectionViewSource.GetDefaultView(_authorsToChooseFrom);

            InstitutionsToChooseFrom = CollectionViewSource.GetDefaultView(CentralRepository.Instance.GetInstitutions());
            Authorships = CollectionViewSource.GetDefaultView(_authorships);

            MoveAuthorshipDownCommand = new DelegateCommand(MoveAuthorshipDown);
            MoveAuthorshipUpCommand = new DelegateCommand(MoveAuthorshipUp);
            RemoveAuthorshipCommand = new DelegateCommand(RemoveAuthorship);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the article created from user-specified data.
        /// </summary>
        /// <returns></returns>
        public Article GetArticle()
        {
            var article = new Article()
            {
                Title = ArticleTitle,
                TitlePl = ArticlePolishTitle,
                PageBegin = StartPageNumber,
                PageEnd = EndPageNumber,
                Lang = ((Language)Languages.CurrentItem).CountryCode
            };

            if (_editedArticle != null)
                article.Id = _editedArticle.Id;

            return article;
        }

        /// <summary>
        /// Gets the authorships for the currently edited article.
        /// </summary>
        /// <returns></returns>
        public IList<Authorship> GetAuthorships()
        {
            IList<Authorship> result = 
                (from authorshipData in _authorships
                select new Authorship
                {
                    AuthorId = authorshipData.Author.Id,
                    InstitutionId = authorshipData.Affiliation.Id
                }).ToList();

            for (int i = 0; i < result.Count; i++)
                result[i].Number = i + 1;

            return result;
        }

        /// <summary>
        /// Adds the authorship to the currently edited article.
        /// </summary>
        /// <param name="author">The author.</param>
        /// <param name="affiliation">The affiliation.</param>
        public void AddAuthorship(object author, object affiliation)
        {
            var authorTyped = author as Author;
            var affiliationTyped = affiliation as Institution;
            if (author != null && affiliation != null)
                _authorships.Add(new AuthorshipData() { Author = authorTyped, Affiliation = affiliationTyped });
        }

        /// <summary>
        /// Adds the author to the currently edited article.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="middleName">Middle name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="nationality">Nationality.</param>
        public void AddAuthor(string firstName, string middleName, string lastName, Country nationality)
        {
            var author = new Author(-1, lastName, firstName, middleName, nationality.Code);
            CentralRepository.Instance.SaveAuthor(author);
            _authorsToChooseFrom.Add(author);
        }

        public void SetEditedArticle(Article article)
        {
            _editedArticle = article;

            ArticleTitle = article.Title;
            ArticlePolishTitle = article.TitlePl;
            StartPageNumber = article.PageBegin;
            EndPageNumber = article.PageEnd;
            
            Languages.MoveCurrentTo(_languages.FirstOrDefault(
                language => language.CountryCode == article.Lang));

            IEnumerable<AuthorshipData> newAuthorships = 
                CentralRepository.Instance.GetAuthorshipDataForArticle(article);

            _authorships.Clear();
            foreach (var authorship in newAuthorships)
                _authorships.Add(authorship);

            if (EditedArticleChanged != null)
                EditedArticleChanged(this, new EditedArticleEventArgs(article));
        }

        private void RemoveAuthorship()
        {
            var authorship = Authorships.CurrentItem as AuthorshipData;
            if (authorship != null)
                _authorships.Remove(authorship);
        }

        private void MoveAuthorshipUp()
        {
            var selectedIndex = Authorships.CurrentPosition;
            if (selectedIndex <= 0)
                return;
            _authorships.Move(selectedIndex, selectedIndex - 1);
        }

        private void MoveAuthorshipDown()
        {
            var selectedIndex = Authorships.CurrentPosition;
            if (selectedIndex < 0 || selectedIndex >= _authorships.Count - 1)
                return;
            _authorships.Move(selectedIndex, selectedIndex + 1);
        }

        #endregion
    }
}
