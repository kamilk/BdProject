using System;
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
                "FROM FROM filo.ZESZYTY " +
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
                "FROM FROM filo.ZESZYTY " +
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

        #endregion

        #region Save methods

        public override bool SaveInstitution(Institution institution)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "INSERT INTO filo.INSTYTUCJE ( ID, NAZWA ) VALUES ( :pId, :pNazwa )";
            command.Parameters.Add(new OracleParameter("Id", institution.Id ));
            command.Parameters.Add(new OracleParameter("Nazwa", institution.Name ));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool SavePublisher(Publisher publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "INSERT INTO filo.WYDAWNICTWA ( ID_INST, ID, TYTUL ) VALUES ( :pId_Inst, :pId, :pTytul )";
            command.Parameters.Add(new OracleParameter("Id_Inst", publisher.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id", publisher.IdWithinInstitution));
            command.Parameters.Add(new OracleParameter("Nazwa", publisher.Title));

            if (command.ExecuteNonQuery() < 1)
                return false;

            return true;
        }

        public override bool SaveResearchJournal(ResearchJournal journal)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = 
                "INSERT INTO filo.SERIE ( ID_INST, ID_WYD, ID, TYTUL, ISSN ) " +
                "VALUES ( :pId_Inst, :pId_Wyd :pId, :pTytul, :pIssn )";
            
            command.Parameters.Add(new OracleParameter("Id_Inst", journal.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", journal.PublisherId));
            command.Parameters.Add(new OracleParameter("Id", journal.IdWithinPublisher));
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
                "INSERT INTO filo.ZESZYTY ( ID_INST, ID_WYD, ID_SERIE, ID_W_SERII, NR_W_SERII, NR_W_WYDAWNICTWIE, TYTUL_PL, ROK_WYDANIA, FL_ZWER, TYP, NR_TYP ) " +
                "VALUES ( :pId_Inst, :pId_Wyd :pId_Serie, :pId_W_Serii, :pNr_W_Serii, :pNr_W_Wyd, :pTytul_Pl, :pRok_Wyd, :pFl_Zwer, :pTyp, :pNr_Typ )";
           
            command.Parameters.Add(new OracleParameter("Id_Inst", issue.InstitutionId));
            command.Parameters.Add(new OracleParameter("Id_Wyd", issue.PublisherId));
            command.Parameters.Add(new OracleParameter("Id_Serie", issue.JournalId));
            command.Parameters.Add(new OracleParameter("Id_W_Serii", issue.IdWithinJournal));
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
                "INSERT INTO filo.INSTYTUCJE ( ID, ID_INST, ID_WYD, ID_SERIE, ID_ZESZYTY, TYTUL, TYTUL_PL, STR_OD, STR_DO, ID_WYD_OBCE, JEZYK ) " +
                "VALUES ( :pId, :pId_Inst, :pId_Wyd, :pId_Serie, :pId_Zesz, :pTytul, :pTytul_Pl, :pStr_Od, :pStr_Do, :pId_Wyd_Obce, :pJezyk )";

            command.Parameters.Add(new OracleParameter("Id", article.Id));
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

        #endregion
    }
}
