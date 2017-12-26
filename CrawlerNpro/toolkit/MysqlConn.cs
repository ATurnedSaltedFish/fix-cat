using CrawlerNpro.Interface;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CrawlerNpro.toolkit
{
   public  class MySqlConn:ISQLConnection
    {
        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回MySqlConnection对象</returns>
        public  MySqlConnection GetMysqlConn()
        {
            string strconn = "server=localhost;Port=3306;Uid=root;Pwd=asd123!@#;database=homefood;charset=utf8;";
            MySqlConnection MysqlConn = new MySqlConnection(strconn);
            return MysqlConn;
        }

        public SQLiteConnection GetSQLiteConn()
        {
            throw new NotImplementedException();
        }
    }
}
