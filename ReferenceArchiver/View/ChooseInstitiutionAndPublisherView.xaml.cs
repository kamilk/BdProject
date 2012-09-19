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
    /// Interaction logic for ChooseInstitiutionView.xaml
    /// </summary>
    public partial class ChooseInstitiutionView : UserControl
    {
        public ChooseInstitiutionView()
        {
            InitializeComponent();
        }

        private void institutionNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            institutionNameTextBox.UpdateTextBinding();
            selectFirstInstitution();
        }

        private void publisherNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            publisherNameTextBox.UpdateTextBinding();
        }

        private void buttonAddInstitution_Click(object sender, RoutedEventArgs e)
        {
            if (institutionNameTextBox.Text.Length != 0)
            {
                if (CentralRepository.Instance.SaveInstitution(new Institution("", institutionNameTextBox.Text)))
                {

                }
                else
                {
                    MessageBox.Show("Przy dodawaniu nowej instytucji do bazy wystąpił błąd!");
                }
            }
            else
            {
                MessageBox.Show("Aby dodać nową instytucję, należy podać jej nazwę!");
            }
        }

        private void selectFirstInstitution()
        {
            institutionsListBox.SelectedIndex = 0;
        }

        private void buttonAddPublisher_Click(object sender, RoutedEventArgs e)
        {
            if (institutionNameTextBox.Text.Length != 0)
            {
                if (CentralRepository.Instance.SavePublisher(new Publisher(institutionsListBox.SelectedItem.,"", institutionNameTextBox.Text)))
                {

                }
                else
                {
                    MessageBox.Show("Przy dodawaniu nowej instytucji do bazy wystąpił błąd!");
                }
            }
            else
            {
                MessageBox.Show("Aby dodać nową instytucję, należy podać jej nazwę!");
            }
        }
    }
}
