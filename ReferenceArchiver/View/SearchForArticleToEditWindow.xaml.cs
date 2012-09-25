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
using System.Windows.Shapes;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.View
{
    /// <summary>
    /// Interaction logic for SearchForArticleToEditWindow.xaml
    /// </summary>
    public partial class SearchForArticleToEditWindow : Window
    {
        public Article SelectedArticle
        { get { return (Article)articlesDataGrid.SelectedItem; } }

        public SearchForArticleToEditWindow()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            articlesDataGrid.ItemsSource = CentralRepository.Instance.GetArticles();
            articlesDataGrid.SelectedIndex = 0;
        }
    }
}
