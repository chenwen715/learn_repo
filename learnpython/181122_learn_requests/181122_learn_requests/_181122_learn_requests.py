import requests
import json

print(json.__all__)#查看json库的主要方法
url="https://www.baidu.com"
res=requests.get(url)
#print(dir(res))
print(res.headers)

url1="http://172.16.18.63:99/login/index"
data={"username":"jjacs","password":"123"}
res1=requests.post(url1,data)
print(type(res1.text))
print(res1.text)
print(type(res1.content))
print(res1.content)
print(type(json.loads(res1.content)))
print(json.loads(res1.content))