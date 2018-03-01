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
using System.Threading;

namespace CrawlerNpro.SqlDAL
{
    public class LocTitleListDAL
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

        ///分页功能 未测试 添加标识
        /// <summary>
        /// 查询贴吧标题
        /// </summary>
        /// <param name="rows">返回行数</param>
        /// <param name="startIndex">起始行号</param>
        /// <returns></returns>
        public DataTable SelectLocalTBTitle(string rows = "50", string startIndex = "0")
        {
            //ISQLConnection iSQLConnection = new SQLiteConn();
            string sql = @"SELECT id,title,url,uname,uid,replies,createcode,createtime,updatetime FROM tbtitlelist ORDER BY  id AND visiable=1  LIMIT " + rows + " OFFSET " + startIndex + "";
            var conn = SQLiteConn.GetSQLiteConn();//iSQLConnection.GetSQLiteConn();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            conn.Open();
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(dataReader);
            if (dataTable == null)
            {
                return null;
            }
            return dataTable;
        }

        ///分页功能 未测试 添加标识
        /// <summary>
        /// 查询贴吧标题 
        /// </summary>
        /// <param name="rows">返回行数</param>
        /// <param name="startIndex">起始行号</param>
        /// <returns></returns>
        public List<ResultEntity> SelectLocalTitleList(string rows = "50", string startIndex = "0")
        {
            List<ResultEntity> listResultEntity = new List<ResultEntity>();
            //ISQLConnection iSQLConnection = new SQLiteConn();
            //string sql = @"SELECT id,title,url,uname,uid,replies,createcode,createtime,updatetime FROM tbtitlelist ORDER BY id AND visiable=1 LIMIT " + rows + " OFFSET " + startIndex + "";
            string sql = @"SELECT id,title,url,uname,uid,replies,createcode,createtime,updatetime FROM tbtitlelist WHERE visiable=1 LIMIT " + rows + " OFFSET " + startIndex + "";
            var conn = SQLiteConn.GetSQLiteConn();//iSQLConnection.GetSQLiteConn();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            conn.Open();
            Monitor.Enter(obj);
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            if (dataReader==null)
            {
                return null;
            }
            while (dataReader.Read())
            {
                ResultEntity resultEntity = new ResultEntity();
                resultEntity.Id = (int)dataReader["id"];
                resultEntity.Title = dataReader["title"].ToString();
                resultEntity.Url = dataReader["url"].ToString();
                resultEntity.Uname = dataReader["uname"].ToString();
                resultEntity.Uid = dataReader["uid"].ToString();
                resultEntity.Intreplies = Convert.ToInt32(dataReader["replies"]);
                resultEntity.CreateCode = dataReader["createcode"].ToString();
                resultEntity.CreateTime = dataReader["createtime"].ToString();
                resultEntity.UpdateTime = dataReader["updatetime"].ToString();
                listResultEntity.Add(resultEntity);
            }
            Monitor.Exit(obj);//修改
            dataReader.Close();
            conn.Close();
            conn.Dispose();
            return listResultEntity;
        }

        ///未测试
        /// <summary>
        /// 删除贴吧数据
        /// </summary>
        /// <param name="strid"></param>
        /// <returns>0传入数据为空，1插入成功，2部分数据插入失败 </returns>
        public int DeleteLocalTBTitle(string[] strid)
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

        ///java  // 测试通过
        /// <summary>
        /// 插入贴吧标题 到本地数据
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="listBarNameEntity"></param>
        /// <returns></returns>2
        public int InsertLocalTBTitle(List<ResultEntity> listResultEntity)
        {
            //  ISQLConnection iSQLConnection = new SQLiteConn();
            // var sqlconn = iSQLConnection.GetSQLiteConn();
            //var sqlconn = SQLiteConn.GetSQLiteConn();
            int rowids = GetSqliteRowId();
            string strsql = "INSERT INTO tbtitlelist(id,title,url,uname,uid,replies,createcode,icode,visiable,createtime) VALUES(@Id,@Title,@Url,@Uname,@Uid,@Replies,@Createcode,@Icode,@Visiable,@Createtime);select last_insert_rowid();";
            foreach (var item in listResultEntity)
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

                    new SQLiteParameter("@Title", DbType.String),

                    new SQLiteParameter("@Url", DbType.String),

                    new SQLiteParameter("@Uname", DbType.String),

                    new SQLiteParameter("@Uid", DbType.String),

                    new SQLiteParameter("@Replies", DbType.String),

                    new SQLiteParameter("@CreateCode", DbType.String),

                    new SQLiteParameter("@Icode", DbType.Int32),

                    new SQLiteParameter("@Visiable", DbType.Int32),

                    new SQLiteParameter("@CreateTime", DbType.DateTime)
                };
                parameters[0].Value = rowids;
                Debug.WriteLine("==rowids====" + rowids + "=====");
                parameters[1].Value = item.Title;
                parameters[2].Value = item.Url;
                parameters[3].Value = item.Uname;
                parameters[4].Value = item.Uid;
                parameters[5].Value = item.Replies;
                parameters[6].Value = item.CreateCode;
                parameters[7].Value = item.Icode;
                if (item.Visiable==0)
                {
                    item.Visiable = 1;
                }
                parameters[8].Value = item.Visiable;//默认插入为1
                parameters[9].Value = item.CreateTime;
                var data = SqliteHelper.ExecuteScalar(strsql, CommandType.Text, parameters);
                Debug.WriteLine("==data====" + data + "=====");
                Intreback = data + 1;
                rowids = -1;
                if (data == 0)
                {
                    Debug.WriteLine("-------------InsertLocalTBTitle ERROR-----------"); ;

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
            return Intreback;
        }

        ///java  // 测试通过
        /// <summary>
        /// 插入贴吧标题 到本地数据
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="listBarNameEntity"></param>
        /// <returns></returns>2
        public int InsertLocalTBTitle(ResultEntity resultEntity)
        {
            //  ISQLConnection iSQLConnection = new SQLiteConn();
            // var sqlconn = iSQLConnection.GetSQLiteConn();
            //var sqlconn = SQLiteConn.GetSQLiteConn();
            int rowids = GetSqliteRowId();
            string strsql = "INSERT INTO tbtitlelist(id,title,url,uname,uid,replies,createcode,visiable,createtime) VALUES(@Id,@Title,@Url,@Uname,@Uid,@Replies,@Createcode,@Visiable,@Createtime);select last_insert_rowid();";
      
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

                    new SQLiteParameter("@Title", DbType.String),

                    new SQLiteParameter("@Url", DbType.String),

                    new SQLiteParameter("@Uname", DbType.String),

                    new SQLiteParameter("@Uid", DbType.String),

                    new SQLiteParameter("@Replies", DbType.String),

                    new SQLiteParameter("@CreateCode", DbType.String),

                    new SQLiteParameter("@Visiable", DbType.Int32),

                    new SQLiteParameter("@CreateTime", DbType.DateTime)
                };
                parameters[0].Value = rowids;
                Debug.WriteLine("==rowids====" + rowids + "=====");
                parameters[1].Value = resultEntity.Title;
                parameters[2].Value = resultEntity.Url;
                parameters[3].Value = resultEntity.Uname;
                parameters[4].Value = resultEntity.Uid;
                parameters[5].Value = resultEntity.Replies;
                parameters[6].Value = resultEntity.CreateCode;
                parameters[7].Value = resultEntity.Visiable;//默认插入为1
                parameters[8].Value = resultEntity.CreateTime;
                var data = SqliteHelper.ExecuteScalar(strsql, CommandType.Text, parameters);
                Debug.WriteLine("==data====" + data + "=====");
                Intreback = data + 1;
               // rowids = -1;
                if (data == 0)
                {
                    Debug.WriteLine("-------------InsertLocalTBTitle ERROR-----------"); ;

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
            return Intreback;
        }


        /// <summary>
        /// 查询贴吧标题 Icode是否存在 
        /// </summary>
        /// <param name="resultEntity"></param>
        /// <returns></returns>
        public bool SelectCreateCode(ResultEntity resultEntity)
        {
            string sql = "SELECT id FROM tbtitlelist WHERE createcode=" + resultEntity.CreateCode + "";
            var conn = SQLiteConn.GetSQLiteConn();//iSQLConnection.GetSQLiteConn();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            conn.Open();
            Monitor.Enter(obj);
            var dataReader = cmd.ExecuteReader();
           
            if (dataReader.HasRows ==false)
            {
                return false;
            }
            dataReader.Read();
            if (dataReader["id"]==null)//id不存在
            {
                return false;
            }
                return true;
        }

        //未测试
        /// <summary>
        /// 更新贴吧标题
        /// </summary>
        /// <param name="listResultEntity"></param>
        /// <returns>1部分完成,2全部完成,0失败</returns>
        public int UpdateLocalTBTitle(List<ResultEntity> listResultEntity)
        {
            foreach (var item in listResultEntity)
            {
                string strsql = "UPDATE tbtitlelist SET replies=@Replies,searchTimes=@SearchTimes,visiable=1  WHERE createcode=" + item.CreateCode + "";
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
                parameters[0].Value = item.Replies;
                parameters[1].Value = (Convert.ToInt32(item.SearchTimes) + 1).ToString();
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

        public int UpdateLocalTBTitle(ResultEntity resultEntity)
        {
            string strsql = "UPDATE tbtitlelist SET replies=@Replies,searchTimes=@SearchTimes,visiable=1  WHERE createcode=" + resultEntity.CreateCode + "";
            SQLiteParameter[] parameters = {
                    new SQLiteParameter("@Replies", DbType.String),
                    new SQLiteParameter("@SearchTimes", DbType.String),
                };
            //parameters[0].Value = rowids;
            parameters[0].Value = resultEntity.Replies;
            parameters[1].Value = (Convert.ToInt32(resultEntity.SearchTimes) + 1).ToString();
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
        private int GetSqliteRowId()
        {
            try
            {
                //ISQLConnection iSQLConnection = new SQLiteConn();
                string sql = @"SELECT id FROM tbtitlelist ORDER BY id DESC LIMIT 0,1;";
                var conn = SQLiteConn.GetSQLiteConn();//iSQLConnection.GetSQLiteConn();
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                conn.Open();
                Monitor.Enter(obj);
                SQLiteDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows==false)
                {
                    return 0;//id为1
                }
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

        /// <summary>
        /// 查询总行数数据
        /// </summary>
        /// <returns></returns>
        public int SelectAllCount()
        {
            //ISQLConnection iSQLConnection = new SQLiteConn();
            string sql = @"select count(*)from tbtitlelist";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strid"></param>
        /// <returns>0 失败, 2部分错误 ，1完成 </returns>
        public int DelUpdateTBTitle(string[] strid)
        {
            int result = 0;
            var num = strid.Length;
            if (num < 1)
            {
                return 0;
            }
            foreach (var item in strid)
            {
                string sql = "UPDATE tbtitlelist SET visiable='0' WHERE id = " + item + ";";
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
