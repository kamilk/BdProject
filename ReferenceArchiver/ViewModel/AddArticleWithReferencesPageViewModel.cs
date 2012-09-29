using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReferenceArchiver.Model;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReferenceArchiver.ViewModel.Helpers;

namespace ReferenceArchiver.ViewModel
{
    /// <summary>
    /// A ViewModel for AddArticleWithReferencesPageViewModel
    /// </summary>
    class AddArticleWithReferencesPageViewModel : WizardPageViewModelBase
    {
        #region Fields

        private ObservableCollection<Article> _selectedArticles;

        #endregion

        #region Properties

        public override string Title { get { return "Podaj dane artykułu"; } }

        /// <summary>
        /// Gets the data context for the subview
        /// </summary>
        /// <value>
        /// The add article data context.
        /// </value>
        public AddArticleViewModel AddArticleDataContext { get; private set; }

        /// <summary>
        /// Gets an ICollectionView exposing articles the user can choose from.
        /// </summary>
        /// <value>
        /// The articles to choose from.
        /// </value>
        public ICollectionView ArticlesToChooseFrom { get; private set; }

        /// <summary>
        /// Gets an ICollectionView exposing the user has selected as references.
        /// </summary>
        /// <value>
        /// The selected articles.
        /// </value>
        public ICollectionView SelectedArticles { get; private set; }

        /// <summary>
        /// Gets the command which should be executed when the user has requested the
        /// currently selected reference to be moved up.
        /// </summary>
        /// <value>
        /// The move reference up command.
        /// </value>
        public ICommand MoveReferenceUpCommand { get; private set; }

        /// <summary>
        /// Gets the command which should be executed when the user has requested the
        /// currently selected reference to be moved down.
        /// </summary>
        /// <value>
        /// The move reference down command.
        /// </value>
        public ICommand MoveReferenceDownCommand { get; private set; }

        /// <summary>
        /// Gets the command which should be executed when the user has requested the
        /// currently selected reference to be removed.
        /// </summary>
        /// <value>
        /// The remove reference command.
        /// </value>
        public ICommand RemoveReferenceCommand { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the article should be exported to Invenio
        /// XML on save.
        /// </summary>
        /// <value>
        /// <c>true</c> if the article should be exported to Invenio; otherwise, <c>false</c>.
        /// </value>
        public bool ShouldExportToInvenio { get; set; }

        #endregion

        #region Constructors

        public AddArticleWithReferencesPageViewModel(WizardViewModel parent)
            : base(parent)
        {
            AddArticleDataContext = new AddArticleViewModel(parent);

            IEnumerable<Article> articles = CentralRepository.Instance.GetArticles();
            ArticlesToChooseFrom = CollectionViewSource.GetDefaultView(articles);

            _selectedArticles = new ObservableCollection<Article>();
            SelectedArticles = CollectionViewSource.GetDefaultView(_selectedArticles);

            MoveReferenceUpCommand = new DelegateCommand(MoveReferenceUp);
            MoveReferenceDownCommand = new DelegateCommand(MoveReferenceDown);
            RemoveReferenceCommand = new DelegateCommand(RemoveReference);

            ShouldExportToInvenio = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the Article object filled with user-specified data.
        /// </summary>
        /// <returns>The article created by the user</returns>
        public Article GetArticle()
        {
            return AddArticleDataContext.GetArticle();
        }

        /// <summary>
        /// Gets the Authorship objects created from user-specified data.
        /// </summary>
        /// <returns>The authorships created by the user</returns>
        public IList<Authorship> GetAuthorships()
        {
            return AddArticleDataContext.GetAuthorships();
        }

        /// <summary>
        /// Gets the articles the user has selected as references for the edited article.
        /// </summary>
        /// <returns></returns>
        public IList<Article> GetReferences()
        {
            return _selectedArticles.ToList();
        }

        /// <summary>
        /// Adds the article as a reference for the currently edited article reference.
        /// </summary>
        /// <param name="article">The article.</param>
        public void AddReference(Article article)
        {
            if (article != null)
                _selectedArticles.Add(article);
        }

        private void MoveReferenceUp()
        {
            int currentPosition = SelectedArticles.CurrentPosition;
            if (currentPosition > 0)
                _selectedArticles.Move(currentPosition, currentPosition - 1);
        }

        private void MoveReferenceDown()
        {
            int currentPosition = SelectedArticles.CurrentPosition;
            if (currentPosition >= 0 && currentPosition < _selectedArticles.Count - 1)
                _selectedArticles.Move(currentPosition, currentPosition + 1);
        }

        private void RemoveReference()
        {
            int currentPosition = SelectedArticles.CurrentPosition;
            if (currentPosition >= 0)
                _selectedArticles.RemoveAt(currentPosition);
        }

        #endregion
    }
}
