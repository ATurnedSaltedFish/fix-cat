using CrawlerNpro.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SQLite;

namespace CrawlerNpro.Toolkit
{
    public class SQLiteConn : ISQLConnection
    {
       public MySqlConnection GetMysqlConn()
        {
            throw new NotImplementedException();
        }

        public SQLiteConnection GetSQLiteConn()
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source= |DataDirectory|/temp.db;Version=3;");
            return conn;
        }
    }
}
