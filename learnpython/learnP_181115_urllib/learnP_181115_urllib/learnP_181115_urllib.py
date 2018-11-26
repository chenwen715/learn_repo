import urllib.request
import json

url="https://www.baidu.com"
req=urllib.request.Request(url)#request类
html=urllib.request.urlopen(req)#HTTPresponse类
#print(dir(req))
#print(dir(html))
print(html.headers)
#print(html.getheader("Server"))
print(html.getcode())
htmllist=html.readlines()
b=""
for a in htmllist:
	b+=a.decode("utf-8")
b+="\n\n"
print(b)


url1="http://172.16.18.63:99/login/index"
data={"username":"jjacs","password":"123"}
data=urllib.parse.urlencode(data).encode("utf-8")
request1=urllib.request.Request(url1,data)
response=urllib.request.urlopen(request1);
print(response.getcode(),response.msg)
respContent=response.read()
#print(type(respContent))
#print(type(respContent.decode("utf-8")))
#print(type(json.loads(respContent)))
respjson=json.loads(respContent)
for key in respjson:
	print(key+":"+str(respjson[key]))



