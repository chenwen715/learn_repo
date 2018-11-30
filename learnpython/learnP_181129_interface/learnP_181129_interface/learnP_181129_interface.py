import requests
import json
from fake_useragent import UserAgent
import learnP_181127_excel
import unittest
from ddt import data,ddt
import HTMLReport
import requestMethod

@ddt
class interfaceTestCase(unittest.TestCase):
	global requestdata
	requestdata=learnP_181127_excel.getExcelRowData('D:\\learn\\learn_repo\\learnpython\\learnP_181129_interface\\data.xlsx')[2:4]

	@unittest.skip("暂时跳过")#用于跳过测试用例
	@data(*requestdata)
	def test_data(self,requestdata):
		url=requestdata["url"]
		method=requestdata["method"]
		param=dict(zip(["key","info","userid"],[requestdata["key"],requestdata["info"],requestdata["userid"]]))
		expectcode=(int)(requestdata["expectcode"])
		#response=requests.request(method,url=url,params=param)
		#rejson=json.loads(response.text)
		if method=="POST":
			rejson=requestMethod.requestMethod().post(url,param)
		self.assertEqual(expectcode,rejson["code"],msg="code有误：预期%s，实际%s"%(expectcode,rejson["code"]))

	

if __name__=="__main__":
	suite = unittest.TestSuite()
	# 测试用例加载器
	loader = unittest.TestLoader()
	# 把测试用例加载到测试套件中
	suite.addTests(loader.loadTestsFromTestCase(interfaceTestCase))
	# 测试用例执行器
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

	runner.run(suite)
