using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReferenceArchiver.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ReferenceArchiver.View
{
    /// <summary>
    /// Interaction logic for AddArticleWithoutReferencesView.xaml
    /// </summary>
    public partial class AddArticleView : UserControl
    {
        private ObservableCollection<AuthorshipData> _authorships = new ObservableCollection<AuthorshipData>();

        public AddArticleView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            authorshipDataGrid.ItemsSource = _authorships;

            authorAutoCompleteBox.ItemsSource = CentralRepository.Instance.GetAuthors();
            affiliationAutoCompleteBox.ItemsSource = CentralRepository.Instance.GetInstitutions();

            langaugesComboBox.ItemsSource = CentralRepository.Instance.GetLanguages();
            langaugesComboBox.SelectedIndex = 0;
        }

        private void addAuthorshipButton_Click(object sender, RoutedEventArgs e)
        {
            Author author = authorAutoCompleteBox.SelectedItem as Author;
            Institution affiliation = affiliationAutoCompleteBox.SelectedItem as Institution;
            if (author != null && affiliation != null)
                _authorships.Add(new AuthorshipData() { Author = author, Affiliation = affiliation });
        }

        private void removeAuthorshipButton_Click(object sender, RoutedEventArgs e)
        {
            var authorship = authorshipDataGrid.SelectedItem as AuthorshipData;
            if (authorship != null)
                _authorships.Remove(authorship);
        }

        private void moveAuthorshipUpButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = authorshipDataGrid.SelectedIndex;
            if (selectedIndex <= 0)
                return;
            _authorships.Move(selectedIndex, selectedIndex - 1);
        }

        private void moveAuthorshipDownButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = authorshipDataGrid.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= _authorships.Count - 1)
                return;
            _authorships.Move(selectedIndex, selectedIndex + 1);
        }
    }
}
