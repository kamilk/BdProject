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

namespace ReferenceArchiver.View
{
    /// <summary>
    /// Interaction logic for NewAuthorWindow.xaml
    /// </summary>
    public partial class NewAuthorWindow : Window
    {
        #region Properties

        public string FirstName
        { get { return firstNameTextBox.Text; } }

        public string MiddleName
        { get { return middleNameTextBox.Text; } }

        public string LastName
        { get { return lastNameTextBox.Text; } }

        #endregion

        #region Constructors

        public NewAuthorWindow()
        {
            InitializeComponent();
        }

        #endregion

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
