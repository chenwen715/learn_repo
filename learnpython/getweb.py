import requests
import re
from requests.exceptions import RequestException

def parse_one_page(url):
	html=get_one_page(url)
	#re.compile将正则表达式字符串编译成正则表达式对象
	pattern=re.compile('<td>\s*<a\s*href=(.*?)>\s*(.*?)\s*<br\/?>\s*</a></td>',re.S)#re.S
	#re.findall搜索字符串，以列表形式返回所有匹配结果
	results=re.findall(pattern,html)
	for result in results:
		t= threading.Thread(target=thread_getpro(result), name=result[1].strip())
		
def thread_getpro(result):
	#re.match尝试从字符串的起始位置匹配的一个模式，如果不是起始位置匹配的话，match()就返回none 
	#基本的语法结构：re.match(pattern,string,flags=0) 
	website=html.replace('index',re.match('\d+',result[0].strip()))
	pro=result[1].strip()
	html1=get_one_page(website)
	pattern1=re.compile('<td><a href=([^>]*?)>([\d]+)</a></td><td><a href=[^>]*?>([^\d]+)</a></td>',re.S)
	results1=re.findall(pattern1,html1)
	for result1 in results1:
		t1 = threading.Thread(target=thread_getshi(result1,pro), name=result1[2].strip())

def thread_getshi(result1,pro):
	 website1=html.replace('index',re.match('\d+\/\d+',result1[0].strip()))
	 shiNo=result1[1].strip()
	 shi=result1[2].strip()
	 html2=get_one_page(website1)
	 pattern2=re.compile('<td><a href=([^>]*?)>([\d]+)</a></td><td><a href=[^>]*?>([^\d]+)</a></td>',re.S)
	 results2=re.findall(pattern2,html2)
	 for result2 in results2:
		 qoxNo=result2[1].strip()
		 qox=result2[2].strip()
		 list=[pro,shiNo,shi,qoxNoqox]
		 listall.append(list)
		 
def get_one_page(url):
	try:
		response=requests.get(url)
		if response.status_code==200:
			return response.text
		else:
			return "状态不正确:"+response.status_code
	except RequestException as ex:
		return "wrong"

def main():
    url="http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/index.html"
    parse_one_page(url)
    listall=[]
    print(listall)

if __name__=="__main__":
    main()