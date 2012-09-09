using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    abstract class CentralRepository
    {
        // Get
        public abstract IEnumerable<Institution> GetInstitutions();
        public abstract IEnumerable<Publisher> GetPublishers();
        public abstract IEnumerable<Publisher> GetPublishersForInstitution(Institution institution);
        public abstract IEnumerable<ResearchJournal> GetJournalsForPublisher(Publisher publisher);
        public abstract IEnumerable<Issue> GetIssuesForJournal(ResearchJournal journal);
        public abstract Issue GetIssueByNumberWithinJournal(ResearchJournal journal, int number);
        public abstract Issue GetIssueByNumberWithinPublisher(Publisher publisher, int number);
        public abstract IEnumerable<Article> GetArticlesFromIssue(Issue issue);
        public abstract IEnumerable<Category> GetArticleCategories(Article article);
        public abstract IEnumerable<Article> GetArticlesByCategory(Category category);
        // RETURNS NULL WHEN ALIENID IS NULL
        public abstract AlienPublisher GetAlienPublisherForArticle(Article article);
        // RETURNS NULL WHEN ABOVECATEGORY IS NULL
        public abstract Category GetAboveCategory(Category category);
        public abstract IEnumerable<Author> GetArticleAuthors(Article article);
        // Only Key field required to be filled in the keyword variable
        public abstract IEnumerable<Article> GetArticlesByKeyword(Keyword keyword);
        // When number is >= 0 returns only 1 annotation - for given number, if it exists
        // Otherwise the list is empty
        public abstract IEnumerable<Article> GetAnnotationsForArticle(Article article, int number = -1);


        // Save
        public abstract bool SaveInstitution(Institution institution);
        public abstract bool SavePublisher(Publisher publisher);
        public abstract bool SaveResearchJournal(ResearchJournal journal);
        public abstract bool SaveIssue(Issue issue);
        public abstract bool SaveArticle(Article article);
        public abstract bool SaveAlienPublisher(AlienPublisher alien_publisher);
        public abstract bool SaveCategory(Category category);
        public abstract bool SaveAuthor(Author author);
        public abstract bool SaveKeyword(Keyword keyword);
        // Add, basically the same as Save, but you can Add many categories/authorships/annotations to the same article
        // So to point this out I used different word
        public abstract bool AddCategoryToArticle(Article article, Category category);
        public abstract bool AddAnnotationToArticle(Article article, Article annotation, int annotation_number);
        public abstract bool AddAuthorshipToArticle(Article article, Author author, int authorship_number);


        // Delete
        // REQUIRES ID FIELDS TO BE FILLED CORRECTLY, USE GET METHOD BEFORE UPDATE TO RECEIVE ID'S
        public abstract bool DeleteInstitution(Institution institution);
        public abstract bool DeletePublisher(Publisher publisher);
        public abstract bool DeleteResearchJournal(ResearchJournal journal);
        public abstract bool DeleteIssue(Issue issue);
        public abstract bool DeleteArticle(Article article);
        public abstract bool DeleteAlienPublisher(AlienPublisher alien_publisher);
        public abstract bool DeleteCategory(Category category);
        public abstract bool DeleteAuthor(Author author);
        // THIS ONE REQUIRES THE KEY FIELD TO BE FILLED
        // WSZYSTKIE SLOWA KLUCZE PISZEMY MALYMI
        public abstract bool DeleteKeyword(Keyword keyword);
        // When numbers are less than 0 (preferably -1), deletes ALL authorships/annotations/categories from article
        public abstract bool DeleteAuthorshipFromArticle(Article article, Author author, int authorship_number);
        public abstract bool DeleteAnnotationFromArticle(Article article, Article annotation, int annotation_number);
        public abstract bool DeleteCategoryFromArticle(Article article, Category category, bool delete_all = false);


        // Update
        // REQUIRES ID FIELDS TO BE FILLED CORRECTLY, USE GET METHOD BEFORE UPDATE TO RECEIVE ID'S
        // YOU CANNOT UPDATE ANY FIELDS THAT ARE WITHIN ANY OF THE KEYS FOR THE TABLE
        // TO DO THAT DELETE AND CREATE NEW ONE
        public abstract bool UpdateInstitution(Institution institution);
        public abstract bool UpdatePublisher(Publisher publisher);
        public abstract bool UpdateResearchJournal(ResearchJournal journal);
        public abstract bool UpdateIssue(Issue issue);
        public abstract bool UpdateArticle(Article article);
        public abstract bool UpdateAlienPublisher(AlienPublisher alien_publisher);
        public abstract bool UpdateCategory(Category category);
        public abstract bool UpdateAuthor(Author author);
        public abstract bool UpdateKeyword(Keyword keyword);

        private static CentralRepository _instance;
        public static CentralRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ArchiverCentralRepository();
                return _instance;
            }
        }
    }
}
