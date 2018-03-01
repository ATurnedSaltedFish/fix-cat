using CrawlerNpro.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Data;
using System.Threading;

namespace CrawlerNpro.Toolkit
{
    public class SQLiteConn
    {
        public MySqlConnection GetMysqlConn()
        {
            throw new NotImplementedException();
        }

        public static SQLiteConnection GetSQLiteConn()
        {
            //SQLiteConnection conn = new SQLiteConnection("Data Source= |DataDirectory|/temp.db;Version=3;");
            //var filePath = @"/CrawlerNproData/Resource/temp.db";
            //Uri uri = new Uri("pack://application:,,," + filePath, UriKind.Absolute);
            // var dd = uri.ToString();
            //var path = "Data Source=" + dd;
            var path = @"Data Source=E:\CrawlerNproData\temp.db";
            SQLiteConnection conn = new SQLiteConnection(path);
            //SQLiteConnection conn = new SQLiteConnection(@"Data Source=E:\CrawlerNproData\temp.db");
            return conn;
        }
    }
}