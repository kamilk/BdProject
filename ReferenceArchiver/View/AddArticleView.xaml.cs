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

namespace ReferenceArchiver.View
{
    /// <summary>
    /// Interaction logic for AddArticleView.xaml
    /// </summary>
    public partial class AddArticleView : UserControl
    {
        AddArticlePageViewModel _viewModel;

        public AddArticleView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = this.DataContext as AddArticlePageViewModel;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.WizardViewModel.NavigateToAddingReferenceArticle();
        }
    }
}
