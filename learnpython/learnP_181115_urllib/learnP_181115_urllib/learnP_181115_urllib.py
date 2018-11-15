import urllib.request

url="https://www.baidu.com"
req=urllib.request.Request(url)#request¿‡
html=urllib.request.urlopen(req)#HTTPresponse¿‡
#print(dir(req))
#print(dir(html))
print(html.headers)
#print(html.getheader("Server"))
print(html.getcode())
print(html.readlines())


