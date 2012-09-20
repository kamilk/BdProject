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
    public partial class AddArticleWithoutReferencesView : UserControl
    {
        private ObservableCollection<AuthorshipData> _authorships = new ObservableCollection<AuthorshipData>();

        public AddArticleWithoutReferencesView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            authorshipDataGrid.ItemsSource = _authorships;

            authorAutoCompleteBox.ItemsSource = CentralRepository.Instance.GetAuthors();
            affiliationAutoCompleteBox.ItemsSource = CentralRepository.Instance.GetInstitutions();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Author author = authorAutoCompleteBox.SelectedItem as Author;
            Institution affiliation = affiliationAutoCompleteBox.SelectedItem as Institution;
            if (author != null && affiliation != null)
                _authorships.Add(new AuthorshipData() { Author = author, Affiliation = affiliation });
        }
    }
}
