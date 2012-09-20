using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ReferenceArchiver.Model
{
    class DummyCentralRepository : CentralRepository
    {
        private List<Institution> _institutions;
        private List<Publisher> _publishers;
        private List<ResearchJournal> _journals;
        private List<Issue> _issues;

        public DummyCentralRepository()
        {
            _institutions = new List<Institution>();
            _institutions.Add(new Institution("1", "Politechnika Śląska"));
            _institutions.Add(new Institution("2", "Politechnika Wrocławska"));
            _institutions.Add(new Institution("3", "Politechnika Warszawska"));
            _institutions.Add(new Institution("4", "Uniwersytet Warszawski"));
            _institutions.Add(new Institution("5", "Akademia Górniczo-Hutnicza"));

            _publishers = new List<Publisher>();
            _publishers.Add(new Publisher("1", "1", "Wydawnictwo Politechniki Śląskiej"));
            _publishers.Add(new Publisher("2", "1", "Wydawnictwo Politechniki Wrocławskiej"));
            _publishers.Add(new Publisher("3", "1", "Wydawnictwo Politechniki Warszawskiej"));
            _publishers.Add(new Publisher("4", "1", "Wydawnictwo Uniwersytetu Warszawskiego"));
            _publishers.Add(new Publisher("5", "1", "Wydawnictwo AGH"));

            _journals = new List<ResearchJournal>();
            int issn = 2000;
            foreach (var publisher in _publishers)
            {
                int idWithingPublisher = 1;
                foreach (var title in new string[] { "Informatyka", "Fizyka molekularna", "Ekologia" })
                {
                    _journals.Add(new ResearchJournal()
                    {
                        InstitutionId = publisher.InstitutionId,
                        PublisherId = publisher.IdWithinInstitution,
                        IdWithinPublisher = (idWithingPublisher++).ToString(),
                        ISSN = string.Format("2000-{0}", issn++),
                        Title = string.Format("{0} - {1}", publisher.Title, title)
                    });
                }
            }

            _issues = new List<Issue>();
            foreach (var journal in _journals)
            {
                for (int i = 0; i < 10; i++)
                {
                    _issues.Add(new Issue(journal)
                    {
                        IdWithinJournal = i,
                        NumberWithinJournal = i,
                        NumberWithinPublisher = _issues.Where(issue => issue.InstitutionId == journal.InstitutionId 
                            && issue.PublisherId == journal.PublisherId).Count() + 1,
                        Title = string.Format("Numer {0}", i),
                        YearOfPublication = 2000 + i,
                    });
                }
            }
        }

        public override IEnumerable<Institution> GetInstitutions()
        {
            return _institutions;
        }

        public override IEnumerable<Publisher> GetPublishers()
        {
            return _publishers;
        }

        public override IEnumerable<Publisher> GetPublishersForInstitution(Institution institution)
        {
            if (institution.Id == null)
                yield break;

            foreach (var publisher in _publishers)
            {
                if (publisher.InstitutionId == institution.Id)
                    yield return publisher;
            }
        }

        public override IEnumerable<ResearchJournal> GetJournalsForPublisher(Publisher publisher)
        {
            if (publisher == null || publisher.IdWithinInstitution == null)
                return Enumerable.Empty<ResearchJournal>();

            return _journals.Where(journal => journal.PublisherId == publisher.IdWithinInstitution
                && journal.InstitutionId == publisher.InstitutionId);
        }

        public override IEnumerable<Issue> GetIssuesForJournal(ResearchJournal journal)
        {
            if (journal == null || journal.IdWithinPublisher == null)
                return Enumerable.Empty<Issue>();

            return _issues.Where(issue => issue.InstitutionId == journal.InstitutionId 
                && issue.PublisherId == journal.PublisherId && issue.JournalId == journal.IdWithinPublisher);
        }

        public override Issue GetIssueByNumberWithinJournal(ResearchJournal journal, int number)
        {
            return _issues.Where(issue => issue.InstitutionId == journal.InstitutionId 
                && issue.PublisherId == journal.PublisherId 
                && issue.JournalId == journal.IdWithinPublisher 
                && issue.NumberWithinJournal == number).FirstOrDefault();
        }

        public override Issue GetIssueByNumberWithinPublisher(Publisher publisher, int number)
        {
            return _issues.Where(issue => issue.InstitutionId == publisher.InstitutionId
                && issue.PublisherId == publisher.IdWithinInstitution
                && issue.NumberWithinPublisher == number).FirstOrDefault();
        }

        public override IEnumerable<Article> GetArticlesFromIssue(Issue issue)
        {
            throw new NotImplementedException();
        }

        public override bool SaveInstitution(Institution institution)
        {
            throw new NotImplementedException();
        }

        public override bool SavePublisher(Publisher publisher)
        {
            throw new NotImplementedException();
        }

        public override bool SaveResearchJournal(ResearchJournal journal)
        {
            throw new NotImplementedException();
        }

        public override bool SaveIssue(Issue issue)
        {
            throw new NotImplementedException();
        }

        public override bool SaveArticle(Article article)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Category> GetArticleCategories(Article article)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Article> GetArticlesByCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public override AlienPublisher GetAlienPublisherForArticle(Article article)
        {
            throw new NotImplementedException();
        }

        public override Category GetAboveCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Author> GetArticleAuthors(Article article)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Article> GetArticlesByKeyword(Keyword keyword)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Article> GetAnnotationsForArticle(Article article, int number = -1)
        {
            throw new NotImplementedException();
        }

        public override bool SaveAlienPublisher(AlienPublisher alien_publisher)
        {
            throw new NotImplementedException();
        }

        public override bool SaveCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public override bool SaveAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public override bool SaveKeyword(Keyword keyword)
        {
            throw new NotImplementedException();
        }

        public override bool AddCategoryToArticle(Article article, Category category)
        {
            throw new NotImplementedException();
        }

        public override bool AddAnnotationToArticle(Article article, Article annotation, int annotation_number)
        {
            throw new NotImplementedException();
        }

        public override bool AddAuthorshipToArticle(Article article, Author author, int authorship_number)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteInstitution(Institution institution)
        {
            throw new NotImplementedException();
        }

        public override bool DeletePublisher(Publisher publisher)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteResearchJournal(ResearchJournal journal)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteIssue(Issue issue)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteArticle(Article article)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAlienPublisher(AlienPublisher alien_publisher)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteKeyword(Keyword keyword)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAuthorshipFromArticle(Article article, Author author, int authorship_number)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAnnotationFromArticle(Article article, Article annotation, int annotation_number)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteCategoryFromArticle(Article article, Category category, bool delete_all = false)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateInstitution(Institution institution)
        {
            throw new NotImplementedException();
        }

        public override bool UpdatePublisher(Publisher publisher)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateResearchJournal(ResearchJournal journal)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateIssue(Issue issue)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateArticle(Article article)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateAlienPublisher(AlienPublisher alien_publisher)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public override bool UpdateKeyword(Keyword keyword)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Author> GetAuthors()
        {
            throw new NotImplementedException();
        }

        public override Institution GetInstitutionByName(string name)
        {
            throw new NotImplementedException();
        }

        public override Publisher GetPublisherByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
