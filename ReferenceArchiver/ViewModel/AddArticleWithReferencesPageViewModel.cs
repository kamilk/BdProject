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

        public AddArticleViewModel AddArticleDataContext { get; private set; }
        public ICollectionView ArticlesToChooseFrom { get; private set; }
        public ICollectionView SelectedArticles { get; private set; }
        public ICommand MoveReferenceUpCommand { get; private set; }
        public ICommand MoveReferenceDownCommand { get; private set; }
        public ICommand RemoveReferenceCommand { get; private set; }

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
        }

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
