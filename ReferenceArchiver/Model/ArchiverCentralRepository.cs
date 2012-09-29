using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace ReferenceArchiver.Model
{
    class ArchiverCentralRepository : CentralRepository
    {
        #region Constants

        private const string ArticleDatabaseColumns = "artykuly.ID, artykuly.ID_INST, artykuly.ID_WYD, artykuly.ID_SERIE, artykuly.ID_ZESZYTY, artykuly.TYTUL, artykuly.TYTUL_PL, artykuly.STR_OD, artykuly.STR_DO, artykuly.ID_WYD_OBCE, artykuly.JEZYK, artykuly.CZAS_WPR";

        #endregion

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

        public override Institution GetInstitutionByName(string name)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT ID, NAZWA FROM filo.INSTYTUCJE WHERE NAZWA = :pNazwa";
            command.Parameters.Add(new OracleParameter("Nazwa", name));

            Institution result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Institution(reader.GetString(0), reader.GetString(1));
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

        public override Publisher GetPublisherByName(string name, string id_inst)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT ID_INST, ID, TYTUL FROM filo.WYDAWNICTWA WHERE TYTUL = :pNazwa AND ID_INST = :pId_inst";
            command.Parameters.Add(new OracleParameter("Nazwa", name));
            command.Parameters.Add(new OracleParameter("Id_inst", id_inst));

            Publisher result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Publisher(reader.GetString(0), reader.GetString(1), reader.GetString(2));
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
                                                   reader.GetString(3), reader["ISSN"] as string));
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
                    result.Add(new Issue(reader.GetString(0), reader.GetString(1), reader.GetString(2), (int)reader["ID_W_SERII"],
                                         (int)reader["NR_W_SERII"], (int)reader["NR_W_WYDAWNICTWIE"], reader.GetString(6), reader["ROK_WYDANIA"] as short?,
                                         (string)reader["FL_ZWER"] == "T", reader.GetString(9), reader["NR_TYP"] as string));
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
                    result = new Issue(reader.GetString(0), reader.GetString(1), reader.GetString(2), (int)reader["ID_W_SERII"],
                                         (int)reader["NR_W_SERII"], (int)reader["NR_W_WYDAWNICTWIE"], reader.GetString(6), reader["ROK_WYDANIA"] as short?,
                                         (string)reader["FL_ZWER"] == "T", reader.GetString(9), reader["NR_TYP"] as string);
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
                    result = new Issue(reader.GetString(0), reader.GetString(1), reader.GetString(2), (int)reader["ID_W_SERII"],
                                         (int)reader["NR_W_SERII"], (int)reader["NR_W_WYDAWNICTWIE"], reader.GetString(6), reader["ROK_WYDANIA"] as short?,
                                         (string)reader["FL_ZWER"] == "T", reader.GetString(9), reader["NR_TYP"] as string);
                }
            }

            return result;
        }

        public override IEnumerable<Article> GetArticles()
        {
            return GetArticles(null, null);
        }

        public override IEnumerable<Article> GetArticlesFromIssue(Issue issue)
        {// FIX TODO, nie jestem pewny co do ID_ZESZYTY czemu odpowiada.
            var parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("Id_Inst", issue.InstitutionId));
            parameters.Add(new OracleParameter("Id_Wyd", issue.PublisherId));
            parameters.Add(new OracleParameter("Id_Serie", issue.JournalId));
            parameters.Add(new OracleParameter("Id_Zesz", issue.IdWithinJournal));

            return GetArticles("ID_INST = :pId_Inst AND ID_WYD = :pId_Wyd AND ID_SERIE = :pId_Serie AND ID_ZESZYTY = :pId_Zesz", parameters);
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
            var parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("Id_Kat", category.Id));
            return GetArticles("ID IN (SELECT DISTINCT ARTYKULY_ID FROM filo.ARTYKULY_KAT WHERE KATEGORIE_ID = :pId_Kat)", parameters);
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
                    result = new AlienPublisher(reader.GetInt16(0), reader.GetString(1), reader.GetString(2),
                                                 reader["ROK_WYDANIA"] as int?, reader.GetString(4), reader.GetString(5));
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

        public override IEnumerable<Author> GetAuthors()
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "SELECT ID, NAZWISKO, IMIE, IMIE2, NARODOWOSC " +
                "FROM filo.AUTORZY";

            List<Author> result = new List<Author>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Author((int)reader["ID"], reader.GetString(1), reader.GetString(2), reader["IMIE2"] as string,
                                          reader.GetString(4)));
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
                    result.Add(new Author((int)reader["ID"], reader.GetString(1), reader.GetString(2), reader["IMIE2"] as string,
                                          reader.GetString(4)));
                }
            }

            return result;
        }

        // Only Key field required to be filled in the keyword variable
        public override IEnumerable<Article> GetArticlesByKeyword(Keyword keyword)
        {
            var parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("Key", keyword.Key));
            return GetArticles("ID IN (SELECT DISTINCT ARTYKULY_ID FROM filo.SLOWA_KLUCZE WHERE KLUCZ = :pKey)", parameters);
        }

        public override IEnumerable<Annotation> GetAnnotationsForArticle(Article article, int number = -1)
        {
            var command = m_connection.CreateCommand();

            List<Annotation> result = new List<Annotation>();
            if (number < 0)
            {
                command.CommandText =
                    "SELECT ID_ART, NR_KOLEJNY, BIB_ART " +
                    "FROM filo.BIBLIOGRAFIA " +
                    "WHERE ID_ART = :pId_Art " +
                    "ORDER BY NR_KOLEJNY";

                command.Parameters.Add(new OracleParameter("Id", article.Id));
            }
            else
            {
                command.CommandText =
                    "SELECT ID_ART, NR_KOLEJNY, BIB_ART " +
                    "FROM filo.BIBLIOGRAFIA " +
                    "WHERE ID_ART = :pId_Art AND NR_KOLEJNY = :pNr";

                command.Parameters.Add(new OracleParameter("Id", article.Id));
                command.Parameters.Add(new OracleParameter("Nr", number));
            }

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Annotation(reader.GetInt16(0), reader.GetInt16(1), reader.GetInt16(2)));
                }
            }

            return result;
        }

        public override IEnumerable<Article> GetReferencedArticlesForArticle(Article article)
        {
            DbCommand command = m_connection.CreateCommand();
            command.CommandText = string.Format(
                @"SELECT {0} 
                FROM filo.bibliografia 
                JOIN filo.artykuly ON bibliografia.id_art=artykuly.id
                WHERE bibliografia.id_art = :pIdArt
                ORDER BY bibliografia.nr_kolejny", 
                ArticleDatabaseColumns);
            command.Parameters.Add(new OracleParameter("IdArt", article.Id));

            DbDataReader reader = command.ExecuteReader();
            return ReadArticles(reader);
        }

        // RETURNS NULL ON id < 0
        public override Article GetArticleById(int id)
        {
            if (id < 0)
                return null;

            var parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter("Id", id));

            return GetArticles("ID = :pId", parameters).FirstOrDefault();
        }

        public override int GetAuthorAfiliationForArticle(Article article, Author author)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "SELECT ID_INST " +
                "FROM filo.AUTORSTWO " +
                "WHERE ID_ART = :pId_Art AND ID_AUT = :pIdAut";

            command.Parameters.Add(new OracleParameter("Id_Art", article.Id));
            command.Parameters.Add(new OracleParameter("Id_Aut", author.Id));

            int result = 0;
            int num = 0;

            // There should be only 1 result
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = reader.GetInt16(0);
                    ++num;
                }

                if (num > 1)
                    return -1;
            }

            return result;
        }

        public override IEnumerable<Country> GetCountries()
        {
            return GetCountries(null, null);
        }

        public override Country GetCountryForLanguage(string lang)
        {
            return GetCountries(
                "WHERE KOD = (SELECT KOD FROM filo.KRAJE_JEZYKI WHERE JEZYK = :pLang",
                new List<OracleParameter> { new OracleParameter("Lang", lang) })
                .FirstOrDefault();
        }

        public override Country GetCountryByCode(string code)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT KOD, NAZWA, FL_AKT FROM filo.KRAJE WHERE KOD = :pKod";

            command.Parameters.Add(new OracleParameter("Kod", code));

            Country result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Country(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                    return result;
                }
            }

            return result;
        }

        public override Country GetCountryByName(string name)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT KOD, NAZWA, FL_AKT FROM filo.KRAJE WHERE NAZWA = :pName";

            command.Parameters.Add(new OracleParameter("Name", name));

            Country result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Country(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                    return result;
                }
            }

            return result;
        }

        public override string GetLanguageForCountry(Country country)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT JEZYK FROM filo.KRAJE_JEZYKI WHERE KOD = :pKod";

            command.Parameters.Add(new OracleParameter("Kod", country.Code));

            string result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = reader.GetString(0);
                    return result;
                }
            }

            return result;
        }

        public override IEnumerable<Language> GetLanguages()
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT KOD, JEZYK FROM filo.KRAJE_JEZYKI ORDER BY JEZYK";

            var result = new List<Language>();

            DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Language()
                {
                    CountryCode = reader["KOD"] as string,
                    Name = reader["JEZYK"] as string
                });
            }

            return result;
        }

        public override IEnumerable<AuthorshipData> GetAuthorshipDataForArticle(Article article)
        {
            using (DbCommand command = m_connection.CreateCommand())
            {
                command.CommandText = @"
                SELECT 
                  autorzy.ID AS author_id, 
                  autorzy.imie AS author_first_name, 
                  autorzy.imie2 AS author_middle_name, 
                  autorzy.nazwisko AS author_last_name, 
                  autorzy.narodowosc AS author_nationality,
                  instytucje.id AS institution_id,
                  instytucje.nazwa AS institution_name
                FROM autorstwo
                JOIN autorzy ON autorstwo.id_aut=autorzy.id
                JOIN instytucje ON instytucje.id=autorstwo.id_inst
                WHERE autorstwo.id_art = :pArtId
                ORDER BY autorstwo.nr";
                command.Parameters.Add(new OracleParameter("ArtId", article.Id));

                using (DbDataReader reader = command.ExecuteReader())
                {
                    var result = new List<AuthorshipData>();
                    while (reader.Read())
                    {
                        var institution = new Institution()
                        {
                            Id = reader["institution_id"] as string,
                            Name = reader["institution_name"] as string
                        };

                        var author = new Author(
                            (int)reader["author_id"], 
                            reader["author_last_name"] as string, 
                            reader["author_first_name"] as string, 
                            reader["author_middle_name"] as string, 
                            reader["author_nationality"] as string);

                        result.Add(new AuthorshipData()
                        {
                            Author = author,
                            Affiliation = institution
                        });
                    }

                    return result;
                }
            }
        }

        #endregion

        #region Save methods

        public override string SaveInstitution(Institution institution)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "INSERT INTO filo.INSTYTUCJE ( NAZWA ) VALUES ( :pNazwa ) RETURNING ID INTO :pId";
            command.Parameters.Add(new OracleParameter("Nazwa", institution.Name));
            command.Parameters.Add(new OracleParameter("Id", OracleDbType.Int64, ParameterDirection.ReturnValue));

            if (command.ExecuteNonQuery() < 1)
                return "";

            return command.Parameters["Id"].Value.ToString();
        }

        public override string SavePublisher(Publisher publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "INSERT INTO filo.WYDAWNICTWA ( ID_INST, TYTUL ) VALUES ( :pId_Inst, :pTytul ) RETURNING ID INTO :pId";
            command.Parameters.Add(new OracleParameter("Id_Inst", publisher.InstitutionId));
            command.Parameters.Add(new OracleParameter("Tytul", publisher.Title));
            command.Parameters.Add(new OracleParameter("Id", OracleDbType.Int64, ParameterDirection.ReturnValue));

            if (command.ExecuteNonQuery() < 1)
                return "";

            return command.Parameters["Id"].Value.ToString();
        }

        public override string SaveResearchJournal(ResearchJournal journal)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.SERIE ( ID_INST, ID_WYD, TYTUL, ISSN ) " +
                "VALUES ( :pId_Inst, :pId_Wyd, :pTytul, :pIssn ) RETURNING ID INTO :pId";

            command.Parameters.Add(new OracleParameter("Id_Inst", journal.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", journal.PublisherId));
            command.Parameters.Add(new OracleParameter("Tytul", journal.Title));
            command.Parameters.Add(new OracleParameter("Issn", journal.ISSN));
            command.Parameters.Add(new OracleParameter("Id", OracleDbType.Int64, ParameterDirection.ReturnValue));

            if (command.ExecuteNonQuery() < 1)
                return "";

            return command.Parameters["Id"].Value.ToString();
        }

        public override string SaveIssue(Issue issue)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.ZESZYTY ( ID_INST, ID_WYD, ID_SERIE, NR_W_SERII, NR_W_WYDAWNICTWIE, TYTUL_PL, ROK_WYDANIA, FL_ZWER, TYP, NR_TYP ) " +
                "VALUES ( :pId_Inst, :pId_Wyd, :pId_Serie, :pNr_W_Serii, :pNr_W_Wyd, :pTytul_Pl, :pRok, :pFl_Zwer, :pTyp, :pNr_Typ ) RETURNING ID_W_SERII INTO :pId";

            command.Parameters.Add(new OracleParameter("Id_Inst", issue.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", issue.PublisherId));
            command.Parameters.Add(new OracleParameter("Id_Serie", issue.JournalId));
            command.Parameters.Add(new OracleParameter("Nr_W_Serii", OracleDbType.Long, issue.NumberWithinJournal, ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Nr_W_Wyd", OracleDbType.Long, issue.NumberWithinPublisher, ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Tytul_Pl", issue.Title));
            command.Parameters.Add(new OracleParameter("Rok_Wyd", OracleDbType.Long, issue.YearOfPublication, ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Fl_Zwer", OracleDbType.Char, issue.WasVerified ? 'T' : 'N', ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Typ", issue.TypeSave));
            command.Parameters.Add(new OracleParameter("Nr_Typ", issue.TypeNumber));
            command.Parameters.Add(new OracleParameter("Id", OracleDbType.Int64, ParameterDirection.ReturnValue));

            if (command.ExecuteNonQuery() < 1)
                return "";

            return command.Parameters["Id"].Value.ToString();
        }

        public override bool SaveArticle(Article article)
        {
            return SaveArticle(article, null);
        }

        private bool SaveArticle(Article article, DbTransaction transaction)
        {
            var command = m_connection.CreateCommand();

            if (transaction != null)
                command.Transaction = transaction;

            command.CommandText =
                "INSERT INTO filo.ARTYKULY ( ID_INST, ID_WYD, ID_SERIE, ID_ZESZYTY, TYTUL, TYTUL_PL, STR_OD, STR_DO, ID_WYD_OBCE, JEZYK ) " +
                "VALUES ( :pId_Inst, :pId_Wyd, :pId_Serie, :pId_Zesz, :pTytul, :pTytul_Pl, :pStr_Od, :pStr_Do, :pId_Wyd_Obce, :pJezyk )" +
                "RETURNING ID INTO :pNewArticleId";

            command.Parameters.Add(new OracleParameter("Id_Inst", article.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", article.PublisherId));
            command.Parameters.Add(new OracleParameter("Id_Serie", article.JournalId));
            command.Parameters.Add(new OracleParameter("Id_Zesz", OracleDbType.Long, article.IssueId, ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Tytul", article.Title));
            command.Parameters.Add(new OracleParameter("Tytul_Pl", article.TitlePl));
            command.Parameters.Add(new OracleParameter("Str_Od", OracleDbType.Long, article.PageBegin, ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Str_Do", OracleDbType.Long, article.PageEnd, ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Id_Wyd_Obce", OracleDbType.Long, article.AlienId, ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Jezyk", article.Lang));

            command.Parameters.Add(new OracleParameter("NewArticleId", OracleDbType.Decimal, ParameterDirection.ReturnValue));

            if (command.ExecuteNonQuery() < 1)
                return false;

            OracleDecimal idInOracleType = (OracleDecimal)command.Parameters["NewArticleId"].Value;
            article.Id = (long)idInOracleType.Value;

            return true;
        }

        public override string SaveAlienPublisher(AlienPublisher alien_publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.WYDAWNICTWA_OBCE ( TYP, TYTUL, ROK_WYDANIA, NUMER, KRAJ ) " +
                "VALUES ( :pTyp, :pTytul, :pRok_Wyd, :pNr, :pKraj ) RETURNING ID INTO :pId";

            command.Parameters.Add(new OracleParameter("Typ", alien_publisher.Type));
            command.Parameters.Add(new OracleParameter("Tytul", alien_publisher.Title));
            command.Parameters.Add(new OracleParameter("Rok_Wyd", OracleDbType.Long, alien_publisher.YearOfPublication, ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Nr", alien_publisher.Number));
            command.Parameters.Add(new OracleParameter("Kraj", alien_publisher.Country));
            command.Parameters.Add(new OracleParameter("Id", OracleDbType.Int64, ParameterDirection.ReturnValue));

            if (command.ExecuteNonQuery() < 1)
                return "";

            return command.Parameters["Id"].Value.ToString();
        }

        public override bool SaveCategory(Category category)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.KATEGORIE ( OPIS, KATEGORIE_ID ) " +
                "VALUES ( :pOpis, :pNad_Kat_Id )";

            command.Parameters.Add(new OracleParameter("Opis", category.Info));
            command.Parameters.Add(new OracleParameter("Nad_Kat_Id", OracleDbType.Long, category.AboveCategory, ParameterDirection.InputOutput));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override string SaveAuthor(Author author)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.AUTORZY ( NAZWISKO , IMIE , IMIE2 , NARODOWOSC ) " +
                "VALUES ( :pNazw , :pImie , :pImie2 , :pNar ) RETURNING ID INTO :pId";

            command.Parameters.Add(new OracleParameter("Nazw", author.LastName));
            command.Parameters.Add(new OracleParameter("Imie", author.Name));
            command.Parameters.Add(new OracleParameter("Imie2", author.Name2));
            command.Parameters.Add(new OracleParameter("Nar", author.Nationality));
            command.Parameters.Add(new OracleParameter("Id", OracleDbType.Int64, ParameterDirection.ReturnValue));

            if (command.ExecuteNonQuery() < 1)
                return "";

            return command.Parameters["Id"].Value.ToString();
        }

        // WSZYSTKIE SLOWA KLUCZE PISZEMY MALYMI
        public override bool SaveKeyword(Keyword keyword)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.SLOWA_KLUCZE ( KLUCZ, ARTYKULY_ID, KRAJ ) " +
                "VALUES ( :pKlucz, :pArt_Id, :pKraj )";

            command.Parameters.Add(new OracleParameter("Klucz", keyword.Key));
            command.Parameters.Add(new OracleParameter("Art_Id", OracleDbType.Long, keyword.ArticleId, ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Kraj", keyword.Country));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool SaveCountry(Country country)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.KRAJE ( KOD, NAZWA, FL_AKT ) " +
                "VALUES ( :pKod, :pNazwa, :pFl )";

            command.Parameters.Add(new OracleParameter("Kod", country.Code));
            command.Parameters.Add(new OracleParameter("Nazwa", country.Name));
            command.Parameters.Add(new OracleParameter("Fl", country.Flag));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override void SaveArticleWithAuthorshipsAndReferences(Article article, IList<Authorship> authorships, IList<Article> references)
        {
            using (DbTransaction transaction = m_connection.BeginTransaction())
            {
                if (article.Id >= 0)
                {
                    DeleteAuthorshipsForArticle(article, transaction);
                    DeleteReferencesForArticle(article, transaction);
                }

                SaveArticle(article, transaction);

                for (int i = 0; i < references.Count; i++)
                {
                    Article reference = references[i];

                    DbCommand command = m_connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = 
                        "INSERT INTO filo.bibliografia (id_art, nr_kolejny, bib_art) " +
                        "VALUES (:pArtId, :pNumber, :pRefArtId)";
                    command.Parameters.Add(new OracleParameter("ArtId", article.Id));
                    command.Parameters.Add(new OracleParameter("Number", i + 1));
                    command.Parameters.Add(new OracleParameter("RefArtId", reference.Id));
                    command.ExecuteNonQuery();
                }

                for (int i = 0; i < authorships.Count; i++)
                {
                    Authorship authorship = authorships[i];
                    authorship.Number = i + 1;
                    authorship.ArticleId = article.Id;

                    DbCommand command = m_connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = 
                        "INSERT INTO filo.autorstwo (id_art, nr, id_aut, id_inst) " +
                        "VALUES (:pIdArt, :pNumber, :pIdAuth, :pIdInst)";
                    command.Parameters.Add(new OracleParameter("IdArt", authorship.ArticleId));
                    command.Parameters.Add(new OracleParameter("Number", authorship.Number));
                    command.Parameters.Add(new OracleParameter("IdAuth", authorship.AuthorId));
                    command.Parameters.Add(new OracleParameter("IdInst", authorship.InstitutionId));
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
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

        public override bool AddLanguageToCountry(Country country, string lang)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "INSERT INTO filo.KRAJE_JEZYKI ( KOD, JEZYK ) VALUES ( :pKod, :pJezyk )";

            command.Parameters.Add(new OracleParameter("Kod", country.Code));
            command.Parameters.Add(new OracleParameter("Jezyk", lang));

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

        public override bool DeleteCountry(Country country)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "DELETE FROM filo.KRAJE WHERE KOD = :pKod";

            command.Parameters.Add(new OracleParameter("Kod", country.Code));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool DeleteLanguageFromCountry(Country country)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "DELETE FROM filo.KRAJE_JEZYKI WHERE KOD = :pKod";

            command.Parameters.Add(new OracleParameter("Kod", country.Code));

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
            command.Parameters.Add(new OracleParameter("Fl_Zwer", OracleDbType.Char, issue.WasVerified ? 'T' : 'N', ParameterDirection.InputOutput));
            command.Parameters.Add(new OracleParameter("Typ", issue.TypeSave));
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

        public override bool UpdateCountry(Country country)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "Update filo.KRAJE " +
                "SET NAZWA = :pNazwa , FL_AKT = :pFl " +
                "WHERE KOD = :pKod";

            command.Parameters.Add(new OracleParameter("Nazwa", country.Name));
            command.Parameters.Add(new OracleParameter("Fl", country.Flag));
            command.Parameters.Add(new OracleParameter("Kod", country.Code));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        #endregion

        #region Private methods

        private IEnumerable<Article> GetArticles(string where, IEnumerable<OracleParameter> parameters)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("SELECT {0} FROM filo.ARTYKULY ", ArticleDatabaseColumns);
            if (!string.IsNullOrWhiteSpace(where))
                queryBuilder.AppendFormat("WHERE {0}", where);

            var command = m_connection.CreateCommand();
            command.CommandText = queryBuilder.ToString();

            if (parameters != null)
            {
                foreach (OracleParameter parameter in parameters)
                    command.Parameters.Add(parameter);
            }

            using (var reader = command.ExecuteReader())
            {
                return ReadArticles(reader);
            }
        }

        private static IEnumerable<Article> ReadArticles(DbDataReader reader)
        {
            List<Article> result = new List<Article>();
            while (reader.Read())
            {
                result.Add(new Article((long)reader["ID"], reader["ID_INST"] as string, reader["ID_WYD"] as string, reader["ID_ZESZYTY"] as string,
                                     reader["ID_ZESZYTY"] as int?, reader.GetString(5), reader["TYTUL_PL"] as string, reader["STR_OD"] as int?,
                                     reader["STR_DO"] as int?, reader["ID_WYD_OBCE"] as int?, reader.GetString(10), (DateTime)reader["CZAS_WPR"]));
            }
            return result;
        }

        private IEnumerable<Country> GetCountries(string where, IEnumerable<OracleParameter> parameters)
        {
            var command = m_connection.CreateCommand();
            command.CommandText =
                "SELECT KOD, NAZWA, FL_AKT " +
                "FROM filo.KRAJE";
            if (!string.IsNullOrWhiteSpace(where))
                command.CommandText += " " + where;

            if (parameters != null)
                foreach (var parameter in parameters)
                    command.Parameters.Add(parameter);

            using (var reader = command.ExecuteReader())
            {
                var result = new List<Country>();
                while (reader.Read())
                {
                    result.Add(new Country(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                }

                return result;
            }
        }

        private void DeleteReferencesForArticle(Article article, DbTransaction transaction)
        {
            var command = m_connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = "DELETE FROM filo.autorstwo WHERE id_art=:pIdArt";
            command.Parameters.Add(new OracleParameter("IdArt", article.Id));
            command.ExecuteNonQuery();
        }

        private void DeleteAuthorshipsForArticle(Article article, DbTransaction transaction)
        {
            var command = m_connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = "DELETE FROM filo.autorstwo WHERE id_art=:pIdArt";
            command.Parameters.Add(new OracleParameter("IdArt", article.Id));
            command.ExecuteNonQuery();
        }

        #endregion
    }
}
