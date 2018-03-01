using CrawlerNpro.Interface;
using CrawlerNpro.toolkit;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.ServiceInput
{
    /// <summary>
    /// mysql 逻辑
    /// </summary>
    public class TBTitleListService
    {
        /// <summary>
        /// Get bar 
        /// </summary>
        /// <param name="listTitleListEntity"></param>
        public void InsertTBTItleList(List<ResultEntity> listTitleListEntity)
        {
            ISQLConnection iSQLConnection = new MySqlConn();
            var sqlconn = iSQLConnection.GetMysqlConn();
            ResultEntity titleListEntity = new ResultEntity();
            string sql = "INSERT INTO homefood.bdtiebalist(Title,Url,Uname,Uid,Replies,CreateCode) VALUE(@Title,@Url,@Uname,@Uid,@Replies,@CreateCode)";
            try
            {
                foreach (var item in listTitleListEntity)
                {
                    MySqlParameter title = new MySqlParameter("@Title", MySqlDbType.String);
                    title.Value = item.Title;

                    MySqlParameter url = new MySqlParameter("@Url", MySqlDbType.String);
                    url.Value = item.Url;

                    MySqlParameter uname = new MySqlParameter("@Uname", MySqlDbType.String);
                    uname.Value = "isnull";

                    MySqlParameter uid = new MySqlParameter("@Uid", MySqlDbType.String);
                    uid.Value = "isnull";

                    MySqlParameter replies = new MySqlParameter("@Replies", MySqlDbType.String);
                    replies.Value = item.Replies;

                    MySqlParameter CreateCode = new MySqlParameter("@CreateCode", MySqlDbType.String);
                    CreateCode.Value = "15652123testcode";

                    MySqlCommand cmd = new MySqlCommand(sql, sqlconn);
                    cmd.Parameters.Add(title);
                    cmd.Parameters.Add(url);
                    cmd.Parameters.Add(uname);
                    cmd.Parameters.Add(uid);
                    cmd.Parameters.Add(replies);
                    cmd.Parameters.Add(CreateCode);
                    sqlconn.Open();
                    cmd.ExecuteNonQuery();
                    sqlconn.Close();
                };
            }
            catch(Exception e)
            {
                Debug.WriteLine("-------TBTitleListService error--------"+e);
            }
        }
    }
}
