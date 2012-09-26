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
using ReferenceArchiver.ViewModel;

namespace ReferenceArchiver.View
{
    /// <summary>
    /// Interaction logic for AddArticleWithoutReferencesView.xaml
    /// </summary>
    public partial class AddArticleView : UserControl
    {
        private AddArticleViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddArticleView" /> class.
        /// </summary>
        public AddArticleView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = (AddArticleViewModel)DataContext;
        }

        private void addAuthorshipButton_Click(object sender, RoutedEventArgs e)
        {
            object author = authorAutoCompleteBox.SelectedItem;
            object affiliation = affiliationAutoCompleteBox.SelectedItem;
            _viewModel.AddAuthorship(author, affiliation);
        }

        private void createAuthorButton_Click(object sender, RoutedEventArgs e)
        {
            var newAuthorWindow = new NewAuthorWindow();
            if (newAuthorWindow.ShowDialog() == true)
            {
                _viewModel.AddAuthor(newAuthorWindow.FirstName, newAuthorWindow.MiddleName, newAuthorWindow.LastName, newAuthorWindow.Nationality);
            }
        }
    }
}
