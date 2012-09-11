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
    /// Interaction logic for ChooseResearchJournalView.xaml
    /// </summary>
    public partial class ChooseResearchJournalView : UserControl
    {
        private ChooseResearchJournalPageViewModel viewModel;

        public ChooseResearchJournalView()
        {
            InitializeComponent();
        }

        private void journalSearchString_TextChanged(object sender, TextChangedEventArgs e)
        {
            journalSearchString.UpdateTextBinding();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Publisher publisher = viewModel.WizardViewModel.SelectedPublisher;
            if (titleBox.Text != "")
            {
                ResearchJournal researchJournal = new ResearchJournal(publisher.InstitutionId, publisher.IdWithinInstitution, null, titleBox.Text, issnBox.Text);
                ArchiverCentralRepository.Instance.SaveResearchJournal(researchJournal);
                viewModel.AddAndSelectJournal(researchJournal);
            }
            else
            {
                MessageBox.Show("Nie podano tytułu!");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = (ChooseResearchJournalPageViewModel)DataContext;
        }
    }
}
