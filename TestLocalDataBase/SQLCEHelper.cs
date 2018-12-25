
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using System.Collections.Concurrent;
using System.Threading;

namespace TestLocalDataBase
{



    public class SQLCEHelper
    {
        private static SQLCEHelper helper;
        private static readonly object obj = new object();

        string connString = "Data Source=" + @"C:\嵌入式数据库测试\SQLCE4.0.sdf;Password=123;";

        //string connString = "Data Source=" + @"C:\嵌入式数据库测试\SQLCE3.5.sdf;Password=123;";



        private SqlCeConnection conn = null;

        private SQLCEHelper()
        {
            conn = new SqlCeConnection(connString);
        }


        public static SQLCEHelper GetInstance()
        {
            if (helper == null)
            {
                lock (obj)
                {
                    if (helper == null)
                    {
                        helper = new SQLCEHelper();
                    }
                }
            }

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

        public void DisConnect()
        {
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }
        }


        public int ExecuteSQL(string[] sql)
        {
            try
            {
                ConcurrentBag<int> rowCount = new ConcurrentBag<int>();
                using (SqlCeTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCeCommand cmd = conn.CreateCommand())
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
         
        public int Query(string sql)
        {
            try
            {
                SqlCeCommand cmd = new SqlCeCommand(sql, conn);
                SqlCeDataReader reader = cmd.ExecuteReader();
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
