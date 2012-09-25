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
        public abstract Institution GetInstitutionByName(string name);
        public abstract IEnumerable<Publisher> GetPublishers();
        public abstract Publisher GetPublisherByName(string name, string id_inst);
        public abstract IEnumerable<Publisher> GetPublishersForInstitution(Institution institution);
        public abstract IEnumerable<ResearchJournal> GetJournalsForPublisher(Publisher publisher);
        public abstract IEnumerable<Issue> GetIssuesForJournal(ResearchJournal journal);
        public abstract Issue GetIssueByNumberWithinJournal(ResearchJournal journal, int number);
        public abstract Issue GetIssueByNumberWithinPublisher(Publisher publisher, int number);
        public abstract IEnumerable<Article> GetArticles();
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
        public abstract IEnumerable<Annotation> GetAnnotationsForArticle(Article article, int number = -1);
        public abstract IEnumerable<Author> GetAuthors();
        // Returns null on id < 0
        public abstract Article GetArticleById(int id);
        // Assuming author is an actual author of the article AND that author is in the AUTORSTWO table only once for the article.
        // Having one author twice for the same article is a mistake.
        // Returns 0 when Author is not the article author in the database.
        // Returns -1 on error (when author is twice recorded for the same article, or when 2 articles have the same ID)
        public abstract int GetAuthorAfiliationForArticle(Article article, Author author);
        public abstract IEnumerable<Country> GetCountries();
        // Gives the country code for language (from Article table)
        public abstract Country GetCountryForLanguage(string lang);
        // Returns null when no country of given code, or FIRST AND ONLY FIRST occurence in database for the code. 
        // Make sure it's not doubled
        public abstract Country GetCountryByCode(string code);
        // The exact same comment as for GetCountryByCode above.
        public abstract Country GetCountryByName(string name);
        // Null on errors. Takes the first matching language for country, assuming 1 country has 1 language only.
        public abstract string GetLanguageForCountry(Country country);
        public abstract IEnumerable<Language> GetLanguages();


        // Save
        public abstract string SaveInstitution(Institution institution);
        public abstract string SavePublisher(Publisher publisher);
        public abstract bool SaveResearchJournal(ResearchJournal journal);
        public abstract bool SaveIssue(Issue issue);
        public abstract bool SaveArticle(Article article);
        public abstract bool SaveAlienPublisher(AlienPublisher alien_publisher);
        public abstract bool SaveCategory(Category category);
        public abstract bool SaveAuthor(Author author);
        public abstract bool SaveKeyword(Keyword keyword);
        public abstract bool SaveCountry(Country country);
        // Add, basically the same as Save, but you can Add many categories/authorships/annotations to the same article
        // So to point this out I used different word
        public abstract bool AddCategoryToArticle(Article article, Category category);
        public abstract bool AddAnnotationToArticle(Article article, Article annotation, int annotation_number);
        public abstract bool AddAuthorshipToArticle(Article article, Author author, int authorship_number);
        public abstract bool AddLanguageToCountry(Country country, string lang);

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
        public abstract bool DeleteCountry(Country country);
        // THIS ONE REQUIRES THE KEY FIELD TO BE FILLED
        // WSZYSTKIE SLOWA KLUCZE PISZEMY MALYMI
        public abstract bool DeleteKeyword(Keyword keyword);
        // When numbers are less than 0 (preferably -1), deletes ALL authorships/annotations/categories from article
        public abstract bool DeleteAuthorshipFromArticle(Article article, Author author, int authorship_number);
        public abstract bool DeleteAnnotationFromArticle(Article article, Article annotation, int annotation_number);
        public abstract bool DeleteCategoryFromArticle(Article article, Category category, bool delete_all = false);
        public abstract bool DeleteLanguageFromCountry(Country country);

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
        public abstract bool UpdateCountry(Country country);

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
