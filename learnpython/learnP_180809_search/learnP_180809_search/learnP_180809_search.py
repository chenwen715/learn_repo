import re,os,sys
import time
#name=os.path.basename(sys.argv[0])
path=os.getcwd()
#fullpath=path+"/"+name
list = os.listdir(path)
list1=[]
for file in list:
	if 'STK' in file and file.strip().endswith('.py'):
		list1.append(file)

for py in list1:
	#fullpath=os.path.join(path,'STK_02.py')
	#print(fullpath)
	#print("aaa")
	#f = open(fullpath,'r', encoding='UTF-8')              # 返回一个文件对象 

	f = open(py,'r', encoding='UTF-8')   	
	fi = f.readlines() 
	f.close()
	print ("length of old file:",fi.__len__())
	tarline=0
	count=0
	for i in range(fi.__len__()):
		if i==2 and 'from learnP_180710_log import Logger' not in fi[i]:
			count+=1
		if 'print' in fi[i] and "Logger" not in fi[i+1] and "Logger" not in fi[i+2]:
			if ('(' in fi[i]) and not(fi[i].count('(') ==  fi[i].count(')')) :
				count+=2
			else:
				count+=1
	count+=fi.__len__()

	#try:
	for i in range(count):
		if i==2 and "Logger" not in fi[i]:
			fi.insert(2,'from learnP_180710_log import Logger\n')
			continue		
		if (i>3 and i<(count-2) and fi[i].__contains__("print") and "Logger" not in fi[i+1] and "Logger" not in fi[i+2]) or ((i==count-2) and fi[i].__contains__("print") and "Logger" not in fi[i+1]) or ((i==count-1) and fi[i].__contains__("print")) :
			#if fi[i].rstrip().endswith("(") and (not fi[i+1].lstrip().startswith("(")) :
			if not(fi[i].count('(') ==  fi[i].count(')') ) :
				tarline = int(i)+2
				if fi[i].__contains__("("):
					txt=fi[i].split("(",1)[1].rsplit("\n",1)[0]
					space=fi[i].split("p",1)[0]
					txt1=fi[i+1].rsplit(")",1)[0]
				else:
						txt=fi[i].split("t",1)[1].rsplit("\n",1)[0]
						space=fi[i].split("p",1)[0]
						txt1=fi[i+1].rsplit('\n',1)[0]
				if "except" in fi[i-1] or ("except" in fi[i-5] and "print" in fi[i-4] and "Logger" in fi[i-2]):
					addinfo =space+ "Logger('error.log', level='error').logger.error("+txt+"\n"
					addinfo1 =txt1+")\n"
				else:		
					addinfo =space+ "Logger('info.log', level='info').logger.info("+txt+"\n"
					addinfo1 =txt1+")\n"
				fi.insert(tarline,addinfo)
				fi.insert(tarline+1,addinfo1)
			else:
				tarline = int(i)+1
				#print ("add <Logger('error.log', level='error').logger.error('hello'):",tarline)
				if fi[i].__contains__("("):
					txt=fi[i].split("(",1)[1].rsplit(")",1)[0]
					space=fi[i].split("p",1)[0]
				else:
						txt=fi[i].split("t",1)[1].rsplit('\n',1)[0]
						space=fi[i].split("p",1)[0]
				if "except" in fi[i-1] or ("except" in fi[i-3] and "print" in fi[i-2]):
					addinfo =space+ "Logger('error.log', level='error').logger.error("+txt+")\n"
				else:		
					addinfo =space+ "Logger('info.log', level='info').logger.info("+txt+")\n"
				fi.insert(tarline,addinfo)
	print("length of new file:",fi.__len__())
	#except Exception as e:
	#	print('except:', e)
	
	try:
		#filenew= open(fullpath,"wb")
		filenew= open(py,"wb")
		for item in fi:
			aa=type(item)
			if isinstance(item,str):
				item=str.encode(item)
				bb=type(item)
			filenew.write(item)
	except Exception as e:
		print('except:', e)
	finally:
		filenew.close()
		print(py,'==finish')
localtime = time.asctime( time.localtime(time.time()))
print(localtime ,'==all finish')