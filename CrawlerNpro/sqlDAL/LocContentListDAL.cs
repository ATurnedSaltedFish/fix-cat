using CrawlerNpro.entity;
using CrawlerNpro.Toolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerNpro.SqlDAL
{
    public class LocContentListDAL
    {
        private static readonly object obj = new object();
        /// <summary>
        /// 插入数据返回当前rowsid
        /// </summary>
        private int intreback;
        public int Intreback
        {
            get
            {
                return intreback;
            }

            set
            {
                intreback = value;
            }
        }

        //修改好未测试
        /// <summary>
        /// 查询sqlite内容
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public DataTable SelectLocalContent(string rows = "50", string startIndex = "0")
        {
            //ISQLConnection iSQLConnection = new SQLiteConn();
            string sql = @"SELECT id,content,titlecreatetime,replytime,floor,replynum,pagenum,createcode,createtime,updatetime FROM tbcontext WHERE visiable=1  LIMIT " + rows + " OFFSET " + startIndex + "";
            var conn = SQLiteConn.GetSQLiteConn();//iSQLConnection.GetSQLiteConn();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            conn.Open();
            Monitor.Enter(obj);
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            Monitor.Exit(obj);
            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);
            if (dataTable == null)
            {
                return null;
            }
            return dataTable;
        }
        //修改好 未测试
        /// <summary>
        /// 查询tb内容返回集合
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public List<ContentEntity> SelectLocalContentList(string rows = "50", string startIndex = "0")
        {
            List<ContentEntity> listContentEntity = new List<ContentEntity>();

            //ISQLConnection iSQLConnection = new SQLiteConn();
            //string sql = @"SELECT id,title,url,uname,uid,replies,createcode,createtime,updatetime FROM tbtitlelist ORDER BY id AND visiable=1 LIMIT " + rows + " OFFSET " + startIndex + "";
            string sql = @"SELECT id,content,titlecreatetime,replytime,floor,replynum,pagenum,createcode,createtime,updatetime FROM tbcontext WHERE visiable=1  LIMIT" + rows + " OFFSET " + startIndex + "";
            var conn = SQLiteConn.GetSQLiteConn();//iSQLConnection.GetSQLiteConn();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            conn.Open();
            Monitor.Enter(obj);
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                ContentEntity contentEntity = new ContentEntity();
                contentEntity.Id = (int)dataReader["id"];
                contentEntity.Content = dataReader["content"].ToString();
                contentEntity.TitleCreateTime = dataReader["titlecreatetime"].ToString();
                contentEntity.ReplyTime = dataReader["replytime"].ToString();
                contentEntity.Floor = dataReader["floor"].ToString();
                contentEntity.ReplyNum = dataReader["replynum"].ToString();
                contentEntity.PageNum = dataReader["pagenum"].ToString();
                contentEntity.CreateCode = (int)dataReader["createcode"];
                contentEntity.CreateTime = dataReader["createtime"].ToString();
                contentEntity.UpdateTime = dataReader["updatetime"].ToString();
                listContentEntity.Add(contentEntity);
            }
            Monitor.Exit(obj);
            dataReader.Close();
            conn.Close();
            conn.Dispose();
            return listContentEntity;
        }

        ///未测试
        /// <summary>
        /// 删除贴吧数据
        /// </summary>
        /// <param name="strid"></param>
        /// <returns>0传入数据为空，1插入成功，2部分数据插入失败 </returns>
        public int DeleteLocalContent(string[] strid)
        {
            int result = 0;
            var num = strid.Length;
            if (num < 1)
            {
                return 0;
            }
            foreach (var item in strid)
            {
                string sql = "DELETE FROM tbtitlelist WHERE id = " + item + ";";
                SQLiteParameter[] parameters = {
                    new SQLiteParameter("Id", DbType.Int32)
            };
                result = SqliteHelper.ExecuteNoQuery(sql, CommandType.Text, parameters);
            }
            var data = +result;
            if (data == num)
            {
                return 1;
            }
            return 2;
        }

        //修改好 未测试
        ///java  // 测试通过
        /// <summary>
        /// 插入贴吧标题 到本地数据
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="listBarNameEntity"></param>
        /// <returns></returns>
        public int InsertLocalContent(List<ContentEntity> listContentEntity)
        {
            //  ISQLConnection iSQLConnection = new SQLiteConn();
            // var sqlconn = iSQLConnection.GetSQLiteConn();
            //var sqlconn = SQLiteConn.GetSQLiteConn();
            int rowids = GetSqliteRowId();
            string strsql = "INSERT INTO tbtitlelist(id,content,titlecreatetime,replytime,floor,replynum,createcode,createtime，updatetime) VALUES(@Id,@Title,@Url,@Uname,@Uid,@Replies,@Createcode,@Createtime);select last_insert_rowid();";
            foreach (var item in listContentEntity)
            {
                if (rowids == 0)
                {
                    rowids = 1;
                }
                else if (Intreback == 2 || rowids == 1)
                {
                    rowids = 2;
                }
                else if (rowids > 1)
                {
                    rowids = rowids + 1;
                }
                else
                {
                    rowids = Intreback;
                }
                SQLiteParameter[] parameters = {
                    new SQLiteParameter("Id", DbType.Int32),

                    new SQLiteParameter("@Content", DbType.String),

                    new SQLiteParameter("@Titlecreatetime", DbType.String),

                    new SQLiteParameter("@Replytime", DbType.String),

                    new SQLiteParameter("@Floor", DbType.String),

                    new SQLiteParameter("@Replynum", DbType.String),

                    new SQLiteParameter("@CreateCode", DbType.String),

                    new SQLiteParameter("@CreateTime", DbType.DateTime),

                    new SQLiteParameter("@Updatetime", DbType.DateTime)
                };
                parameters[0].Value = rowids;
                Debug.WriteLine("==rowids====" + rowids + "=====");
                parameters[1].Value = item.Content;
                parameters[2].Value = item.TitleCreateTime;
                parameters[3].Value = item.ReplyTime;
                parameters[4].Value = item.Floor;
                parameters[5].Value = item.ReplyNum;
                parameters[6].Value = item.CreateCode;
                parameters[7].Value = item.CreateTime;
                parameters[8].Value = item.UpdateTime;
                var data = SqliteHelper.ExecuteScalar(strsql, CommandType.Text, parameters);
                Debug.WriteLine("==data====" + data + "=====");
                Intreback = data + 1;
                rowids = -1;
                if (data == 0)
                {
                    Debug.WriteLine("-------------InsertLocalTBTitle ERROR-----------"); ;
                    return 0;
                }
                //SQLiteCommand cmd = new SQLiteCommand(sql, sqlconn);
                //cmd.Parameters.Add(id);
                //cmd.Parameters.Add(title);
                //cmd.Parameters.Add(url);
                //cmd.Parameters.Add(uname);
                //cmd.Parameters.Add(uid);
                //cmd.Parameters.Add(replies);
                //cmd.Parameters.Add(createcode);
                //cmd.Parameters.Add(createtime);
                //sqlconn.Open();
                //rowids = cmd.ExecuteNonQuery();
                //sqlconn.Close();
                //};
            }
            return 2;
        }

        //不使用
        /// <summary>
        /// 更新贴吧标题
        /// </summary>
        /// <param name="listResultEntity"></param>
        /// <returns></returns>
        public int UpdateLocalContent(List<ContentEntity> listResultEntity)
        {
            foreach (var item in listResultEntity)
            {
                string strsql = "UPDATE tbtitlelist SET replies=@Replies,searchTimes=@SearchTimes  WHERE createcode=" + item.CreateCode + "";
                //if (rowids == 0)
                //{
                //    rowids = 1;
                //}
                //else if (Intreback == 2 || rowids == 1)
                //{
                //    rowids = 2;
                //}
                //else if (rowids > 1)
                //{
                //    rowids = rowids + 1;
                //}
                //else
                //{
                //    rowids = Intreback;
                //}
                SQLiteParameter[] parameters = {
                    new SQLiteParameter("@Replies", DbType.String),
                    new SQLiteParameter("@SearchTimes", DbType.String),
                };
                //parameters[0].Value = rowids;
                parameters[0].Value = item.ReplyNum;//...
                parameters[1].Value = (Convert.ToInt32(item.ReplyNum) + 1).ToString();//...
                var result = SqliteHelper.ExecuteScalar(strsql, CommandType.Text, parameters);
                if (result == 0)
                {
                    Debug.WriteLine("-------------InsertLocalTBTitle ERROR-----------");
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            return 1;
        }

        //修改完成 未测试
        /// <summary>
        /// 获取表id
        /// </summary>
        /// <returns></returns>
        private int GetSqliteRowId()
        {
            try
            {
                //ISQLConnection iSQLConnection = new SQLiteConn();
                string sql = @"SELECT id FROM tbcontext ORDER BY id DESC LIMIT 0,1;";
                var conn = SQLiteConn.GetSQLiteConn();//iSQLConnection.GetSQLiteConn();
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                conn.Open();
                Monitor.Enter(obj);
                SQLiteDataReader dataReader = cmd.ExecuteReader();
                dataReader.Read();
                int id = (int)dataReader["id"];
                Monitor.Exit(obj);
                dataReader.Close();
                conn.Close();
                conn.Dispose();
                return id;
            }
            catch (Exception e)
            {
                return 0;
                //Debug.WriteLine("-----clsLocTitleListDAL---GetSqliteRowId----------");
            }
        }

        //未使用
        /// <summary>
        /// 获取最后一行数据
        /// </summary>
        /// <returns></returns>
        private int GetLastRowId()
        {
            try
            {
                //ISQLConnection iSQLConnection = new SQLiteConn();
                string sql = @"select last_insert_rowid();";
                var conn = SQLiteConn.GetSQLiteConn();//iSQLConnection.GetSQLiteConn();
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                conn.Open();
                Monitor.Enter(obj);
                SQLiteDataReader dataReader = cmd.ExecuteReader();
                dataReader.Read();
                int id = (int)dataReader["id"];
                Monitor.Exit(obj);
                dataReader.Close();
                conn.Close();
                conn.Dispose();
                return id;
            }
            catch (Exception e)
            {
                return 0;
                //Debug.WriteLine("-----clsLocTitleListDAL---GetSqliteRowId----------");
            }
        }

        //修改好未测试
        /// <summary>
        /// 查询总行数数据
        /// </summary>
        /// <returns></returns>
        public int SelectAllCount()
        {
            //ISQLConnection iSQLConnection = new SQLiteConn();
            string sql = @"select count(*)from tbcontext";
            var conn = SQLiteConn.GetSQLiteConn();//iSQLConnection.GetSQLiteConn();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            conn.Open();
            var data = cmd.ExecuteScalar();

            if (data == null)
            {
                return 0;
            }
            return Convert.ToInt32(data);
        }

        //修改好未测试
        /// <summary>
        /// 业务删除内容（更新）
        /// </summary>
        /// <param name="strid"></param>
        /// <returns>0 失败, 2部分错误 ，1完成 </returns>
        public int DelUpdateContente(string[] strid)
        {
            int result = 0;
            var num = strid.Length;
            if (num < 1)
            {
                return 0;
            }
            foreach (var item in strid)
            {
                string sql = "UPDATE tbcontext SET visiable='0' WHERE id = " + item + ";";
                SQLiteParameter[] parameters = {
                    new SQLiteParameter("Id", DbType.Int32)
            };
                result = SqliteHelper.ExecuteNoQuery(sql, CommandType.Text, parameters);
            }
            var data = +result;
            if (data == num)
            {
                return 1;
            }
            return 2;
        }
    }
}
