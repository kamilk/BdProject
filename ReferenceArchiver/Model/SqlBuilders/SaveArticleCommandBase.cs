using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Oracle.DataAccess.Client;
using System.Data;

namespace ReferenceArchiver.Model.SqlBuilders
{
    abstract class SaveArticleCommandBase
    {
        #region Properties

        public long ArticleId { get; protected set; }
        protected DbCommand Command { get; private set; }

        #endregion

        #region Constructors

        protected SaveArticleCommandBase(DbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            Command = command;
            ArticleId = -1;
        }

        #endregion

        #region Methods

        public abstract void AddValue(string databaseColumn, OracleParameter parameter);
        public abstract bool Execute(DbTransaction transaction = null);

        #endregion

        #region Static methods

        public static SaveArticleCommandBase CreateSaveCommandForArticle(Article article, DbCommand command)
        {
            SaveArticleCommandBase result;
            if (article.Id < 0)
                result = new InsertArticleCommand(command);
            else
                result = new UpdateArticleCommand(command, article.Id);

            result.AddValue("ID_INST", new OracleParameter("Id_Inst", article.InstitutionId));
            result.AddValue("ID_WYD", new OracleParameter("Id_Wyd", article.PublisherId));
            result.AddValue("ID_SERIE", new OracleParameter("Id_Serie", article.JournalId));
            result.AddValue("ID_ZESZYTY", new OracleParameter("Id_Zesz", OracleDbType.Long, article.IssueId, ParameterDirection.InputOutput));
            result.AddValue("TYTUL", new OracleParameter("Tytul", article.Title));
            result.AddValue("TYTUL_PL", new OracleParameter("Tytul_Pl", article.TitlePl));
            result.AddValue("STR_OD", new OracleParameter("Str_Od", OracleDbType.Long, article.PageBegin, ParameterDirection.InputOutput));
            result.AddValue("STR_DO", new OracleParameter("Str_Do", OracleDbType.Long, article.PageEnd, ParameterDirection.InputOutput));
            result.AddValue("ID_WYD_OBCE", new OracleParameter("Id_Wyd_Obce", OracleDbType.Long, article.AlienId, ParameterDirection.InputOutput));
            result.AddValue("JEZYK", new OracleParameter("Jezyk", article.Lang));

            return result;
        }

        #endregion
    }
}
