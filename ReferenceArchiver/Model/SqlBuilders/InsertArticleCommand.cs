using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Oracle.DataAccess.Client;
using ReferenceArchiver.Model.Helpers;
using Oracle.DataAccess.Types;
using System.Data;

namespace ReferenceArchiver.Model.SqlBuilders
{
    class InsertArticleCommand : SaveArticleCommandBase
    {
        #region Fields

        private List<string> _databaseColumns = new List<string>();
        private List<string> _values = new List<string>();

        #endregion

        #region Constructors

        public InsertArticleCommand(DbCommand command)
            : base(command)
        { }

        #endregion

        #region Methods

        public override void AddValue(string databaseColumn, OracleParameter parameter)
        {
            _databaseColumns.Add(databaseColumn);
            _values.Add(parameter.GetStringForCommand());
            Command.Parameters.Add(parameter);
        }

        public override bool Execute(DbTransaction transaction = null)
        {
            string columnsString = string.Join(",", _databaseColumns);
            string valuesString = string.Join(",", _values);

            Command.CommandText = string.Format(
                "INSERT INTO filo.ARTYKULY ( {0} ) " +
                "VALUES ( {1} )" +
                "RETURNING ID INTO :pNewArticleId",
                columnsString,
                valuesString);

            Command.Parameters.Add(new OracleParameter("NewArticleId", OracleDbType.Decimal, ParameterDirection.ReturnValue));

            if (Command.ExecuteNonQuery() < 1)
                return false;

            OracleDecimal idInOracleType = (OracleDecimal)Command.Parameters["NewArticleId"].Value;
            ArticleId = (long)idInOracleType.Value;

            return true;
        }

        #endregion
    }
}
