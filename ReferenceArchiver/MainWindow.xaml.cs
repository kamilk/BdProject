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

namespace BdGui2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WizardViewModel _wizardViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _wizardViewModel = new WizardViewModel();
            
            this.DataContext = _wizardViewModel;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _wizardViewModel.PageManager.NavigateForward();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            _wizardViewModel.PageManager.NavigateBackward();
        }
    }
}
