# -- coding: utf-8 --
import xlwt
import xlrd
import xlutils
from xlrd import sheet
from xlrd.sheet import Cell
from tempfile import TemporaryFile
from datetime import date,datetime,time
from decimal import Decimal
from xlwt.Style import XFStyle, easyxf
from xlwt.Formatting import Pattern

def main():
	#wb=xlwt.Workbook()
	wb=xlwt.Workbook(encoding="utf-8")
	sheet1=wb.add_sheet("testcase")
	sheet2=wb.add_sheet("testreport")
	title=["ID","description","url","method","key","info","username","expectedcode","actualcode","text","result","time"]
	i=0
	#print(sheet1.row_values(0))
	for ti in title:
		#sheet1.write(0,i,ti)#对特定单元格写入使用write方法。sheet1.write(0,i,ti)相当于sheet1.row(0).write(i,ti)，写入大量数据时该方法较慢。
		sheet1.row(0).set_cell_text(i,ti,easyxf("font: bold True;""pattern: pattern SOLID_PATTERN,fore_colour green;"))#写入大量数据时使用该方法。
		#sheet1.col(i).width=256*(len(ti)+2)#单元格宽度为256*(字符长度+2)
		#if i==0:
		#	sheet1.col(i).hidden=True#隐藏第一列
		i+=1
	sheet2.flush_row_data()#减小内存压力，flush之前的数据不可再改动	
	row1=sheet1.row(1)
	#print(datetime.now())
	row1_content=[1,"验证key的正确性","http://www.tuling123.com/openapi/api","POST","123","","hm","40001","","",None,datetime.now()]
	row2=sheet1.row(2)
	row2_content=[2,"验证key的正确性","http://www.tuling123.com/openapi/api","POST","abc","","hm","40001","","","True",datetime.now()]
	row3=sheet1.row(3)
	row3_content=[3,"验证key的正确性","http://www.tuling123.com/openapi/api","POST","abc","","hm","40001","","","False",datetime.now()]
	writeData(row1,title,row1_content)
	writeData(row2,title,row2_content)
	writeData(row3,title,row3_content)
	wb.save("data.xls")

	'''
	TemporaryFile类是tempfile中最常用的类之一，其目的就在于提供一个统一的临时文件调用接口，读写临时文件，并且保证临时文件的隐形性
	TemporaryFile类的构造方法，其返回的还是一个文件对象。但这个文件对象特殊的地方在于
	1. 对应的文件没有文件名，对除了本程序之外的程序不可见
	2. 在被关闭的同时被删除
	'''
	wb.save(TemporaryFile())


def setCellStyle(bool):
	style=XFStyle()
	pattern=Pattern()
	pattern.pattern=1
	if bool=="True":
		pattern.pattern=Pattern.SOLID_PATTERN
		pattern.pattern_fore_colour=3
	elif bool=="False":
		pattern.pattern=Pattern.SOLID_PATTERN
		pattern.pattern_fore_colour=2
	else:
		pattern.pattern=Pattern.NO_PATTERN
	style.pattern=pattern
	return style
	
def writeData(row,title,row_c):
	row_cd=dict(zip(title,row_c))
	for key in row_cd:
		if isinstance(row_cd[key],int):
			row.set_cell_number(title.index(key),row_cd[key])
		elif isinstance(row_cd[key],str):
			row.set_cell_text(title.index(key),row_cd[key],setCellStyle(row_cd[key]))
		elif isinstance(row_cd[key],date) or isinstance(row_cd[key],datetime) or isinstance(row_cd[key],time):
			row.set_cell_date(title.index(key),row_cd[key],easyxf(num_format_str="YYYY-MM-DD HH:MM:SS"))
		elif isinstance(row_cd[key],bool):
			row.set_cell_boolean(title.index(key),row_cd[key],setCellStyle(row_cd[key]))
		else:
			row.set_cell_blank(title.index(key))

if __name__=="__main__":
	main()
