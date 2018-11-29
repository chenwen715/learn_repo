import requests
import json
import os

def main():
	print(os.getcwd())
	url="https://www.apiopen.top/weatherApi"
	para={"city":"苏州"}
	r=requests.request("post",url,params=para)
	print(r)
	rj=json.loads(r.text)
	showDict(rj)


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
