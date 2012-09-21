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
        #region Fields

        private ChooseInstitiutionAndPublisherPageViewModel viewModel;
        private WizardViewModel wizardViewModel;
        private Institution choosenInstitution;
        private Publisher choosenPublisher;

        #endregion

        #region Constructor

        public ChooseInstitiutionView()
        {
            InitializeComponent();
            buttonAddPublisher.IsEnabled = false;
        }

        #endregion

        #region Events

        private void institutionNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            institutionNameTextBox.UpdateTextBinding();            
        }

        private void publisherNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            publisherNameTextBox.UpdateTextBinding();
        }

        private void buttonAddInstitution_Click(object sender, RoutedEventArgs e)
        {
            saveInstitution();
        }

        private void buttonAddPublisher_Click(object sender, RoutedEventArgs e)
        {
            savePublisher();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = (ChooseInstitiutionAndPublisherPageViewModel)DataContext;
            institutionNameTextBox.Focus();
        }

        private void institutionNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    saveInstitution();
                }
                else
                {
                    selectFirstInstitution();
                }
            }
        }

        private void institutionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (institutionsListBox.SelectedItem != null)
            {
                onSelectInstitution();
            }
            else
            {
                choosenInstitution = null;
                buttonAddPublisher.IsEnabled = false;
            }
        }

        #endregion

        #region Other Methods

        private void selectFirstInstitution()
        {
            institutionsListBox.SelectedIndex = 0;
        }

        private bool saveInstitution()
        {
            bool result = false;
            if (institutionNameTextBox.Text.Length != 0)
            {
                Institution inst = new Institution("", institutionNameTextBox.Text);
                if (CentralRepository.Instance.SaveInstitution(inst))
                {
                    inst = CentralRepository.Instance.GetInstitutionByName(inst.Name);
                    viewModel.AddAndSelectInstitution(inst);
                    result = true;
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
            return result;
        }

        private bool savePublisher()
        {
            bool result = false;
            if (publisherNameTextBox.Text.Length != 0 && choosenInstitution != null)
            {
                if (CentralRepository.Instance.SavePublisher(new Publisher(choosenInstitution.Id.ToString(),"", publisherNameTextBox.Text)))
                {
                    result = true;
                }
                else
                {
                    MessageBox.Show("Przy dodawaniu nowego wydawnictwa do bazy wystąpił błąd!");
                }
            }
            else
            {
                MessageBox.Show("Aby dodać nowe wydawnictwo, należy podać jej nazwę!");
            }
            return result;
        }

        private void onSelectInstitution()
        {
            institutionNameTextBox.Text = institutionsListBox.SelectedItem.ToString();
            choosenInstitution = (Institution)viewModel.Institutions.CurrentItem;
            publisherNameTextBox.Focus();
            buttonAddPublisher.IsEnabled = true;
        }

        #endregion


    }
}
