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
        private DbConnection m_connection;

        // Connection Methods
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

        // Get Methods
        public override IEnumerable<Institution> GetInstitutions()
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT * FROM FILO.Instytucje";

            List<Institution> result = new List<Institution>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add( new Institution(reader.GetInt16(0), reader.GetString(1)));
                }
            }

            return result;
        }

        public override IEnumerable<Publisher> GetPublishers()
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT * FROM FILO.Wydawnictwa";

            List<Publisher> result = new List<Publisher>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add( new Publisher(reader.GetInt16(0), reader.GetInt16(1), reader.GetString(2)));
                }  
            }

            return result;
        }

        public override IEnumerable<Publisher> GetPublishersForInstitution(Institution institution)
        {
            var command = m_connection.CreateCommand();

            // TODO FIX WARN: Nie jestem pewny czy tu ma byc id czy id_inst
            command.CommandText = "SELECT * FROM FILO.Wydawnictwa WHERE id_inst = :pId";
            command.Parameters.Add(institution.Id);

            List<Publisher> result = new List<Publisher>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Publisher(reader.GetInt16(0), reader.GetInt16(1), reader.GetString(2)));
                }
            }

            return result;
        }

        public override IEnumerable<ResearchJournal> GetJournalsForPublisher(Publisher publisher)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT * FROM FILO.Serie WHERE id_inst = :pId_Inst AND id_wyd = :pId_Wyd";
            command.Parameters.Add(publisher.InstitutionId);
            command.Parameters.Add(publisher.IdWithinInstitution);

            List<ResearchJournal> result = new List<ResearchJournal>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new ResearchJournal(reader.GetInt16(0), reader.GetInt16(1), reader.GetInt16(2), 
                                                   reader.GetString(3), reader.GetString(4)));
                }
            }

            return result;
        }

        public override IEnumerable<Issue> GetIssuesForJournal(ResearchJournal journal)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT * FROM FILO.Zeszyty WHERE id_inst = :pId_Inst AND id_wyd = :pId_Wyd AND id_serie = :pId_Serie";
            command.Parameters.Add(journal.InstitutionId);
            command.Parameters.Add(journal.PublisherId);
            command.Parameters.Add(journal.IdWithinPublisher);

            List<Issue> result = new List<Issue>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Issue(reader.GetInt16(0), reader.GetInt16(1), reader.GetInt16(2), reader.GetInt16(3), 
                                         reader.GetInt16(4), reader.GetInt16(5), reader.GetString(6), reader["rok_wydania"] as int?, 
                                         reader.GetBoolean(8), reader.GetString(9), reader.GetString(10)));
                }
            }

            return result;
        }

        public override Issue GetIssueByNumberWithinJournal(ResearchJournal journal, int number)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT * FROM FILO.Artykuly WHERE id_inst = :pId_Inst AND id_wyd = :pId_Wyd AND id_serie = :pId_Serie AND nr_w_serii = :pNr_W_Serii";
            command.Parameters.Add(journal.InstitutionId);
            command.Parameters.Add(journal.PublisherId);
            command.Parameters.Add(journal.IdWithinPublisher);
            command.Parameters.Add(number);

            Issue result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Issue(reader.GetInt16(0), reader.GetInt16(1), reader.GetInt16(2), reader.GetInt16(3),
                                         reader.GetInt16(4), reader.GetInt16(5), reader.GetString(6), reader["rok_wydania"] as int?, 
                                         reader.GetBoolean(8), reader.GetString(9), reader.GetString(10));
                }
            }

            return result;
        }

        public override Issue GetIssueByNumberWithinPublisher(Publisher publisher, int number)
        {
            var command = m_connection.CreateCommand();
            command.CommandText = "SELECT * FROM FILO.Artykuly WHERE id_inst = :pId_Inst AND id_wyd = :pId_Wyd AND nr_w_wyd = :pNr_W_Wyd";
            command.Parameters.Add(publisher.InstitutionId);
            command.Parameters.Add(publisher.IdWithinInstitution);
            command.Parameters.Add(number);

            Issue result = null;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Issue(reader.GetInt16(0), reader.GetInt16(1), reader.GetInt16(2), reader.GetInt16(3),
                                         reader.GetInt16(4), reader.GetInt16(5), reader.GetString(6), reader["rok_wydania"] as int?, 
                                         reader.GetBoolean(8), reader.GetString(9), reader.GetString(10));
                }
            }

            return result;
        }
    }
}
