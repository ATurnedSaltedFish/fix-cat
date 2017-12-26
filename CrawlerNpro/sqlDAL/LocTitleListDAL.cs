using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using CrawlerNpro.Interface;
using MySql.Data.MySqlClient;
using CrawlerNpro.Toolkit;
using System.Data;
using CrawlerNpro.entity;
using System.Diagnostics;

namespace CrawlerNpro.SqlDAL
{
   public class LocTitleListDAL
    {
        /// <summary>
        /// get localBarName
        /// </summary>
        /// <returns></returns>
      //  /// 分页功能
        public DataTable SelectLocalTBTitle()
        {
            ISQLConnection iSQLConnection = new SQLiteConn();
            string sql = @"SELECT id,title,url,uname,uid,replies,createCode,createtime,updatetime FROM tbtitlelist";
            var conn = iSQLConnection.GetSQLiteConn();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            conn.Open();
            SQLiteDataReader DataReader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(DataReader);
            if (dataTable==null)
            {
                return null;
            }
            return dataTable;
        }

        ///java
        /// <summary>
        /// insert barName   data form json file 
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="listBarNameEntity"></param>
        /// <returns></returns>
        public int InsertLocalTBTitle(List<TitleContentEntity> listTitleContentEntity)
        {
            ISQLConnection iSQLConnection = new SQLiteConn();
            var sqlconn = iSQLConnection.GetMysqlConn();
            int result = 0;
            string sql = "INSERT INTO tbtitlelist(title,url,uname,uid,replies,createcode,createtime) VALUE(@Title,@Url,@Uname,@Uid,@Replies,@Createcode,@Createtime)";
            try
            {
                foreach (var item in listTitleContentEntity)
                {
                    MySqlParameter barName = new MySqlParameter("@BarName", MySqlDbType.String);
                    barName.Value = item.Title;

                    MySqlParameter url = new MySqlParameter("@Url", MySqlDbType.String);
                    url.Value = item.Url;//数据要设计

                    MySqlParameter uname = new MySqlParameter("@Uname", MySqlDbType.String);
                    url.Value = item.Url;//数据要设计

                    MySqlParameter uid = new MySqlParameter("@Uid", MySqlDbType.String);
                    url.Value = item.Uid;//数据要设计

                    MySqlParameter replies = new MySqlParameter("@Replies", MySqlDbType.String);
                    url.Value = item.Replies;//数据要设计

                    MySqlParameter createcode = new MySqlParameter("@CreateCode", MySqlDbType.String);
                    url.Value = item.CreateCode;//数据要设计

                    MySqlParameter createtime = new MySqlParameter("@CreateTime", MySqlDbType.String);
                    url.Value = item.CreateTime;//数据要设计

                    MySqlCommand cmd = new MySqlCommand(sql, sqlconn);
                    cmd.Parameters.Add(barName);
                    cmd.Parameters.Add(url);
                    sqlconn.Open();
                    result = cmd.ExecuteNonQuery();
                    sqlconn.Close();
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------InsertLocalTBTitle error--------" + e);
            }
            return result;
        }

        
    }
}
