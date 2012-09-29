using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace ReferenceArchiver.Model.SqlBuilders
{
    class UpdateArticleCommand : SaveArticleCommandBase
    {
        public UpdateArticleCommand(DbCommand command, long articleId)
            : base(command)
        {
            ArticleId = articleId;
        }

        public override void AddValue(string databaseColumn, Oracle.DataAccess.Client.OracleParameter parameter)
        {
            throw new NotImplementedException();
        }

        public override bool Execute(System.Data.Common.DbTransaction transaction = null)
        {
            throw new NotImplementedException();
        }
    }
}
