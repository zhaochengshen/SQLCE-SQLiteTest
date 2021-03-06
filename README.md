# SQLCE-SQLiteTest
SQLCE和SQLite性能测试

由于项目需要，在客户端需要做数据存储功能，考虑到部署方便同时满足功能需要的情况下选择了SQLCE 和SQLite两种数据库进行客户端数据存储。当然还有很多其他的方式做本地数据存储，比如本地文件存储、微软的Access等也可以做本地数据存储，都不再本次测试的考虑范围之内。本次主要针对SQLCE3.5、SQLCE4.0 和SQLite数据库的性能对比。
SQL CE: 全名（SQL Server Compact）该数据库为微软的产品，免费试用；数据库存储在扩展名为.sdf，最大容量为4GB。

SQLite：开源数据库，免费试用；数据库扩展名.db 或者.db3，理论上SQLite支持140TB的容量。当然如果数据量这么大的情况下可以考虑其他的数据库。
 

测试方法：通过一个连接的同时操作多条数据的方式测试数据库插入、查询的处理速度；SQLCE包含两个版本SQLCE3.5和SQLCE4.0，分别进行了测试。下图为数据库测试的记录报告。
说明：该测试记录在一张有4个字段的数据表上进行测试。





总结：
性能对比：
数据插入：不增加事务SQLCE的性能要比SQLite好很多，但是如果增加事务操作上SQLite综合效率要比SQLCE块大概5-10倍左右。
查询：SQLCE要优于SQLite 2倍左右。
修改和删除几乎差不太多。 

部署方便程度对比：
在开发.NET平台的程序时，SQLite只需要一个DLL文件即可完成对数据库的操作。但是SQLCE需要额外安装SQLCE的安装包才可以支持SQLCE数据库。从这个方面说SQLCE的部署要比SQLite复杂一些。
 
数据库容量对比：
SQLCE只能支持4GB的数据存储量，SQLite理论上支持140TB的数据存储。所以在存储量上SQLite优势很大。

备注：以上测试只是结合业务场景需要可能存在不科学之处，仅供参考和学习使用。
