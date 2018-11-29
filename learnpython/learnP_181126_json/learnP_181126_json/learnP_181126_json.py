import json
import requests

#json.dumps()将Python对象（dict）转换为字符串（str）
#json.loads()将字符串（str）转换为Python对象（dict）
#dump和load也是类似的功能，只是与文件操作结合起来
def main():
	url="http://wthrcdn.etouch.cn/weather_mini?city=上海"
	response=requests.get(url)
	#print(response.text)
	#print(type(response.text))
	rd=json.loads(response.text)
	#print(rd)
	#print(type(rd))
	showDict(rd)
	print(rd["data"]["aqi"])


#打印dict内容（key：value）
def showDict(d,k=""):
	if isinstance(d,dict):
		for key in d:
			if k=="":
				showDict(d[key],key)
			else:
				showDict(d[key],k+"."+key)
	elif isinstance(d,str):
		print(k+":"+d)
	elif isinstance(d,int):
		print(k+":"+str(d))
	else:
		for i in d:
			showDict(i,k+"["+str(d.index(i))+"]")

main()