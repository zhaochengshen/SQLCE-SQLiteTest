using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace TestLocalDataBase
{

    public class KeyValue
    {
        int rowCount;
        long time;

        public int RowCount { get => rowCount; set => rowCount = value; }
        public long Time { get => time; set => time = value; }

    }
    public partial class Form1 : Form
    {
        List<KeyValue> SqlCEInsertList = new List<KeyValue>();
        List<KeyValue> SqlCEQueryList = new List<KeyValue>();
        List<KeyValue> SqlCEQueryWhereList = new List<KeyValue>();
        List<KeyValue> SqlCEUpdateList = new List<KeyValue>();
        List<KeyValue> SqlCEDeleteList = new List<KeyValue>();

        List<KeyValue> SqliteInsertList = new List<KeyValue>();
        List<KeyValue> SqliteQueryList = new List<KeyValue>();
        List<KeyValue> SqliteQueryWhereList = new List<KeyValue>();
        List<KeyValue> SqliteUpdateList = new List<KeyValue>();
        List<KeyValue> SqliteDeleteList = new List<KeyValue>();

        SQLLiteHelper sqliterhelper = SQLLiteHelper.GetInstance();

        bool AutoTestFlag = false;


        public Form1()
        {
            InitializeComponent();
        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            try
            {
                sqliterhelper.Connect();
                SQLCEHelper.GetInstance().Connect();
                WriteLog("数据库连接成功");
            }
            catch (Exception ex)
            {
                WriteLog("连接失败！" + ex.Message);
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                InsertData();
            });
        }

        public void InsertData()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string[] sql = new string[int.Parse(txtNum.Text)];

            Parallel.For(10000000, sql.Count(), item =>
            {
                sql[item] = " insert into testtable(id, name, age, address) values(" + item + ", '张三" + item + "', '" + item + "', '地址" + item + "') ";

            });
            stopwatch.Stop();
            WriteLog("循环添加sql耗费时间：" + stopwatch.ElapsedMilliseconds + "毫秒");



            //stopwatch.Restart();
            //int rowCount = SQLCE40Helper.GetInstance().Insert(sql);
            //stopwatch.Stop();
            //WriteLog("SQLCE添加数据 " + rowCount + " 行执行时间：" + stopwatch.ElapsedMilliseconds + " 毫秒");

            //if (AutoTestFlag)
            //{
            //    SqlCEInsertList.Add(new KeyValue { RowCount = rowCount, Time = stopwatch.ElapsedMilliseconds });
            //}

            int rowCount = 0;
            stopwatch.Restart();
            rowCount = sqliterhelper.Insert(sql);
            stopwatch.Stop();
            WriteLog("SQLite添加数据 " + rowCount + " 行执行时间：" + stopwatch.ElapsedMilliseconds + " 毫秒");

            if (AutoTestFlag)
            {
                SqliteInsertList.Add(new KeyValue { RowCount = rowCount, Time = stopwatch.ElapsedMilliseconds });
            }
        }

        //修改数据
        private void UpdateData()
        {
            Stopwatch stopwatch = new Stopwatch();
            string[] sql = new string[1];
            sql[0] = " update testtable set name='李四' where name like '%张三%' and age > 100 and age <10000000";



            int RowSum = SQLCEHelper.GetInstance().Query("select * from testtable");
            stopwatch.Restart();
            int rowCount = SQLCEHelper.GetInstance().ExecuteSQL(sql);
            stopwatch.Stop();
            WriteLog("SQLCE修改数据 " + rowCount + " 行 总行数" + RowSum + "执行时间：" + stopwatch.ElapsedMilliseconds + " 毫秒");

            if (AutoTestFlag)
            {
                SqlCEUpdateList.Add(new KeyValue { RowCount = rowCount, Time = stopwatch.ElapsedMilliseconds });
            }

            ///sqlite总行数
            RowSum = sqliterhelper.Query("select * from testtable");
            stopwatch.Restart();
            rowCount = sqliterhelper.Insert(sql);
            stopwatch.Stop();
            WriteLog("SQLite修改数据 " + rowCount + " 行 总行数" + RowSum + "执行时间：" + stopwatch.ElapsedMilliseconds + " 毫秒");

            if (AutoTestFlag)
            {
                SqliteUpdateList.Add(new KeyValue { RowCount = rowCount, Time = stopwatch.ElapsedMilliseconds });
            }
        }

        public void WriteLog(string msg)
        {
            this.Invoke((EventHandler)delegate
            {
                txtlog.Text += "（" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "）:" + msg + "\r\n";
            });

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string sql = "select * from TestTable";
            string sqlWhere = "select * from TestTable where name like '%张三%' and age > 100 and age <1200";


            Stopwatch stopwatch = new Stopwatch();


            stopwatch.Start();
            int rowNum = SQLCEHelper.GetInstance().Query(sql);
            stopwatch.Stop();
            WriteLog("SQLCE无条件查询" + rowNum + "行耗费时间：" + stopwatch.ElapsedMilliseconds + " 毫秒");
            if (AutoTestFlag)
            {
                SqlCEQueryList.Add(new KeyValue { RowCount = rowNum, Time = stopwatch.ElapsedMilliseconds });
            }

            stopwatch.Start();
            SQLCEHelper.GetInstance().Query(sqlWhere);
            stopwatch.Stop();
            WriteLog("SQLCE带条件查询:" + rowNum + "行查询1099条记录耗费时间：" + stopwatch.ElapsedMilliseconds + " 毫秒");
            if (AutoTestFlag)
            {
                SqlCEQueryWhereList.Add(new KeyValue { RowCount = rowNum, Time = stopwatch.ElapsedMilliseconds });
            }


            stopwatch.Start();
            rowNum = sqliterhelper.Query(sql);
            stopwatch.Stop();
            WriteLog("SQLiter无条件查询" + rowNum + "行耗费时间：" + stopwatch.ElapsedMilliseconds + " 毫秒");

            if (AutoTestFlag)
            {
                SqliteQueryList.Add(new KeyValue { RowCount = rowNum, Time = stopwatch.ElapsedMilliseconds });
            }

            stopwatch.Start();
            rowNum = sqliterhelper.Query(sqlWhere);
            stopwatch.Stop();
            WriteLog("SQLiter带条件查询" + rowNum + "行耗费时间：" + stopwatch.ElapsedMilliseconds + " 毫秒");

            if (AutoTestFlag)
            {
                SqliteQueryWhereList.Add(new KeyValue { RowCount = rowNum, Time = stopwatch.ElapsedMilliseconds });
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                String[] sql = new String[1];
                sql[0] = " delete from testtable";

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();
                int rowCount = SQLCEHelper.GetInstance().ExecuteSQL(sql);
                stopwatch.Stop();
                WriteLog("SQLCE删除" + rowCount + "行耗费时间：" + stopwatch.ElapsedMilliseconds + " 毫秒");
                if (AutoTestFlag)
                {
                    SqlCEDeleteList.Add(new KeyValue { RowCount = rowCount, Time = stopwatch.ElapsedMilliseconds });
                }


                stopwatch.Start();
                rowCount = sqliterhelper.Insert(sql); ;
                stopwatch.Stop();
                WriteLog("SQLite删除" + rowCount + "行耗费时间：" + stopwatch.ElapsedMilliseconds + " 毫秒");
                if (AutoTestFlag)
                {

                    SqliteDeleteList.Add(new KeyValue { RowCount = rowCount, Time = stopwatch.ElapsedMilliseconds });
                }


                // WriteLog("清空数据库成功！");
            }
            catch (Exception ex)
            {
                WriteLog("清空数据库错误:" + ex.Message); 
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtlog.Text = "";
        }

        public void ChanageRowNum(int rowNum)
        {
            this.Invoke((EventHandler)delegate
            {
                txtNum.Text = rowNum.ToString();
            });
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            try
            {
                SqliteInsertList.Clear();
                SqliteQueryList.Clear();
                SqlCEInsertList.Clear();
                SqlCEQueryList.Clear();
                AutoTestFlag = true;
                Task.Factory.StartNew(() =>
                {
                    for (int i = 10000; i <= 1000000;)
                    {
                        ChanageRowNum(i);
                        // int i = int.Parse(txtNum.Text);
                        WriteLog(((i / 10000.0)) + "万条数据测试：");
                        AutoTest();
                        if (i >= 100000)
                        {
                            i = i + 100000;
                        }
                        else
                        {
                            i = i + 10000;
                        }
                    }
                    WriteLog("自动测试完成！");

                    //sqlite 插入测试结果
                    foreach (var item in SqliteInsertList.GroupBy(v => v.RowCount))
                    {
                        int count = SqliteInsertList.Where(v => v.RowCount == item.Key).Count();
                        double time = (from t in SqliteInsertList
                                       where t.RowCount == item.Key
                                       select t.Time).Average();
                        WriteLog("SQLite 插入" + (item.Key / 10000.0) + "万行测试" + count + "次平均运行时间：" + time + "毫秒");
                    }

                    //sqlite 无条件查询测试结果
                    foreach (var item in SqliteQueryList.GroupBy(v => v.RowCount))
                    {
                        int count = SqliteQueryList.Where(v => v.RowCount == item.Key).Count();
                        double time = (from t in SqliteQueryList
                                       where t.RowCount == item.Key
                                       select t.Time).Average();
                        WriteLog("SQLite 无条件查询" + (item.Key / 10000.0) + "万行查询1099条测试" + count + "次平均运行时间：" + time + "毫秒");
                    }

                    //sqlite 带条件查询测试结果
                    foreach (var item in SqliteQueryWhereList.GroupBy(v => v.RowCount))
                    {
                        int count = SqliteQueryWhereList.Where(v => v.RowCount == item.Key).Count();
                        double time = (from t in SqliteQueryWhereList
                                       where t.RowCount == item.Key
                                       select t.Time).Average();
                        WriteLog("SQLite 带条件查询" + (item.Key / 10000.0) + "万行测试" + count + "次平均运行时间：" + time + "毫秒");
                    }
                    //sqlite 修改 
                    foreach (var item in SqliteUpdateList.GroupBy(v => v.RowCount))
                    {
                        int count = SqliteUpdateList.Where(v => v.RowCount == item.Key).Count();
                        double time = (from t in SqliteUpdateList
                                       where t.RowCount == item.Key
                                       select t.Time).Average();
                        WriteLog("SQLite 修改" + (item.Key / 10000.0) + "万行测试" + count + "次平均运行时间：" + time + "毫秒");
                    }

                    //sqlite 删除
                    foreach (var item in SqliteDeleteList.GroupBy(v => v.RowCount))
                    {
                        int count = SqliteDeleteList.Where(v => v.RowCount == item.Key).Count();
                        double time = (from t in SqliteDeleteList
                                       where t.RowCount == item.Key
                                       select t.Time).Average();
                        WriteLog("SQLite 删除" + (item.Key / 10000.0) + "万行测试" + count + "次平均运行时间：" + time + "毫秒");
                    }

                    //sqlce 插入测试结果
                    foreach (var item in SqlCEInsertList.GroupBy(v => v.RowCount))
                    {
                        int count = SqlCEInsertList.Where(v => v.RowCount == item.Key).Count();
                        double time = (from t in SqlCEInsertList
                                       where t.RowCount == item.Key
                                       select t.Time).Average();
                        WriteLog("SQLCE 插入" + (item.Key / 10000.0) + "万行测试" + count + "次平均运行时间：" + time + "毫秒");
                    }

                    //sqlce 无条件查询测试结果
                    foreach (var item in SqlCEQueryList.GroupBy(v => v.RowCount))
                    {
                        int count = SqlCEQueryList.Where(v => v.RowCount == item.Key).Count();
                        double time = (from t in SqlCEQueryList
                                       where t.RowCount == item.Key
                                       select t.Time).Average();
                        WriteLog("SQLCE 无条件查询" + (item.Key / 10000.0) + "万行测试" + count + "次平均运行时间：" + time + "毫秒");
                    }

                    //sqlce 带条件查询测试结果
                    foreach (var item in SqlCEQueryWhereList.GroupBy(v => v.RowCount))
                    {
                        int count = SqlCEQueryWhereList.Where(v => v.RowCount == item.Key).Count();
                        double time = (from t in SqlCEQueryWhereList
                                       where t.RowCount == item.Key
                                       select t.Time).Average();
                        WriteLog("SQLCE 带条件查询" + (item.Key / 10000.0) + "万行测试" + count + "次平均运行时间：" + time + "毫秒");
                    }


                    //sqlce 修改
                    foreach (var item in SqlCEUpdateList.GroupBy(v => v.RowCount))
                    {
                        int count = SqlCEUpdateList.Where(v => v.RowCount == item.Key).Count();
                        double time = (from t in SqlCEUpdateList
                                       where t.RowCount == item.Key
                                       select t.Time).Average();
                        WriteLog("SQLCE 修改" + (item.Key / 10000.0) + "万行测试" + count + "次平均运行时间：" + time + "毫秒");
                    }

                    //sqlce 删除
                    foreach (var item in SqlCEDeleteList.GroupBy(v => v.RowCount))
                    {
                        int count = SqlCEDeleteList.Where(v => v.RowCount == item.Key).Count();
                        double time = (from t in SqlCEDeleteList
                                       where t.RowCount == item.Key
                                       select t.Time).Average();
                        WriteLog("SQLCE 删除" + (item.Key / 10000.0) + "万行测试" + count + "次平均运行时间：" + time + "毫秒");
                    }

                    AutoTestFlag = false;
                });
            }
            catch
            {
                AutoTestFlag = false;
            }
        }
        public void AutoTest()
        {
            //每种数据进行10次测试
            for (int i = 0; i < 10; i++)
            {
                btnCreate_Click(this, null);//添加数据
                UpdateData();//修改数据
                btnQuery_Click(this, null);//查询数据
                btnClear_Click(this, null);//清空数据库
            }


        }

    }
}
