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
    class AddArticleWithReferencesPageViewModel : AddArticleViewModel
    {
        #region Fields

        private ObservableCollection<Article> _selectedArticles;

        #endregion

        #region Properties

        public ICollectionView ArticlesToChooseFrom { get; private set; }
        public ICollectionView SelectedArticles { get; private set; }

        #endregion

        #region Constructors

        public AddArticleWithReferencesPageViewModel(WizardViewModel parent)
            : base(parent)
        {
            IEnumerable<Article> articles = CentralRepository.Instance.GetArticles();
            ArticlesToChooseFrom = CollectionViewSource.GetDefaultView(articles);

            _selectedArticles = new ObservableCollection<Article>();
            SelectedArticles = CollectionViewSource.GetDefaultView(_selectedArticles);
        }

        public void AddReference(Article article)
        {
            if (article != null)
                _selectedArticles.Add(article);
        }

        #endregion
    }
}
