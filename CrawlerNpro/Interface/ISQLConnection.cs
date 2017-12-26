using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.Interface
{
public  interface  ISQLConnection
    {
        MySqlConnection GetMysqlConn();

        SQLiteConnection GetSQLiteConn();
    }
}
