﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Oracle.DataAccess.Client;

namespace ReferenceArchiver.Model
{
    class ArchiverCentralRepository : CentralRepository
    {
        #region Fields

        private DbConnection m_connection;

        #endregion

        #region Connection methods

        public ArchiverCentralRepository()
        {
            m_connection = new OracleConnection(Properties.Resources.ConnectionString);
            m_connection.Open();
        }

        public bool IsConnected
        {
            get
            {
                return (m_connection.State & ConnectionState.Open) != 0;
            }
        }

        public void Dispose()
        {
            m_connection.Close();
        }

        #endregion

        #region Get methods

        public override IEnumerable<Institution> GetInstitutions()
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT ID, NAZWA FROM filo.INSTYTUCJE";

            List<Institution> result = new List<Institution>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Institution(reader.GetString(0), reader.GetString(1)));
                }
            }

            return result;
        }

        public override IEnumerable<Publisher> GetPublishers()
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT ID_INST, ID, TYTUL FROM filo.WYDAWNICTWA";

            List<Publisher> result = new List<Publisher>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Publisher(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                }  
            }

            return result;
        }

        public override IEnumerable<Publisher> GetPublishersForInstitution(Institution institution)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT ID_INST, ID, TYTUL FROM filo.WYDAWNICTWA WHERE ID_INST = :pId";
            command.Parameters.Add(new OracleParameter("Id", institution.Id));

            List<Publisher> result = new List<Publisher>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Publisher(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                }
            }

            return result;
        }

        public override IEnumerable<ResearchJournal> GetJournalsForPublisher(Publisher publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "SELECT ID_INST, ID_WYD, ID, TYTUL, ISSN " +
                "FROM filo.SERIE WHERE ID_INST = :pId_Inst AND ID_WYD = :pId_Wyd";

            command.Parameters.Add(new OracleParameter("Id_Inst", publisher.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", publisher.IdWithinInstitution));

            List<ResearchJournal> result = new List<ResearchJournal>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new ResearchJournal(reader.GetString(0), reader.GetString(1), reader.GetString(2), 
                                                   reader.GetString(3), reader.GetString(4)));
                }
            }

            return result;
        }

        public override IEnumerable<Issue> GetIssuesForJournal(ResearchJournal journal)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "SELECT ID_INST, ID_WYD, ID_SERIE, ID_W_SERII, NR_W_SERII, NR_W_WYDAWNICTWIE, TYTUL_PL, ROK_WYDANIA, FL_ZWER, TYP, NR_TYP " +
                "FROM filo.ZESZYTY " +
                "WHERE ID_INST = :pId_Inst AND ID_WYD = :pId_Wyd AND ID_SERIE = :pId_Serie";
            
            command.Parameters.Add(new OracleParameter("Id_Inst", journal.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", journal.PublisherId));
            command.Parameters.Add(new OracleParameter("Id_Serie", journal.IdWithinPublisher));

            List<Issue> result = new List<Issue>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Issue(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt16(3), 
                                         reader.GetInt16(4), reader.GetInt16(5), reader.GetString(6), reader.GetInt16(7), 
                                         reader.GetBoolean(8), reader.GetString(9), reader.GetString(10)));
                }
            }

            return result;
        }

        public override Issue GetIssueByNumberWithinJournal(ResearchJournal journal, int number)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "SELECT ID_INST, ID_WYD, ID_SERIE, ID_W_SERII, NR_W_SERII, NR_W_WYDAWNICTWIE, TYTUL_PL, ROK_WYDANIA, FL_ZWER, TYP, NR_TYP " +
                "FROM filo.ZESZYTY " +
                "WHERE ID_INST = :pId_Inst AND ID_WYD = :pId_Wyd AND ID_SERIE = :pId_Serie AND NR_W_SERII = :pNr_W_Serii";
            
            command.Parameters.Add(new OracleParameter("Id_Inst", journal.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", journal.PublisherId));
            command.Parameters.Add(new OracleParameter("Id_Serie", journal.IdWithinPublisher));
            command.Parameters.Add(new OracleParameter("Nr_W_Serii", number));

            Issue result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Issue(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt16(3), 
                                         reader.GetInt16(4), reader.GetInt16(5), reader.GetString(6), reader["ROK_WYDANIA"] as int?, 
                                         reader.GetBoolean(8), reader.GetString(9), reader.GetString(10));
                }
            }

            return result;
        }

        public override Issue GetIssueByNumberWithinPublisher(Publisher publisher, int number)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "SELECT ID_INST, ID_WYD, ID_SERIE, ID_W_SERII, NR_W_SERII, NR_W_WYDAWNICTWIE, TYTUL_PL, ROK_WYDANIA, FL_ZWER, TYP, NR_TYP " +
                "FROM filo.ZESZYTY " +
                "WHERE ID_INST = :pId_Inst AND ID_WYD = :pId_Wyd AND NR_W_WYDAWNICTWIE = :pNr_W_Wyd";
            
            command.Parameters.Add(new OracleParameter("Id_Inst", publisher.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", publisher.IdWithinInstitution));
            command.Parameters.Add(new OracleParameter("Nr_W_Wyd", number));

            Issue result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Issue(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt16(3),
                                         reader.GetInt16(4), reader.GetInt16(5), reader.GetString(6), reader["ROK_WYDANIA"] as int?,
                                         reader.GetBoolean(8), reader.GetString(9), reader.GetString(10));
                }
            }

            return result;
        }

        public override IEnumerable<Article> GetArticlesFromIssue(Issue issue)
        {// FIX TODO, nie jestem pewny co do ID_ZESZYTY czemu odpowiada.
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "SELECT ID, ID_INST, ID_WYD, ID_SERIE, ID_ZESZYTY, TYTUL, TYTUL_PL, STR_OD, STR_DO, ID_WYD_OBCE, JEZYK, CZAS_WPR " +
                "FROM filo.ARTYKULY " +
                "WHERE ID_INST = :pId_Inst AND ID_WYD = :pId_Wyd AND ID_SERIE = :pId_Serie AND ID_ZESZYTY = :pId_Zesz";
            
            command.Parameters.Add(new OracleParameter("Id_Inst", issue.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", issue.PublisherId));
            command.Parameters.Add(new OracleParameter("Id_Serie", issue.JournalId));
            command.Parameters.Add(new OracleParameter("Id_Zesz", issue.IdWithinJournal));

            List<Article> result = new List<Article>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Article(reader.GetInt16(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                         reader["ID_ZESZYTY"] as int?, reader.GetString(5), reader.GetString(6), reader["STR_OD"] as int?,
                                         reader["STR_DO"] as int?, reader["ID_WYD_OBCE"] as int?, reader.GetString(10), reader.GetString(11)));
                }
            }

            return result;
        }

        public override IEnumerable<Category> GetArticleCategories(Article article)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "SELECT ID, OPIS, KATEGORIE_ID " +
                "FROM filo.KATEGORIE " +
                "WHERE ID = ( SELECT KATEGORIE_ID FROM filo.ARTYKULY_KAT WHERE ARTYKULY_ID = :pArt_Id )";

            command.Parameters.Add(new OracleParameter("Art_Id", article.Id));

            List<Category> result = new List<Category>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Category(reader.GetInt16(0), reader.GetString(1), reader["KATEGORIE_ID"] as int?));
                }
            }

            return result;
        }

        public override IEnumerable<Article> GetArticlesByCategory(Category category)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "SELECT ID, ID_INST, ID_WYD, ID_SERIE, ID_ZESZYTY, TYTUL, TYTUL_PL, STR_OD, STR_DO, ID_WYD_OBCE, JEZYK, CZAS_WPR " +
                "FROM filo.ARTYKULY " +
                "WHERE ID IN (SELECT DISTINCT ARTYKULY_ID FROM filo.ARTYKULY_KAT WHERE KATEGORIE_ID = :pId_Kat)";

            command.Parameters.Add(new OracleParameter("Id_Kat", category.Id));

            List<Article> result = new List<Article>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Article(reader.GetInt16(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                         reader["ID_ZESZYTY"] as int?, reader.GetString(5), reader.GetString(6), reader["STR_OD"] as int?,
                                         reader["STR_DO"] as int?, reader["ID_WYD_OBCE"] as int?, reader.GetString(10), reader.GetString(11)));
                }
            }

            return result;
        }

        // RETURNS NULL WHEN ALIENID IS NULL
        public override AlienPublisher GetAlienPublisherForArticle(Article article)
        {
            if (!article.AlienId.HasValue)
                return null;

            var command = m_connection.CreateCommand();
            command.CommandText =
                "SELECT ID, TYP, TYTUL, ROK_WYDANIA, NUMER, KRAJ " +
                "FROM filo.WYDAWNICTWA_OBCE " +
                "WHERE ID = :pId_Alien";

            command.Parameters.Add(new OracleParameter("Id_Alien", article.AlienId));

            AlienPublisher result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new AlienPublisher( reader.GetInt16(0), reader.GetString(1), reader.GetString(2), 
                                                 reader["ROK_WYDANIA"] as int?, reader.GetString(4), reader.GetString(5) );
                }
            }

            return result;
        }

        // RETURNS NULL WHEN ABOVECATEGORY IS NULL
        public override Category GetAboveCategory(Category category)
        {
            if (!category.AboveCategory.HasValue)
                return null;

            var command = m_connection.CreateCommand();
            command.CommandText =
                "SELECT ID, OPIS, KATEGORIE_ID " +
                "FROM filo.KATEGORIE " +
                "WHERE ID = :pAbove_Cat";

            command.Parameters.Add(new OracleParameter("Above_Cat", category.AboveCategory));

            Category result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Category(reader.GetInt16(0), reader.GetString(1), reader["KATEGORIE_ID"] as int?);
                }
            }

            return result;
        }

        public override IEnumerable<Author> GetArticleAuthors(Article article)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "SELECT ID, NAZWISKO, IMIE, IMIE2, NARODOWOSC " +
                "FROM filo.AUTORZY " +
                "WHERE ID IN (SELECT DISTINCT ID_AUT FROM filo.AUTORSTWO WHERE ID_ART = :pId_Art)";

            command.Parameters.Add(new OracleParameter("Id_Art", article.Id));

            List<Author> result = new List<Author>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Author(reader.GetInt16(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), 
                                          reader.GetString(4)));
                }
            }

            return result;
        }

        // Only Key field required to be filled in the keyword variable
        public override IEnumerable<Article> GetArticlesByKeyword(Keyword keyword)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "SELECT ID, ID_INST, ID_WYD, ID_SERIE, ID_ZESZYTY, TYTUL, TYTUL_PL, STR_OD, STR_DO, ID_WYD_OBCE, JEZYK, CZAS_WPR " +
                "FROM filo.ARTYKULY " +
                "WHERE ID IN (SELECT DISTINCT ARTYKULY_ID FROM filo.SLOWA_KLUCZE WHERE KLUCZ = :pKey)";

            command.Parameters.Add(new OracleParameter("Key", keyword.Key));

            List<Article> result = new List<Article>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Article(reader.GetInt16(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                         reader["ID_ZESZYTY"] as int?, reader.GetString(5), reader.GetString(6), reader["STR_OD"] as int?,
                                         reader["STR_DO"] as int?, reader["ID_WYD_OBCE"] as int?, reader.GetString(10), reader.GetString(11)));
                }
            }

            return result;
        }

        public override IEnumerable<Article> GetAnnotationsForArticle(Article article, int number = -1)
        {
            var command = m_connection.CreateCommand();

            List<Article> result = new List<Article>();
            if (number < 0)
            {
                command.CommandText =
                    "SELECT ID, ID_INST, ID_WYD, ID_SERIE, ID_ZESZYTY, TYTUL, TYTUL_PL, STR_OD, STR_DO, ID_WYD_OBCE, JEZYK, CZAS_WPR " +
                    "FROM filo.ARTYKULY " +
                    "WHERE ID IN (SELECT DISTINCT BIB_ART FROM filo.BIBLIOGRAFIA WHERE ID_ART = :pId)";

                command.Parameters.Add(new OracleParameter("Id", article.Id));  
            }
            else
            {
                command.CommandText =
                    "SELECT ID, ID_INST, ID_WYD, ID_SERIE, ID_ZESZYTY, TYTUL, TYTUL_PL, STR_OD, STR_DO, ID_WYD_OBCE, JEZYK, CZAS_WPR " +
                    "FROM filo.ARTYKULY " +
                    "WHERE ID IN (SELECT DISTINCT BIB_ART FROM filo.BIBLIOGRAFIA WHERE ID_ART = :pId AND NR_KOLEJNY = :pNr)";

                command.Parameters.Add(new OracleParameter("Id", article.Id));
                command.Parameters.Add(new OracleParameter("Nr", number));
            }

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Article(reader.GetInt16(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                         reader["ID_ZESZYTY"] as int?, reader.GetString(5), reader.GetString(6), reader["STR_OD"] as int?,
                                         reader["STR_DO"] as int?, reader["ID_WYD_OBCE"] as int?, reader.GetString(10), reader.GetString(11)));
                }
            }

            return result;
        }


        #endregion

        #region Save methods

        public override bool SaveInstitution(Institution institution)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "INSERT INTO filo.INSTYTUCJE ( NAZWA ) VALUES ( :pNazwa )";
            command.Parameters.Add(new OracleParameter("Nazwa", institution.Name ));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool SavePublisher(Publisher publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "INSERT INTO filo.WYDAWNICTWA ( ID_INST, TYTUL ) VALUES ( :pId_Inst, :pTytul )";
            command.Parameters.Add(new OracleParameter("Id_Inst", publisher.InstitutionId));
            command.Parameters.Add(new OracleParameter("Nazwa", publisher.Title));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool SaveResearchJournal(ResearchJournal journal)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "INSERT INTO filo.SERIE ( ID_INST, ID_WYD, TYTUL, ISSN ) " +
                "VALUES ( :pId_Inst, :pId_Wyd, :pTytul, :pIssn )";
            
            command.Parameters.Add(new OracleParameter("Id_Inst", journal.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", journal.PublisherId));
            command.Parameters.Add(new OracleParameter("Tytul", journal.Title));
            command.Parameters.Add(new OracleParameter("Issn", journal.ISSN));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }
        
        public override bool SaveIssue(Issue issue)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "INSERT INTO filo.ZESZYTY ( ID_INST, ID_WYD, ID_SERIE, NR_W_SERII, NR_W_WYDAWNICTWIE, TYTUL_PL, ROK_WYDANIA, FL_ZWER, TYP, NR_TYP ) " +
                "VALUES ( :pId_Inst, :pId_Wyd :pId_Serie, :pNr_W_Serii, :pNr_W_Wyd, :pTytul_Pl, :pRok_Wyd, :pFl_Zwer, :pTyp, :pNr_Typ )";
           
            command.Parameters.Add(new OracleParameter("Id_Inst", issue.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", issue.PublisherId));
            command.Parameters.Add(new OracleParameter("Id_Serie", issue.JournalId));
            command.Parameters.Add(new OracleParameter("Nr_W_Serii", issue.NumberWithinJournal));
            command.Parameters.Add(new OracleParameter("Nr_W_Wyd", issue.NumberWithinPublisher));
            command.Parameters.Add(new OracleParameter("Tytul_Pl", issue.Title));
            command.Parameters.Add(new OracleParameter("Rok_Wyd", issue.YearOfPublication));
            command.Parameters.Add(new OracleParameter("Fl_Zwer", issue.WasVerified));
            command.Parameters.Add(new OracleParameter("Typ", issue.Type));
            command.Parameters.Add(new OracleParameter("Nr_Typ", issue.TypeNumber));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool SaveArticle(Article article)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "INSERT INTO filo.INSTYTUCJE ( ID_INST, ID_WYD, ID_SERIE, ID_ZESZYTY, TYTUL, TYTUL_PL, STR_OD, STR_DO, ID_WYD_OBCE, JEZYK ) " +
                "VALUES ( :pId_Inst, :pId_Wyd, :pId_Serie, :pId_Zesz, :pTytul, :pTytul_Pl, :pStr_Od, :pStr_Do, :pId_Wyd_Obce, :pJezyk )";

            command.Parameters.Add(new OracleParameter("Id_Inst", article.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", article.PublisherId));
            command.Parameters.Add(new OracleParameter("Id_Serie", article.JournalId));
            command.Parameters.Add(new OracleParameter("Id_Zesz", article.IssueId));
            command.Parameters.Add(new OracleParameter("Tytul", article.Title));
            command.Parameters.Add(new OracleParameter("Tytul_Pl", article.TitlePl));
            command.Parameters.Add(new OracleParameter("Str_Od", article.PageBegin));
            command.Parameters.Add(new OracleParameter("Str_Do", article.PageEnd));
            command.Parameters.Add(new OracleParameter("Id_Wyd_Obce", article.AlienId));
            command.Parameters.Add(new OracleParameter("Jezyk", article.Lang));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool SaveAlienPublisher(AlienPublisher alien_publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.WYDAWNICTWA_OBCE ( TYP, TYTUL, ROK_WYDANIA, NUMER, KRAJ ) " +
                "VALUES ( :pTyp, :pTytul, :pRok_Wyd, :pNr, :pKraj )";

            command.Parameters.Add(new OracleParameter("Typ", alien_publisher.Type));
            command.Parameters.Add(new OracleParameter("Tytul", alien_publisher.Title));
            command.Parameters.Add(new OracleParameter("Rok_Wyd", alien_publisher.YearOfPublication));
            command.Parameters.Add(new OracleParameter("Nr", alien_publisher.Number));
            command.Parameters.Add(new OracleParameter("Kraj", alien_publisher.Country));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool SaveCategory(Category category)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.KATEGORIE ( OPIS, KATEGORIE_ID ) " +
                "VALUES ( :pOpis, :pNad_Kat_Id )";

            command.Parameters.Add(new OracleParameter("Opis", category.Info));
            command.Parameters.Add(new OracleParameter("Nad_Kat_Id", category.AboveCategory));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool SaveAuthor(Author author)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.AUTORZY ( NAZWISKO, IMIE, IMIE2, NARODOWOSC ) " +
                "VALUES ( :pNazw, :pImie, :pImie2, :pNar )";

            command.Parameters.Add(new OracleParameter("Nazw", author.LastName));
            command.Parameters.Add(new OracleParameter("Imie", author.Name));
            command.Parameters.Add(new OracleParameter("Imie2", author.Name2));
            command.Parameters.Add(new OracleParameter("Nar", author.Nationality));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        // WSZYSTKIE SLOWA KLUCZE PISZEMY MALYMI
        public override bool SaveKeyword(Keyword keyword)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.SLOWA_KLUCZE ( KLUCZ, ARTYKULY_ID, KRAJ ) " +
                "VALUES ( :pKlucz, :pArt_Id, :pKraj )";

            command.Parameters.Add(new OracleParameter("Klucz", keyword.Key));
            command.Parameters.Add(new OracleParameter("Art_Id", keyword.ArticleId));
            command.Parameters.Add(new OracleParameter("Kraj", keyword.Country));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }


        #endregion

        #region Add methods

        public override bool AddCategoryToArticle(Article article, Category category)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "INSERT INTO filo.ARTYKULY_KAT ( ARTYKULY_ID, KATEGORIE_ID ) VALUES ( :pId_Art, :pId_Kat )";
            command.Parameters.Add(new OracleParameter("Id_Art", article.Id));
            command.Parameters.Add(new OracleParameter("Id_Kat", category.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool AddAnnotationToArticle(Article article, Article annotation, int annotation_number)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "INSERT INTO filo.BIBLIOGRAFIA ( ID_ART, NR_KOLEJNY, BIB_ART ) VALUES ( :pId_Art, :pNr, :pId_Bib )";
            command.Parameters.Add(new OracleParameter("Id_Art", article.Id));
            command.Parameters.Add(new OracleParameter("Nr", annotation_number));
            command.Parameters.Add(new OracleParameter("Id_Bib", annotation.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool AddAuthorshipToArticle(Article article, Author author, int authorship_number)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.AUTORSTWO ( ID_ART, NR, ID_AUT, ID_INST ) VALUES ( :pId_Art, :pNr, :pId_Aut, :pId_Inst )";

            command.Parameters.Add(new OracleParameter("Id_Art", article.Id));
            command.Parameters.Add(new OracleParameter("Nr", authorship_number));
            command.Parameters.Add(new OracleParameter("Id_Aut", author.Id));
            command.Parameters.Add(new OracleParameter("Id_Inst", article.InstitutionId));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        #endregion

        #region Delete methods

        public override bool DeleteInstitution(Institution institution)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "DELETE FROM filo.INSTYTUCJE WHERE ID = :pId";
            command.Parameters.Add(new OracleParameter("Id", institution.Id));            

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeletePublisher(Publisher publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "DELETE FROM filo.WYDAWNICTWA WHERE ID_INST = :pId_Inst AND ID = :pId";
            command.Parameters.Add(new OracleParameter("Id_Inst", publisher.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id", publisher.IdWithinInstitution));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteResearchJournal(ResearchJournal journal)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "DELETE FROM filo.SERIE WHERE ID_INST = :pId_Inst AND ID_WYD = :pId_Wyd AND ID = :pId";
            command.Parameters.Add(new OracleParameter("Id_Inst", journal.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", journal.PublisherId));
            command.Parameters.Add(new OracleParameter("Id", journal.IdWithinPublisher));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteIssue(Issue issue)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "DELETE FROM filo.ZESZYTY WHERE ID_INST = :pId_Inst AND ID_WYD = :pId_Wyd AND ID_SERIE = :pId_Serie AND ID_W_SERII = :pId";

            command.Parameters.Add(new OracleParameter("Id_Inst", issue.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", issue.PublisherId));
            command.Parameters.Add(new OracleParameter("Id_Serie", issue.JournalId));
            command.Parameters.Add(new OracleParameter("Id_Serie", issue.IdWithinJournal));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteArticle(Article article)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "DELETE FROM filo.ARTYKULY WHERE ID = :pId";
            command.Parameters.Add(new OracleParameter("Id", article.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteAlienPublisher(AlienPublisher alien_publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "DELETE FROM filo.WYDAWNICTWA_OBCE WHERE ID = :pId";
            command.Parameters.Add(new OracleParameter("Id", alien_publisher.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteCategory(Category category)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "DELETE FROM filo.KATEGORIE WHERE ID = :pId";
            command.Parameters.Add(new OracleParameter("Id", category.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteAuthor(Author author)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "DELETE FROM filo.AUTORZY WHERE ID = :pId";
            command.Parameters.Add(new OracleParameter("Id", author.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteKeyword(Keyword keyword)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "DELETE FROM filo.SLOWA_KLUCZE WHERE KLUCZ = :pKey";
            command.Parameters.Add(new OracleParameter("Key", keyword.Key));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteAuthorshipFromArticle(Article article, Author author, int authorship_number)
        {
            var command = m_connection.CreateCommand();

            if (authorship_number < 0)
            {
                command.CommandText = "DELETE FROM filo.AUTORSTWO WHERE ID_ART = :pId_Art AND ID_AUT = :pId_Aut";
                command.Parameters.Add(new OracleParameter("Id_Art", article.Id));
                command.Parameters.Add(new OracleParameter("Id_Aut", author.Id));
            }
            else
            {
                command.CommandText = "DELETE FROM filo.AUTORSTWO WHERE ID_ART = :pId_Art AND ID_AUT = :pId_Aut AND NR = :pNr";
                command.Parameters.Add(new OracleParameter("Id_Art", article.Id));
                command.Parameters.Add(new OracleParameter("Id_Aut", author.Id));
                command.Parameters.Add(new OracleParameter("Nr", authorship_number));
            }

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteAnnotationFromArticle(Article article, Article annotation, int annotation_number)
        {
            var command = m_connection.CreateCommand();

            if (annotation_number < 0)
            {
                command.CommandText = "DELETE FROM filo.BIBLIOGRAFIA WHERE ID_ART = :pId_Art AND ID_BIB = :pId_Bib";
                command.Parameters.Add(new OracleParameter("Id_Art", article.Id));
                command.Parameters.Add(new OracleParameter("Id_Bib", annotation.Id));
            }
            else
            {
                command.CommandText = "DELETE FROM filo.BIBLIOGRAFIA WHERE ID_ART = :pId_Art AND ID_BIB = :pId_Bib AND NR_KOLEJNY = :pNr";
                command.Parameters.Add(new OracleParameter("Id_Art", article.Id));
                command.Parameters.Add(new OracleParameter("Id_Bib", annotation.Id));
                command.Parameters.Add(new OracleParameter("Nr", annotation_number));
            }

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteCategoryFromArticle(Article article, Category category, bool delete_all)
        {
            var command = m_connection.CreateCommand();

            if (!delete_all)
            {
                command.CommandText = "DELETE FROM filo.ARTYKULY_KAT WHERE ARTYKULY_ID = :pId_Art AND KATEGORIE_ID = :pId_Kat";
                command.Parameters.Add(new OracleParameter("Id_Art", article.Id));
                command.Parameters.Add(new OracleParameter("Id_Kat", category.Id));
            }
            else
            {
                command.CommandText = "DELETE FROM filo.ARTYKULY_KAT WHERE ID_ART = :pId_Art";
                command.Parameters.Add(new OracleParameter("Id_Art", article.Id));
            }

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        #endregion

        #region Update methods

        public override bool UpdateInstitution(Institution institution)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "UPDATE filo.INSTYTUCJE " +
                "SET NAZWA = :pNazwa " +
                "WHERE ID = :pId";

            command.Parameters.Add(new OracleParameter("Nazwa", institution.Name));
            command.Parameters.Add(new OracleParameter("Id", institution.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool UpdatePublisher(Publisher publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "UPDATE filo.WYDAWNICTWA " +
                "SET TYTUL = :pTytul " +
                "WHERE ID_INST = :pId_Inst AND ID = :pId";

            command.Parameters.Add(new OracleParameter("Tytul", publisher.Title));
            command.Parameters.Add(new OracleParameter("Id", publisher.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id", publisher.IdWithinInstitution));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool UpdateResearchJournal(ResearchJournal journal)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "UPDATE filo.SERIE " +
                "SET TYTUL = :pTytul , ISSN = :pIssn " +
                "WHERE ID_INST = :pId_Inst AND ID_WYD = :pId_Wyd AND ID = :pId";

            command.Parameters.Add(new OracleParameter("Tytul", journal.Title));
            command.Parameters.Add(new OracleParameter("Issn", journal.ISSN));
            command.Parameters.Add(new OracleParameter("Id_Inst", journal.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", journal.PublisherId));
            command.Parameters.Add(new OracleParameter("Id", journal.IdWithinPublisher));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool UpdateIssue(Issue issue)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "UPDATE filo.ZESZYTY " +
                "SET TYTUL_PL = :pTytul_Pl , ROK_WYDANIA = :pRok_Wyd , FL_ZWER = :pFl , TYP = :pTyp, NR_TYP = :pNr " +
                "WHERE ID_INST = :pId_Inst AND ID_WYD = :pId_Wyd AND ID_SERIE = :pId_Serie AND ID_W_SERII = :pId_W_Serii";

            command.Parameters.Add(new OracleParameter("Tytul_pl", issue.Title));
            command.Parameters.Add(new OracleParameter("Rok_Wyd", issue.YearOfPublication));
            command.Parameters.Add(new OracleParameter("Fl", issue.WasVerified));
            command.Parameters.Add(new OracleParameter("Typ", issue.Type));
            command.Parameters.Add(new OracleParameter("Nr", issue.TypeNumber));
            command.Parameters.Add(new OracleParameter("Id_Inst", issue.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", issue.PublisherId));
            command.Parameters.Add(new OracleParameter("Id_Serie", issue.JournalId));
            command.Parameters.Add(new OracleParameter("Id_W_Serii", issue.IdWithinJournal));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool UpdateArticle(Article article)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "UPDATE filo.INSTYTUCJE " +
                "SET TYTUL = :pTytul , TYTUL_PL = :pTytul_Pl , STR_OD = :pStr_Od , STR_DO = :pStr_Do " +
                "WHERE ID = :pId";

            command.Parameters.Add(new OracleParameter("Tytul", article.Title));
            command.Parameters.Add(new OracleParameter("Tytul_Pl", article.TitlePl));
            command.Parameters.Add(new OracleParameter("Str_Od", article.PageBegin));
            command.Parameters.Add(new OracleParameter("Str_Do", article.PageEnd));
            command.Parameters.Add(new OracleParameter("Id", article.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool UpdateAlienPublisher(AlienPublisher alien_publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "UPDATE filo.WYDAWNICTWA_OBCE " +
                "SET TYP = :pTyp , TYTUL = :pTytul , ROK_WYDANIA = :pRok_Wyd , NUMER = :pNr , KRAJ = :pKraj   " +
                "WHERE ID = :pId";

            command.Parameters.Add(new OracleParameter("Typ", alien_publisher.Type));
            command.Parameters.Add(new OracleParameter("Tytul", alien_publisher.Title));
            command.Parameters.Add(new OracleParameter("Rok_Wyd", alien_publisher.YearOfPublication));
            command.Parameters.Add(new OracleParameter("Nr", alien_publisher.Number));
            command.Parameters.Add(new OracleParameter("Kraj", alien_publisher.Country));
            command.Parameters.Add(new OracleParameter("Id", alien_publisher.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool UpdateCategory(Category category)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "UPDATE filo.KATEGORIE " +
                "SET OPIS = :pOpis , KATEGORIE_ID = :pNad_Kat_Id " +
                "WHERE ID = :pId";

            command.Parameters.Add(new OracleParameter("Opis", category.Info));
            command.Parameters.Add(new OracleParameter("Nad_Kat_Id", category.AboveCategory));
            command.Parameters.Add(new OracleParameter("Id", category.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool UpdateAuthor(Author author)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "UPDATE filo.AUTORZY " +
                "SET NAZWISKO = :pNazw , IMIE = :pImie , IMIE2 = :pImie2 , NARODOWOSC = :pNar " +
                "WHERE ID = :pId";

            command.Parameters.Add(new OracleParameter("Nazw", author.LastName));
            command.Parameters.Add(new OracleParameter("Imie", author.Name));
            command.Parameters.Add(new OracleParameter("Imie2", author.Name2));
            command.Parameters.Add(new OracleParameter("Nar", author.Nationality));
            command.Parameters.Add(new OracleParameter("Id", author.Id));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        // WSZYSTKIE SLOWA KLUCZE PISZEMY MALYMI
        public override bool UpdateKeyword(Keyword keyword)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "UPDATE filo.SLOWA_KLUCZE " +
                "SET ARTYKULY_ID = :pArt_Id , KRAJ = :pKraj " +
                "WHERE KLUCZ = :pKlucz";

            command.Parameters.Add(new OracleParameter("Art_Id", keyword.ArticleId));
            command.Parameters.Add(new OracleParameter("Kraj", keyword.Country));
            command.Parameters.Add(new OracleParameter("Klucz", keyword.Key));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        #endregion
    }
}