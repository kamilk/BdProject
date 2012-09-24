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
        public Xml xmlmanager;
        private ArchiverCentralRepository db;

        Marcxml()
        {
            db = new ArchiverCentralRepository();
            xmlmanager = new Xml();
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
            //dane tymczasowe czekajace na metody bd
            xmlmanager.AddArticle(article);
            Issue temp = new Issue();
            xmlmanager.AddIssue(temp);
            ResearchJournal temp2 = new ResearchJournal();
            xmlmanager.AddResearchJournal(temp2);
            Publisher temp3 = new Publisher();
            xmlmanager.AddPublisher(temp3);
            Institution temp4 = new Institution();
            xmlmanager.AddInstitution(temp4);
            this.ProcessBibliography(article);
            this.ProcessAuthors(article);
        }

        private void ProcessAuthors(Article article)
        {
            IEnumerable<Author> authorlist = null;
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

    }
}
