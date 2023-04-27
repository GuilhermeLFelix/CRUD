using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CRUD
{
    class Database
    {
        public static string StrCon()
        {
            return @"Data Source=;Initial Catalog=;User ID=;Password=";
        }
    }
}
