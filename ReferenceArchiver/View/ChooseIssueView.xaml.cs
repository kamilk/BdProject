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
        
        //TYPE??

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
        }

        private void FillIssueData(Issue issue)
        {
            if (issue == null)
            {
                numberWithinJournalBox.Text = "";
                numberWithinPublisherBox.Text = "";

                editButton.IsEnabled = true;
                cancelButton.IsEnabled = true;

                yearBox.IsEnabled = true;
                titleBox.IsEnabled = true;
                typeCombo.IsEnabled = true;
                typeNumberBox.IsEnabled = true;

            }
            else
            {
                numberWithinJournalBox.Text = issue.NumberWithinJournal.ToString();
                numberWithinPublisherBox.Text = issue.NumberWithinPublisher.ToString();
                yearBox.Text = issue.YearOfPublication != null ? issue.YearOfPublication.ToString() : "";
                titleBox.Text = issue.Title != null ? issue.Title : "";
                _viewModel.IssueTypes.MoveCurrentTo(issue.Type);
                typeNumberBox.Text = issue.TypeNumber != null ? issue.TypeNumber.ToString() : "";

                _title = titleBox.Text;
                //TYPE??
                _typeNumber = typeNumberBox.Text;
                _year = yearBox.Text;

                editButton.IsEnabled = true;
                cancelButton.IsEnabled = true;
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            numberWithinJournalBox.IsEnabled = false;
            numberWithinPublisherBox.IsEnabled = false;

            yearBox.IsEnabled = true;
            titleBox.IsEnabled = true;
            typeCombo.IsEnabled = true;
            typeNumberBox.IsEnabled = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            yearBox.Text = _year;
            titleBox.Text = _title;
            //TYPE??
            typeNumberBox.Text = _typeNumber;
        }
    }
}
