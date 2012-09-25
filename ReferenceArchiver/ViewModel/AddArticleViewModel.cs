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
    class AddArticleViewModel : WizardPageViewModelBase
    {
        #region Fields

        private ObservableCollection<AuthorshipData> _authorships = new ObservableCollection<AuthorshipData>();
        private ObservableCollection<Author> _authorsToChooseFrom = new ObservableCollection<Author>();

        #endregion

        #region Properties

        public override string Title { get { return "Podaj dane artykułu"; } }
        public string ArticleTitle { get; set; }
        public string ArticlePolishTitle { get; set; }
        public ICollectionView Languages { get; private set; }
        public ICollectionView AuthorsToChooseFrom { get; private set; }
        public ICollectionView InstitutionsToChooseFrom { get; private set; }
        public ICollectionView Authorships { get; private set; }
        public ICommand MoveAuthorshipUpCommand { get; private set; }
        public ICommand MoveAuthorshipDownCommand { get; private set; }
        public ICommand RemoveAuthorshipCommand { get; private set; }

        #endregion

        #region Constructors

        public AddArticleViewModel(WizardViewModel parent)
            : base(parent)
        {
            Languages = CollectionViewSource.GetDefaultView(CentralRepository.Instance.GetLanguages());
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

        public Article GetArticle()
        {
            return new Article()
            {
                Title = ArticleTitle,
                TitlePl = ArticlePolishTitle,
                Lang = ((Language)Languages.CurrentItem).CountryCode
            };
        }

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

        public void AddAuthorship(object author, object affiliation)
        {
            var authorTyped = author as Author;
            var affiliationTyped = affiliation as Institution;
            if (author != null && affiliation != null)
                _authorships.Add(new AuthorshipData() { Author = authorTyped, Affiliation = affiliationTyped });
        }

        public void AddAuthor(string firstName, string middleName, string lastName, Country nationality)
        {
            var author = new Author(-1, lastName, firstName, middleName, nationality.Code);
            CentralRepository.Instance.SaveAuthor(author);
            _authorsToChooseFrom.Add(author);
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
