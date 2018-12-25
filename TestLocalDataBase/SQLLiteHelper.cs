using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Collections.Concurrent;

namespace TestLocalDataBase
{
    public sealed class SQLLiteHelper
    {
        private static readonly SQLLiteHelper helper = new SQLLiteHelper();
        string connString = "Data Source=" + @"C:\嵌入式数据库测试\SQLLite.db";
        private SQLiteConnection conn = null;

        private SQLLiteHelper()
        {
            conn = new SQLiteConnection(connString);
        }
        public static SQLLiteHelper GetInstance()
        {
            return helper;
        }
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        public void Connect()
        {
            conn.Open();
        }

        public int Insert(string[] sql)
        {

            try
            {
                ConcurrentBag<int> rowCount = new ConcurrentBag<int>();
                using (SQLiteTransaction trans = conn.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.Transaction = trans;
                        try
                        {
                            foreach (var item in sql)
                            {
                                cmd.CommandText = item;
                                int count = cmd.ExecuteNonQuery();
                                rowCount.Add(count);
                            }
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                        }
                    } 
                }
                return rowCount.Sum();
            }
            catch { return -1; }

        }

        //public int Insert(string[] sql)
        //{
        //    try
        //    {
        //        ConcurrentBag<int> rowCount = new ConcurrentBag<int>();

        //        Parallel.ForEach(sql, (item, loopstate) =>
        //        {
        //            SQLiteCommand cmd = new SQLiteCommand(item, conn);
        //            int count = cmd.ExecuteNonQuery();
        //            rowCount.Add(count);
        //        });
        //        return rowCount.Sum();
        //    }
        //    catch { return -1; }
        //}
        public int Query(string sql)
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();
                int rowCount = 0;
                while (reader.Read())
                {
                    rowCount++;
                }
                return rowCount;
            }
            catch { return -1; }
        }






    }
}
