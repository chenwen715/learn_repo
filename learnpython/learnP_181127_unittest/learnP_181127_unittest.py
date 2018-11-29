import requests,json,unittest
import HTMLReport#pip install HTMLReport进行安装

class weatherapiTestCase(unittest.TestCase):
	def setUp(self):
		url="https://www.apiopen.top/weatherApi"
	def testCorrect(self):
		para={"city":"苏州"}
		url="https://www.apiopen.top/weatherApi"
		r=requests.request("post",url,params=para)
		rj=json.loads(r.text)
		assert para["city"]==rj["data"]["city"]
	def testIncorrect(self):
		para={"city":"苏州"}
		url="https://www.apiopen.top/weatherApi"
		r=requests.request("post",url,params=para)
		rj=json.loads(r.text)
		#assert rj["data"]["city"]==u'上海'
		assert para["city"]!=rj["data"]["city"]
	def tearDown(self):
		pass

if __name__=="__main__":
	suite = unittest.TestSuite()
	# 测试用例加载器
	loader = unittest.TestLoader()
	# 把测试用例加载到测试套件中
	suite.addTests(loader.loadTestsFromTestCase(weatherapiTestCase))
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