using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;

namespace Activity3_DeDios
{
    public static class database
    {
        public static readonly string DBpath =
            Path.GetFullPath(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"..\..\imsdb.accdb"
                    )
                );

        // FIXED: Changed "Mircrosoft" to "Microsoft" and "16.0" is correct.
        private static readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + DBpath + ";Persist Security Info=False;";

        public static OleDbConnection GetDbConnection()
        {
            return new OleDbConnection(connectionString);
        }
    }
}