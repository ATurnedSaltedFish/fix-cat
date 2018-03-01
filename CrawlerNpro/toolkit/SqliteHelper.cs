using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlerNpro.Toolkit
{
    public static class SqliteHelper
    {

        private static readonly object obj = new object();
        /// <summary>
        /// executes a SQL and returns the number of rows affected used for update/insert
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="cmdType"></param>
        /// <param name="paramater"></param>
        /// <returns></returns>
        public static int ExecuteNoQuery(string strsql, CommandType cmdType, params SQLiteParameter[] paramater)
        {
            try
            {
                var conn = SQLiteConn.GetSQLiteConn();
                SQLiteCommand cmd = new SQLiteCommand();
                PrepareCommand(cmd, conn, cmdType, strsql, paramater,null);
                Monitor.Enter(obj);
                int result = cmd.ExecuteNonQuery();
                Monitor.Exit(obj);
                conn.Close();
                conn.Dispose();
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-----------clsSqliteHelper------funExecuteNoQuery----------------------");
                return 0;
            }
        }

        public static int ExecuteScalar(string strsql, CommandType cmdType, params SQLiteParameter[] paramater)
        {
            try
            {
                var conn = SQLiteConn.GetSQLiteConn();
                SQLiteCommand cmd = new SQLiteCommand();
                PrepareCommand(cmd, conn, cmdType, strsql, paramater, null);
                Monitor.Enter(obj);
                var result = Convert.ToInt32(cmd.ExecuteScalar());
                Monitor.Exit(obj);
                conn.Close();
                conn.Dispose();
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine("-----------clsSqliteHelper------funExecuteNoQuery----------------------");
                return 0;
            }
        }

        private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, CommandType cmdType, string strsql, SQLiteParameter[] cmdParms, SQLiteTransaction trans = null)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = strsql;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
