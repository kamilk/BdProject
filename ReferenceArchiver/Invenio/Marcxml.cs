using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.Invenio
{
    /// <summary>
    /// Class used to create Marcxml 
    /// </summary>
    class Marcxml
    {
        private Xml xmlmanager;
        private ArchiverCentralRepository db;

        Marcxml()
        {
            db = new ArchiverCentralRepository();
            xmlmanager = new Xml();
        }

        public void CreateMarcxml(string path)
        {
            this.xmlmanager.SaveXml(path);
        }

        public void ProcessArticle(int articleid)
        {
            if (db.IsConnected == true)
            {
                Article temparticle = db.GetArticleById(articleid);
                ProcessData(temparticle);
            }
            else
            {
                MessageBox.Show("fail");
            }
        }

        public void ProcessArticle(Article article)
        {
            if (db.IsConnected == true)
            {
                ProcessData(article);
            }
            else
            {
                MessageBox.Show("fail");
            }
        }

        private void ProcessData(Article article)
        {
            xmlmanager.AddArticle(article);

            Publisher publisher = this.ChoosePublisher(article);
            xmlmanager.AddPublisher(publisher);
            
            Institution institution = this.ChooseInstitution(article);
            xmlmanager.AddInstitution(institution);

            ResearchJournal journal = this.ChooseJournal(article, publisher);
            xmlmanager.AddResearchJournal(journal);

            Issue issue = this.ChooseIssue(article,journal);
            xmlmanager.AddIssue(issue);

            this.ProcessCategory(article);
            this.ProcessBibliography(article);
            this.ProcessAuthors(article);
        }

        private Issue ChooseIssue(Article article, ResearchJournal journal)
        {
            IEnumerable<Issue> issues = db.GetIssuesForJournal(journal);
            foreach (var issue in issues)
            {
                if (issue.IdWithinJournal == article.IssueId) return issue;
            }
            return null;
        }

        private ResearchJournal ChooseJournal(Article article,Publisher publisher)
        {
            IEnumerable<ResearchJournal> jaournals = db.GetJournalsForPublisher(publisher);
            foreach (var journal in jaournals)
            {
                if (journal.IdWithinPublisher == article.JournalId) return journal ;
            }
            return null;
        }

        private Publisher ChoosePublisher(Article article)
        {
            IEnumerable<Publisher> publishers = db.GetPublishers();
            foreach (var publisher in publishers)
            {
                if (publisher.IdWithinInstitution == article.PublisherId) return publisher;
            }
            return null;
        }

        private Institution ChooseInstitution(Article article)
        {
            IEnumerable<Institution> institutions = db.GetInstitutions();
            foreach (var institution in institutions)
            {
                if (institution.Id == article.InstitutionId) return institution;
            }
            return null;
        }

        private void ProcessAuthors(Article article)
        {
            IEnumerable<Author> authorlist = db.GetArticleAuthors(article);
            foreach (var item in authorlist)
            {
                this.xmlmanager.AddAuthor(item);
            }
        }

        private void ProcessBibliography(Article article)
        {
            IEnumerable<Annotation> bibliolist = db.GetAnnotationsForArticle(article);
            foreach (var item in bibliolist)
            {
                Article biblioarticle = db.GetArticleById(item.Id_Art);
                this.xmlmanager.AddBiblioArticle(biblioarticle);
            }
        }

        private void ProcessCategory(Article article)
        {
            IEnumerable<Category> categorylist = db.GetArticleCategories(article);
            foreach (var item in categorylist)
            {
                this.xmlmanager.AddCategory(item);
            }
        }

    }
}
