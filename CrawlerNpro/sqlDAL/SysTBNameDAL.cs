using CrawlerNpro.entity;
using CrawlerNpro.Interface;
using CrawlerNpro.toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.SqlDAL
{
  public  class SysTBNameDAL
    {
        ///java
        /// <summary>
        ///获取贴吧名称列表 插入到mysql数据库
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="listBarNameEntity"></param>
        /// <returns></returns>
        public int InsertTBName(List<BarNameEntity> listBarNameEntity)
        {
            ISQLConnection iSQLConnection = new MySqlConn();
            var sqlconn=  iSQLConnection.GetMysqlConn();
            int result = 0;
            string sql = "INSERT INTO homefood.bd_bar_name(BarName,Url,Type,Hot) VALUE(@BarName,@Url,@Type,@Hot)";
            try  
            {
                foreach (var item in listBarNameEntity)
                {
                    MySqlParameter barName = new MySqlParameter("@BarName", MySqlDbType.String);
                    barName.Value = item.BarName;

                    MySqlParameter url = new MySqlParameter("@Url", MySqlDbType.String);
                    url.Value = item.Url;//数据要设计

                    MySqlParameter type = new MySqlParameter("@Type", MySqlDbType.String);
                    type.Value = item.Type;

                    MySqlParameter hot = new MySqlParameter("@Hot", MySqlDbType.Int32);
                    hot.Value = item.Hot;

                    MySqlCommand cmd = new MySqlCommand(sql, sqlconn);
                    cmd.Parameters.Add(barName);
                    cmd.Parameters.Add(url);
                    cmd.Parameters.Add(type);
                    cmd.Parameters.Add(hot);
                    sqlconn.Open();
                    result = cmd.ExecuteNonQuery();
                    sqlconn.Close();
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------SysTBNameDAL error--------" + e);
            }
            return result;
        }

          
        /// java服务端编写 不安全方法
         /// <summary>
         /// 给mysql查询贴吧名称列表
         /// </summary>
         /// <param name="brNameEntity"></param>
         /// <returns></returns>
        public List<BarNameEntity> SelectTBName(string token=null)
        {
            ISQLConnection iSQLConnection = new MySqlConn();
            var sqlconn = iSQLConnection.GetMysqlConn();
            List<BarNameEntity> listBarNameEntity = new List<BarNameEntity>();
            try
            {
                string sql = @"SELECT BarName,Url,Type,Hot FROM homefood.bd_bar_name";
                MySqlCommand cmd = new MySqlCommand(sql, sqlconn);
                sqlconn.Open();
                MySqlDataReader DataReader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(DataReader);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    BarNameEntity barNameEntity = new BarNameEntity();
                    barNameEntity.BarName = Convert.ToString(dataRow["BarName"]);
                    barNameEntity.Url = Convert.ToString(dataRow["Url"]);
                    barNameEntity.Type = Convert.ToString(dataRow["Type"]);
                    barNameEntity.Hot = Convert.ToInt32(dataRow["Hot"]);
                    listBarNameEntity.Add(barNameEntity);
                }
                return listBarNameEntity;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------TBKeywordDAL error--------" + e);
            }
            return listBarNameEntity;
        }
    }
}
