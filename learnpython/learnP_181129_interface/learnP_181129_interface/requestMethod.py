import requests
import json
from fake_useragent import UserAgent

class requestMethod:
	def __init__(self):
		try:
			ua=UserAgent(verify_ssl=False)
			ua=ua.random
			self.headers={"User-Agent":ua}
		except Exception as e:
			print(e)

	def get(self,url):
		try:
			resp=requests.get(url,headers=self.headers)
			resp.encoding="utf-8"
			respjson=json.loads(resp.text)
			return respjson
		except Exception as e:
			print("get请求错误：%s"%e)

	def post(self,url,param):
		if isinstance(param,dict):
			param=json.dumps(param)
		try:
			resp=requests.post(url,data=param,headers=self.headers)
			respjson=json.loads(resp.text)
			return respjson
		except Exception as e:
			print("post请求错误：%s"%e)

