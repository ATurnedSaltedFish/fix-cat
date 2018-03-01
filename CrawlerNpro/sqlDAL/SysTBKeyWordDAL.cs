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
#if DEBUG
namespace CrawlerNpro.sqlDAL
{
    public class SysTBKeyWordDAL
    {
        /// <summary>
        /// insert keyworld
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="listKeyWorldEntity"></param>
        /// <returns></returns>
        public int InsertSysKeyworld(List<KeyWorldEntity> listKeyWorldEntity)
        {
            ISQLConnection iSQLConnection = new MySqlConn();
            var sqlconn = iSQLConnection.GetMysqlConn();
            int result = 0;
            //KeyWorldEntity keyWorldEntity = new KeyWorldEntity();
            string sql = "INSERT INTO homefood.sys_key_world(KeyWorld,HotLeve) VALUE(@KeyWorld,@HotLeve)";
            try
            {
                foreach (var item in listKeyWorldEntity)
                {
                    MySqlParameter keyWorld = new MySqlParameter("@KeyWorld", MySqlDbType.String);
                    keyWorld.Value = item.KeyWorld;
                    MySqlParameter hotLeve = new MySqlParameter("@HotLeve", MySqlDbType.String);
                    hotLeve.Value = item.HotLeve;
                    MySqlCommand cmd = new MySqlCommand(sql, sqlconn);
                    cmd.Parameters.Add(keyWorld);
                    cmd.Parameters.Add(hotLeve);
                    sqlconn.Open();
                    result = cmd.ExecuteNonQuery();
                    sqlconn.Close();
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------TBKeywordDAL error--------" + e);
            }
            return result;
        }

        /// <summary>
        /// search keyworld
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="keyWorldEntity"></param>
        /// <returns></returns>
        public List<KeyWorldEntity> SelectSysKeyworld(KeyWorldEntity keyWorldEntity = null)
        {
            ISQLConnection iSQLConnection = new MySqlConn();
            var sqlconn = iSQLConnection.GetMysqlConn();
            List<KeyWorldEntity> listKeyWorldEntity = new List<KeyWorldEntity>();
            try
            {
                string sql = @"SELECT KeyWorld,HotLeve FROM homefood.sys_key_world where visiable=1";
                MySqlCommand cmd = new MySqlCommand(sql, sqlconn);
                sqlconn.Open();
                MySqlDataReader DataReader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(DataReader);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    KeyWorldEntity kwEntity = new KeyWorldEntity();

                    kwEntity.KeyWorld = Convert.ToString(dataRow["KeyWorld"]);

                    kwEntity.HotLeve = Convert.ToInt32(dataRow["HotLeve"]);

                    listKeyWorldEntity.Add(kwEntity);
                }
                return listKeyWorldEntity;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-------TBKeywordDAL error--------" + e);
            }
            return listKeyWorldEntity;
        }
#endif
    }
}
