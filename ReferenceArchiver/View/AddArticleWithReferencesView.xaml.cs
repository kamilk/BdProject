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
using ReferenceArchiver.ViewModel;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.View
{
    /// <summary>
    /// Interaction logic for AddArticleView.xaml
    /// </summary>
    public partial class AddArticleWithReferencesView : UserControl
    {
        private AddArticleWithReferencesPageViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddArticleWithReferencesView" /> class.
        /// </summary>
        public AddArticleWithReferencesView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = (AddArticleWithReferencesPageViewModel)DataContext;
        }

        private void addReferenceButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddReference(articlesAutoCompleteBox.SelectedItem as Article);
        }
    }
}
