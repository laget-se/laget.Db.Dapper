using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace laget.Db.Dapper.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValid(this string query, out IEnumerable<string> errors)
        {
            using (var reader = new StringReader(query))
            {
                new TSql150Parser(initialQuotedIdentifiers: true).Parse(reader, out var parseErrors);

                errors = parseErrors.Select(err => err.Message).ToList();

                return !parseErrors.Any();
            }
        }
    }
}
