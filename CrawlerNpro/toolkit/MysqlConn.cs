using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.toolkit
{
   public static class MySqlConn
    {
        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回MySqlConnection对象</returns>
        public static MySqlConnection GetMysqlConn()
        {
            string strconn = "server=localhost;Port=3306;Uid=root;Pwd=asd123!@#;database=homefood;charset=utf8;";
            MySqlConnection MysqlConn = new MySqlConnection(strconn);
            return MysqlConn;
        }
    }
}
