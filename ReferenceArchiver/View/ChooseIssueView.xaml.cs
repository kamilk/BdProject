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
    /// Interaction logic for ChooseIssueView.xaml
    /// </summary>
    public partial class ChooseIssueView : UserControl
    {
        ChooseIssuePageViewModel _viewModel;

        String _year = "";
        String _title = "";
        String _typeNumber = "";
        //TYPE
        IssueType _type;
        int _publisherNumber = 0;
        int _seriesNumber = 0;

        bool _issueFound = false;

        public ChooseIssueView()
        {
            InitializeComponent();
        }

        private void searchJournal_Click(object sender, RoutedEventArgs e)
        {
            Issue issue = null;
            int numberWithinJournal;
            if (int.TryParse(numberWithinJournalBox.Text, out numberWithinJournal))
            {
                issue = CentralRepository.Instance.GetIssueByNumberWithinJournal(
                     _viewModel.WizardViewModel.SelectedJournal, numberWithinJournal);
            }
            FillIssueData(issue);
            _viewModel.SelectedIssue = issue;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = (ChooseIssuePageViewModel)DataContext;
            Issue issue = _viewModel.SelectedIssue;
            if (issue != null && issue.IdWithinJournal.HasValue && issue.IdWithinJournal >= 0)
                FillIssueData(issue);
        }

        private void FillIssueData(Issue issue)
        {
            if (issue == null)
            {
                _issueFound = false;
                numberWithinJournalBox.IsEnabled = false;
                numberWithinPublisherBox.IsEnabled = false;

                editButton.IsEnabled = true;
                cancelButton.IsEnabled = true;

                yearBox.IsEnabled = true;
                titleBox.IsEnabled = true;
                typeCombo.IsEnabled = true;
                typeNumberBox.IsEnabled = true;

                if (numberWithinJournalBox.Text != "")
                {
                    _seriesNumber = int.Parse(numberWithinJournalBox.Text);
                }
                if (numberWithinPublisherBox.Text != "")
                {
                    _publisherNumber = int.Parse(numberWithinPublisherBox.Text);
                }

                yearBox.Text = "";
                titleBox.Text = "";
                typeNumberBox.Text = "";

                _title = "";
                _type = default(IssueType);
                _typeNumber = "";
                _year = "";

                editButton.Content = "Dodaj zeszyt";

            }
            else
            {
                _issueFound = true;
                numberWithinJournalBox.Text = issue.NumberWithinJournal.ToString();
                numberWithinPublisherBox.Text = issue.NumberWithinPublisher.ToString();
                yearBox.Text = issue.YearOfPublication != null ? issue.YearOfPublication.ToString() : "";
                titleBox.Text = issue.Title != null ? issue.Title : "";
                _viewModel.IssueTypes.MoveCurrentTo(issue.Type);
                typeNumberBox.Text = issue.TypeNumber != null ? issue.TypeNumber.ToString() : "";

                _title = titleBox.Text;
                //TYPE??
                _type = issue.Type;
                _typeNumber = typeNumberBox.Text;
                _year = yearBox.Text;

                editButton.IsEnabled = true;
                cancelButton.IsEnabled = true;

                editButton.Content = "Edytuj zeszyt";
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (editButton.Content.Equals("Dodaj zeszyt"))
            {
                if (!_issueFound)
                {
                    //dodajemy zeszyt (nie istnieje zeszyt o podanym numerze)
                    Issue issue = new Issue(_viewModel.WizardViewModel.SelectedJournal.InstitutionId,       //id_inst
                                            _viewModel.WizardViewModel.SelectedJournal.PublisherId,         //id_wyd
                                            _viewModel.WizardViewModel.SelectedJournal.IdWithinPublisher,   //id_serie
                                            null,                                                           //id_w_serii
                                            _seriesNumber,                                                  //nr_w_serii
                                            _publisherNumber,                                               //nr_w_wydawnictwie
                                            titleBox.Text,
                                            int.Parse(yearBox.Text),
                                            true,
                                            typeCombo.Text,
                                            typeNumberBox.Text);

                    ArchiverCentralRepository.Instance.SaveIssue(issue);
                }
                else
                {
                    //zmieniamy zeszyt juz istniejacy
                    Issue issue = new Issue(_viewModel.WizardViewModel.SelectedJournal.InstitutionId,       //id_inst
                                            _viewModel.WizardViewModel.SelectedJournal.PublisherId,         //id_wyd
                                            _viewModel.WizardViewModel.SelectedJournal.IdWithinPublisher,   //id_serie
                                            _viewModel.SelectedIssue.IdWithinJournal,                                                           //id_w_serii
                                            _seriesNumber,                                                  //nr_w_serii
                                            _publisherNumber,                                               //nr_w_wydawnictwie
                                            titleBox.Text,
                                            int.Parse(yearBox.Text),
                                            true,
                                            typeCombo.Text,
                                            typeNumberBox.Text);

                    ArchiverCentralRepository.Instance.UpdateIssue(issue);
                }
            }
            else
            {
                //edytujemy wybrany zeszyt
                numberWithinJournalBox.IsEnabled = false;
                numberWithinPublisherBox.IsEnabled = false;

                yearBox.IsEnabled = true;
                titleBox.IsEnabled = true;
                typeCombo.IsEnabled = true;
                typeNumberBox.IsEnabled = true;

                editButton.Content = "Dodaj zeszyt";
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            yearBox.Text = _year;
            titleBox.Text = _title;
            //TYPE??
            switch (_type)
            {
                case IssueType.Normal:
                    typeCombo.SelectedIndex = 0;
                    break;
                case IssueType.Conference:
                    typeCombo.SelectedIndex = 1;
                    break;
                case IssueType.Habilitation:
                    typeCombo.SelectedIndex = 2;
                    break;
                case IssueType.Monograph:
                    typeCombo.SelectedIndex = 3;
                    break;
                case IssueType.Session:
                    typeCombo.SelectedIndex = 4;
                    break;
                case IssueType.Symposium:
                    typeCombo.SelectedIndex = 5;
                    break; 
            }
            typeNumberBox.Text = _typeNumber;
            numberWithinJournalBox.IsEnabled = true;
            numberWithinPublisherBox.IsEnabled = true;

            yearBox.IsEnabled = false;
            titleBox.IsEnabled = false;
            typeCombo.IsEnabled = false;
            typeNumberBox.IsEnabled = false;

            editButton.Content = "Edytuj zeszyt";
        }

        private void searchPublicity_Click(object sender, RoutedEventArgs e)
        {
            Issue issue = null;
            int numberWithinPublisher;
            if (int.TryParse(numberWithinPublisherBox.Text, out numberWithinPublisher))
            {
                issue = CentralRepository.Instance.GetIssueByNumberWithinPublisher(
                     _viewModel.WizardViewModel.SelectedPublisher, numberWithinPublisher);
            }
            FillIssueData(issue);
            _viewModel.SelectedIssue = issue;
        }
    }
}
