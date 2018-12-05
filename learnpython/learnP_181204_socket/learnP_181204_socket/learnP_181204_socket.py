# -*- coding:utf-8 -*-
import socket

def serverSocket():
	s=socket.socket()
	#host=socket.gethostname()
	host="127.0.0.1"
	port=2223
	s.bind((host,port))

	s.listen(5)

	while True:
		c,addr=s.accept()
		c.settimeout(3)
		print("连接地址为：",addr)		
		receivedata=c.recv(1024)
		#解析agvNo
		a=receivedata[3:13]
		b=str(a,encoding="utf-8").replace("\x00","")
		print(b)
		
		#c.close()

if __name__=="__main__":
	serverSocket()

