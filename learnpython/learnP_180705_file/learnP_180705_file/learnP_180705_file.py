#try:
#    f=open(r'D:\xuex2\test.txt',"r")
#    print(f.read())
#finally:
#    if f:
#        f.close()

#with open(r'D:\xuex2\test.txt','r') as f:
#    print(f.read())
#with open(r'/xuex2/test.txt','r') as f:
#    print(f.read())

#with open(r'D:\xuex2\test.txt','a') as f:
#    f.write("\n123")

#with open(r'D:\xuex2\test.txt','r') as f:
#    print(f.read())

import os
#a=os.environ.get('Path').split(';')
#for i in a:
#    print(i)

#print(os.path.abspath('.'))
#a=os.path.join('/xuex2','python')#拼接路径，python为新建文件夹的名字
#os.mkdir(a)#新建文件夹
#os.rmdir(a)#删除文件夹
#print(os.path.abspath('.').split(':')[0]+':')

#list=sorted([x for x in os.listdir('/')],key=str.lower)
#for i in list:
#    print(i)

#bb=[3,5,6,2,9,4]
#print(sorted(bb,key=int))
def findfile(text,path='/'):
    list=[]
    for i in os.listdir(path):
        filename=os.path.join(path,i)
        if os.path.isfile(filename):
            if text in i:
                list.append(i)
        elif os.path.isdir(filename):           
            findfile(text,filename)
    return list


print(findfile('file',os.path.abspath('.')))  