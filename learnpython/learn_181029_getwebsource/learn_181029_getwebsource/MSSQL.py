import pyodbc
#或使用pymssql
import pymssql

class MSSQLS:
    def __init__(self,driver,server,user,password,database):
        self.driver = driver
        self.server = server
        self.user = user
        self.password = password
        self.database = database

    def __GetConnect(self):
        if not self.database:
            raise(NameError,"没有设置数据库信息")
        self.conn = pyodbc.connect(driver=self.driver,server=self.server,user=self.user,password=self.password,database=self.database)
        cur = self.conn.cursor()
        if not cur:
            raise(NameError,"连接数据库失败")
        else:
            return cur

    def ExecQuery(self,sql):
        cur = self.__GetConnect()
        cur.execute(sql)
        resList = cur.fetchall()
        #查询完毕后必须关闭连接
        self.conn.close()
        return resList

    def ExecNonQuery(self,sql):
        cur = self.__GetConnect()
        cur.execute(sql)
		#更新，插入，删除操作后需提交commit
        self.conn.commit()
        self.conn.close()


