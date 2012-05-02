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
using BdGui2.ViewModel;

namespace BdGui2.View
{
    /// <summary>
    /// Interaction logic for ChooseInstitiutionView.xaml
    /// </summary>
    public partial class ChooseInstitiutionView : UserControl
    {
        ChooseInstitiutionAndPublisherPageViewModel dataContext;

        public ChooseInstitiutionView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataContext = DataContext as ChooseInstitiutionAndPublisherPageViewModel;
        }

        private void institutionNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dataContext == null)
                return;

            dataContext.Institutions.Filter(institutionNameTextBox.Text);
        }
    }
}
