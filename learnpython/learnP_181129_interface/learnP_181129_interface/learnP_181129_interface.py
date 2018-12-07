import requests
import json
from fake_useragent import UserAgent
import learnP_181127_excel
import unittest
from ddt import data,ddt
import HTMLReport
import requestMethod
import xlrd,xlutils
import xlwt
from xlwt import Worksheet,Workbook
from xlutils.copy import copy
#from openpyxl import load_workbook
#from openpyxl import Workbook
#from openpyxl.writer.excel import ExcelWriter
from xlwt.Style import easyxf
import datetime
import re
import logging

#整个文件的开始和结束执行
def setUpModule():
	print("开始执行该文件")

def tearDowmModule():
	print("结束执行该文件")


@ddt
class interfaceTestCase(unittest.TestCase):
	global requestdata
	requestdata=learnP_181127_excel.getExcelRowData('D:\\learn\\learn_repo\\learnpython\\learnP_181129_interface\\data2003.xls')[:2]

	#整个Test类的开始和结束执行，必须在方法上加@classmethod的装饰器
	@classmethod
	def setUpClass(cls):
		print("开始执行测试类")
		
		#使用xlrd/xlwt
		cls.wb=xlrd.open_workbook('D:\\learn\\learn_repo\\learnpython\\learnP_181129_interface\\data2003.xls',formatting_info=True)#加上formatting_info=True，文件格式为.xlsx时报错
		#cls.sheeto=cls.wb.sheet_by_index(0)
		cls.wbc=copy(cls.wb)
		cls.sheetc=cls.wbc.get_sheet(0)

		#使用openpyxl
		#cls.wbc=load_workbook('D:\\learn\\learn_repo\\learnpython\\learnP_181129_interface\\data.xlsx')
		#cls.sheetc=cls.wbc.get_sheet_by_name(cls.wbc.get_sheet_names()[0])

	@classmethod
	def tearDownClass(cls):
		#try:
		for data in requestdata:
			cls.sheetc.row((int)(data["ID"])).set_cell_number(8,data["actualcode"])#若set_cell_number写成set_cell_text，保存文件时会报错。注意使用正确的方法赋值。
			cls.sheetc.row((int)(data["ID"])).set_cell_text(9,data["text"])
			func=lambda x:"pattern: pattern SOLID_PATTERN,fore-color bright_green;" if x else"pattern: pattern SOLID_PATTERN,fore-color red;"
			cls.sheetc.row((int)(data["ID"])).set_cell_boolean(10,data["result"],easyxf(func(data["result"])))
			cls.sheetc.row((int)(data["ID"])).set_cell_date(11,data["time"],easyxf(num_format_str="YYYY-MM-DD HH:MM:SS"))
		cls.wbc.save('D:\\learn\\learn_repo\\learnpython\\learnP_181129_interface\\data_result.xls')
		#except Exception as e:
		#	print(e)

		#使用openpyxl
		#for data in requestdata:
		#	cls.sheetc.cell((int)(data["ID"])+1,9,data["actualcode"])
		#	cls.sheetc.cell((int)(data["ID"])+1,10,data["text"])
		#	cls.sheetc.cell((int)(data["ID"])+1,11,data["result"])
		#cls.wbc.save('D:\\learn\\learn_repo\\learnpython\\learnP_181129_interface\\data1.xlsx')
		print("结束执行测试类")
	
	#每个用例的开始和结束执行
	def setUp(self):
		print(self.id())#输出__main__.类名.方法名
		print(self._testMethodName+":============>Test start")

	def tearDown(self):
		print(self._testMethodName+":============>Test finish")
	
	#@unittest.skipIf(False,"暂时跳过")#用于跳过指定条件的测试用例
	#@unittest.skipUnless(True,"暂时跳过")#用于跳过指定条件的测试用例
	#@unittest.skip("暂时跳过")#用于跳过测试用例		
	@data(*requestdata)#装饰器需紧跟方法，即写在方法上，中间无其他内容
	def test_data(self,requestdata):
		"""使用ddt库测试"""
		self.data=requestdata
		url=requestdata["url"]
		method=requestdata["method"]
		param=dict(zip(["key","info","userid"],[requestdata["key"],requestdata["info"],requestdata["userid"]]))
		expectcode=(int)(requestdata["expectcode"])
		#response=requests.request(method,url=url,params=param)
		#rejson=json.loads(response.text)
		if method=="POST":
			rejson=requestMethod.requestMethod().post(url,param)
		self.data["actualcode"]=rejson["code"]
		self.data["text"]=rejson["text"]
		requestdata["actualcode"]=rejson["code"]
		requestdata["text"]=rejson["text"]
		requestdata["time"]=datetime.datetime.now()
		#try:
		#	self.assertEqual(expectcode,rejson["code"],msg="code有误：预期%s，实际%s"%(expectcode,rejson["code"]))
		#	requestdata["result"]=True
		#except:
		#	requestdata["result"]=False
		#	#return False
		#self.longMessage=False	#self.longMessage默认为True，表明将下面方法的msg加到默认信息后面，改为False，则msg覆盖默认信息
		self.assertEqual(expectcode,rejson["code"],msg="code有误：预期%s，实际%s"%(expectcode,rejson["code"]))
		self.data["result"]=True

	
	@unittest.skip("暂时跳过")#用于跳过测试用例
	def test_subtest(self):
		"""使用子类测试"""
		for request in requestdata:
			with self.subTest(request=request):	
				print(self._testMethodName+str(request["ID"])+":==============start")
				self.data=request		
				url=request["url"]
				method=request["method"]
				param=dict(zip(["key","info","userid"],[request["key"],request["info"],request["userid"]]))
				expectcode=(int)(request["expectcode"])
				#response=requests.request(method,url=url,params=param)
				#rejson=json.loads(response.text)
				if method=="POST":
					rejson=requestMethod.requestMethod().post(url,param)
				self.data["actualcode"]=rejson["code"]
				self.data["text"]=rejson["text"]
				request["actualcode"]=rejson["code"]
				request["text"]=rejson["text"]
				request["time"]=datetime.datetime.now()
				self.assertEqual(expectcode,rejson["code"],msg="code有误：预期%s，实际%s"%(expectcode,rejson["code"]))
				request["result"]=True
				print(self._testMethodName+str(request["ID"])+":==============end")

	
	#@unittest.expectedFailure
	#def test_fail(self):
	#	'''
	#	预期失败，不计入测试不通过数量中
	#	'''
	#	with self.assertLogs('foo', level='INFO') as cm:
	#	   logging.getLogger('foo').info('first message')#getLogger：名称
	#	   logging.getLogger('foo.bar').error('second message')
	#	   print(cm.output[1])#log内容
	#	   print(cm.records[1])#log出现在哪个文件的第几行
	#	self.assertEqual(cm.output, ['INFO:foo:first message',
	#								 'ERROR:foo.bar:second message'])	 
	#	self.assertEqual(1,0,"excepted failure")

def discoverTestCase():	
	'''
		打印当前文件中的特定测试方法的名称
	'''
	allTestCaseName=[]
	discover=unittest.defaultTestLoader.discover(".","learnP_181129_interface.py")
	for suite in discover:
		for tc in suite:
			#print(tc)
			pattern=re.compile("testMethod=(test_data.*?)>")
			result=pattern.findall(str(tc))
			#for r in result:
			#	allTestCaseName.append(r)
			allTestCaseName.extend(result)
	return allTestCaseName

def skipUnlessHasAttr(obj,attr):
	'''
		自定义一个跳过装饰器
	'''	
	if hasattr(obj,attr):
		return lambda func:func
	else:
		return unittest.skip("{!r} doesn't have {!r}".format(obj, attr))

def suite(TestCase,*CaseName):
	'''
	组合测试用例，返回一个testsuite（测试套件）
	'''
	suite = unittest.TestSuite()
	# 测试用例加载器
	if len(CaseName)==0:
		loader = unittest.TestLoader()
		# 把测试用例加载到测试套件中
		suite.addTests(loader.loadTestsFromTestCase(TestCase))
	else:
		for tc in CaseName:
			if isinstance(tc,str):
				suite.addTest(TestCase(tc))
			elif isinstance(tc,list):
				for tc1 in tc:
					suite.addTest(TestCase(tc1))
	return suite

def printInfo(result):
	'''打印结果相关数据'''
	print("[errors]:"+str(result.errors))
	print("[failures]:"+str(result.failures))
	print("[skipped]:"+str(result.skipped))
	print("[expectedFailures]:"+str(result.expectedFailures))
	print("[testsRun]:"+str(result.testsRun))


def runTest():
	runner = HTMLReport.TestRunner(
							   #report_file_name='test',  # 报告文件名，如果未赋值，将采用“test+时间戳”
                               output_path='report',  # 保存文件夹名，默认“report”
                               title='测试报告',  # 报告标题，默认“测试报告”
                               description='无测试描述',  # 报告描述，默认“测试描述”
                               thread_count=1,  # 并发线程数量（无序执行测试），默认数量 1
                               thread_start_wait=3,  # 各线程启动延迟，默认 0 s
                               sequential_execution=False,  # 是否按照套件添加(addTests)顺序执行，
                               # 会等待一个addTests执行完成，再执行下一个，默认 False
                               # 如果用例中存在 tearDownClass ，建议设置为True，
                               # 否则 tearDownClass 将会在所有用例线程执行完后才会执行。
                               # lang='en'
                               lang='cn'  # 支持中文与英文，默认中文
                               )
	return runner


	
if __name__=="__main__":
	#suite = unittest.TestSuite()
	## 测试用例加载器
	#loader = unittest.TestLoader()
	## 把测试用例加载到测试套件中
	#suite.addTests(loader.loadTestsFromTestCase(interfaceTestCase))


	#name=discoverTestCase()
	#suite =suite(interfaceTestCase,name)
	#suite =suite(interfaceTestCase,name,"test_fail")

	suite =suite(interfaceTestCase)

	
	#runner=unittest.TextTestRunner()
	## 测试用例执行器
	runner = HTMLReport.TestRunner(
							   #report_file_name='test',  # 报告文件名，如果未赋值，将采用“test+时间戳”
                               output_path='report',  # 保存文件夹名，默认“report”
                               title='测试报告',  # 报告标题，默认“测试报告”
                               description='无测试描述',  # 报告描述，默认“测试描述”
                               thread_count=1,  # 并发线程数量（无序执行测试），默认数量 1
                               thread_start_wait=3,  # 各线程启动延迟，默认 0 s
                               sequential_execution=False,  # 是否按照套件添加(addTests)顺序执行，
                               # 会等待一个addTests执行完成，再执行下一个，默认 False
                               # 如果用例中存在 tearDownClass ，建议设置为True，
                               # 否则 tearDownClass 将会在所有用例线程执行完后才会执行。
                               # lang='en'
                               lang='cn'  # 支持中文与英文，默认中文
                               )
 # 执行测试用例套件
	result=runner.run(suite)
	printInfo(result)

	#unittest.main()
	