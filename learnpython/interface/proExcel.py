import xlrd
import xlwt
import xlutils
from xlrd import sheet
from builtins import zip

def getExcelColData(filepath,col=-1,sheet=0):
	'''
	获取特定sheet页特定列数据
	参数filepath：文件路径
	参数col:获取哪一列数据，可输入数字或字符串，不输入时以dict形式返回所有列数据
			输入参数类型不正确时，返回错误信息"wrong:[sheet]参数输入错误，应输入数字或sheet页名称"
	参数sheet:获取哪一sheet页的数据，可输入数字或字符串，默认获取第一个sheet页的数据
	'''	
	file=xlrd.open_workbook(filepath)
	#print(file.nsheets)#nsheets属性为excel文件中sheet页数
	#print(file.sheet_names())
	if isinstance(sheet,int):
		st=file.sheets()[sheet]#通过索引选择sheet页
	elif isinstance(sheet,str):
		st=file.sheet_by_name(sheet)
	else:
		return "wrong:[sheet]参数输入错误，应输入数字或sheet页名称"
	#print(type(st))
	#print(st.nrows,st.ncols)
	line1_cell=st.row(0)#需赋值给变量，否则后面报不在list中的错
	#print(line1_cell)
	#title=st.row(0)[0]#text：单元格内容(type:value)
	#print(title.value)
	#title=st.cell_value(0,0)#单元格内容(value)
	#print(title)
	if isinstance(col,int):
		if col<=0 or col>st.ncols:
			choosecol=line1_cell[:]
		else:
			choosecol=line1_cell[col-1:col]
	else:
		choosecol=line1_cell[:]
	data={}
	names=locals()
	for i in choosecol:
		listname=i.value
		names[listname]=st.col_values(choosecol.index(i),1,st.nrows)
		data[listname]=names[listname]
	if isinstance(col,str):
		return data[col]
	else:
		return data
	#print(data)
	#st1=file.sheet_by_name("BulksLoading")#通过sheet页名称选择sheet页
	#print(file.sheet_names().index("BulksLoading"))
	#st2=file.sheet_by_index(2)#通过索引选择sheet页
	#print(file.sheets().index(st2))

def getExcelRowData(filepath,sheet=0):
	'''
	按行返回sheet页中所有数据
	'''
	file=xlrd.open_workbook(filepath)
	if isinstance(sheet,int):
		st=file.sheet_by_index(sheet)#通过索引选择sheet页
	elif isinstance(sheet,str):
		st=file.sheet_by_name(sheet)
	else:
		return "wrong:[sheet]参数输入错误，应输入数字或sheet页名称"
	attrname=st.row_values(0)
	data=[]
	for i in range(1,st.nrows):
		rowdata={}
		rdata=st.row_values(i)
		rowdata=dict(zip(attrname,rdata))
		data.append(rowdata)
	return data
		

if __name__=="__main__":
	filepath="C:\\Users\\0322\\Desktop\\WarehouseData.xlsx"
	a=getExcelColData(filepath)
	print(a)
	b=getExcelRowData(filepath)
	for i in b:
		print(i)

