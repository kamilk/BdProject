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
    /// Interaction logic for ChoosePublisherForFootnoteView.xaml
    /// </summary>
    public partial class ChoosePublisherForFootnoteView : UserControl
    {
        private ChoosePublisherForReferencePageViewModel _viewModel;

        public ChoosePublisherForFootnoteView()
        {

            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as ChoosePublisherForReferencePageViewModel;
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.SwitchToStandardPublisher();
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
                _viewModel.SwitchToExternalPublisher();
        }
    }
}
