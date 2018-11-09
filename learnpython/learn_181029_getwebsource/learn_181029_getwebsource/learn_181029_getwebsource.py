#_*_coding: utf-8_*_
import requests
import re
from requests.exceptions import RequestException
import threading
import chardet
import io
from bs4 import BeautifulSoup
from MSSQL import MSSQLS
import time

listall=[]
def get_one_page(url):
	try:
		header={'User-Agent': 'Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.81 Safari/537.36', 'Accept-Encoding': 'gzip, deflate', 'Accept': '*/*', 'Connection': 'keep-alive'}
		response=requests.get(url,headers=header)
		#print(response.text.encode("iso-8859-1").decode('gb2312'))
		#使用ISO-8859-1解码之后的代码
		#soup = BeautifulSoup(response.text.encode("iso-8859-1").decode('gb2312'))
		#print(soup.prettify())
		if response.status_code==200:
			#使用print(response.encoding)查看获取的response编码为iso-8859-1，而查看网页源码编码为gb2312，所以要做下面的处理
			return response.text.encode("iso-8859-1").decode('gb2312',"ignore")
		else:
			return "wrong状态不正确:"+str(response.status_code)
	except RequestException as ex:
		return "wrong"

def parse_one_page(url):
	html=get_one_page(url)
	if 'wrong' not in html:
	#re.compile将正则表达式字符串编译成正则表达式对象
		pattern=re.compile('<td>\s*<a\s*href=(?:\'|")(.*?)(?:\'|")>\s*(.+?)\s*<br\/?>\s*</a></td>',re.S)#re.S
		#re.findall搜索字符串，以列表形式返回所有匹配结果
		results=re.findall(pattern,html)
		numb=1
		print(time.time())
		for result in results:
			mz=result[1].strip()
			#t= threading.Thread(target=thread_getpro(result,url), name=mz)
			thread_getpro(result,url)
			print(str(numb)+"/"+str(len(results))+"......"+mz+"ok")
			numb+=1
		print(time.time())
		return "ok"
	else:
		return html
		
def thread_getpro(result,url):
	#re.match尝试从字符串的起始位置匹配的一个模式，如果不是起始位置匹配的话，match()就返回none 
	#基本的语法结构：re.match(pattern,string,flags=0) 
	website=url.replace('index',re.findall('\d+',result[0].strip())[0])
	pro=result[1].strip()
	html1=get_one_page(website)
	pattern1=re.compile('<td><a href=([^>]*?)>([\d]+)</a></td><td><a href=[^>]*?>([^\d]+)</a></td>',re.S)
	results1=re.findall(pattern1,html1)
	for result1 in results1:
		#t1 = threading.Thread(target=thread_getshi(result1,pro,url), name=result1[2].strip())
		thread_getshi(result1,pro,url)

def thread_getshi(result1,pro,url):
	website1=url.replace('index',re.findall('\d+\/\d+',result1[0].strip())[0])
	shiNo=result1[1].strip()
	shi=result1[2].strip()
	html2=get_one_page(website1)
	pattern2=re.compile('<td><a href=([^>]*?)>([\d]+)</a></td><td><a href=[^>]*?>([^\d]+)</a></td>',re.S)
	results2=re.findall(pattern2,html2)
	for result2 in results2:
		qoxNo=result2[1].strip()
		qox=result2[2].strip()
		list=[pro,shiNo,shi,qoxNo,qox]
		listall.append(list)

def main():
	url="http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2017/index.html"   
	a=parse_one_page(url)
	str=""
	print(len(listall))
	total=len(listall)
	count=0
	if "ok" in a:
		for detail in listall:
			if count<=100:
				count+=1
			else:
				count=1
				str=""
			total-=1
			str+=" INSERT INTO dbo.T_CHINA (province, cityNo, city, countyNo, county) VALUES ('%s', '%s', '%s', '%s', '%s') "%(detail[0],detail[1],detail[2],detail[3],detail[4])
			if count==101 or total==0:
				ms = MSSQLS(driver='SQL Server Native Client 11.0',server="172.16.5.51",user="sa",password="abc123*",database="XueXi")
				ms.ExecNonQuery(str)
		print("finish")

if __name__=="__main__":
    main()