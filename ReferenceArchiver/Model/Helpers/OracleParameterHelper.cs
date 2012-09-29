using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;

namespace ReferenceArchiver.Model.Helpers
{
    static class OracleParameterHelper
    {
        public static string GetStringForCommand(this OracleParameter paramter)
        {
            return string.Concat(":p", paramter.ParameterName);
        }
    }
}
