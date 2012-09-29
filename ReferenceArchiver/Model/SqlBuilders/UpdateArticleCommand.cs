using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Oracle.DataAccess.Client;
using ReferenceArchiver.Model.Helpers;

namespace ReferenceArchiver.Model.SqlBuilders
{
    class UpdateArticleCommand : SaveArticleCommandBase
    {
        #region Fields

        private List<string> _assignments = new List<string>();

        #endregion

        #region Constructors

        public UpdateArticleCommand(DbCommand command, long articleId)
            : base(command)
        {
            ArticleId = articleId;
        }

        #endregion

        #region Methods

        public override void AddValue(string databaseColumn, OracleParameter parameter)
        {
            _assignments.Add(string.Format("{0} = {1}", databaseColumn, parameter.GetStringForCommand()));
            Command.Parameters.Add(parameter);
        }

        public override bool Execute(DbTransaction transaction = null)
        {
            if (transaction != null)
                Command.Transaction = transaction;

            Command.CommandText = string.Format(
                @"UPDATE filo.artykuly
                SET {0}
                WHERE ID = :pId",
                string.Join(",", _assignments));

            Command.Parameters.Add(new OracleParameter("Id", ArticleId));

            return Command.ExecuteNonQuery() >= 1;
        }

        #endregion
    }
}
