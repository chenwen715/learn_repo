# -*- coding: utf-8 -*-
import math
import os
#print("""hello中文测试
#a
#b""")
#name=input("please input your name : ")
#print("hello ,",name)
#print('%2d-%02d' % (3, 1))
#print('%.2f' % 3.1415926)
#lastyearscore=72
#thisyearscore=85
#percent=(thisyearscore-lastyearscore)/lastyearscore*100
#print("成绩提高 %.1f%%" %percent)
#if 2:
#    print("True")
#else:
#    print("false")
#sum=0
#for i in range(101):
#    sum+=i
#print(sum)
#d={"a":1,"b":2}
#print(d["a"])
#d.update(c=2)
#print(d)
#d.update(c=3, d=4)
#print(d)
#d["e"]=5
#print(d)
#for i in d:
#    print (d[i])
#s=set((1, [2, 3]))
#print(s)

'''
m=["a","b",1]
for i in m:
    print(i)

print(hex(255))
'''

'''
def quadratic(a, b, c):
    det=b*b-4*a*c
    if(det<0):
        print("{0}x^2+({1})x+{2}=0 无解".format(a,b,c))
    elif(det==0):
        x1=-b/(2*a)
        x2=-b/(2*a)
        print("{0}x^2+({1})x+{2}=0 的两个解均为{3}".format(a,b,c,x1))
        return x1,x2
    else:
        x1= (-b+math.sqrt(det))/(2*a)
        x2= (-b-math.sqrt(det))/(2*a)
        print("{0}x^2+({1})x+{2}=0 的两个解分别为{3}，{4}".format(a,b,c,x1,x2))
        return x1,x2

print(quadratic(1, 0, -4))
'''

'''
def multi(a,*args):
    m=1
    l=len(args)
    m=m*a
    n=0
    while l>n:
        m=m*args[n]
        n+=1
    return m

print(multi(100))

#实现c#中的trim函数的功能，Python中为strip()函数
def trim(str):
    if str[:1]==" ":
        return trim(str[1:len(str)])
    elif str[-1:]==" ":
        return trim(str[0:len(str)-1])
    else:
        return str 
# 测试:
if trim('hello  ') != 'hello':
    print('测试失败!')
elif trim('  hello') != 'hello':
    print('测试失败!')
elif trim('  hello  ') != 'hello':
    print('测试失败!')
elif trim('  hello  world  ') != 'hello  world':
    print('测试失败!')
elif trim('') != '':
    print('测试失败!')
elif trim('    ') != '':
    print('测试失败!')
else:
    print('测试成功!')  
    print('['+'hello  '+']'+'['+trim('hello  ')+']')
    print('[','  hello',']','[',trim('  hello'),']')
    print('[','  hello  world  ',']','[',trim('  hello  world  '),']')
    print('[','',']','[',trim(''),']')    
    print('[','    ',']','[',trim('    '),']')   

#找出list中的最大值和最小值
def findMinAndMax(L):
    if len(L)==0:
        return (None,None)
    else:
        min=L[0]
        max=L[0]
        for i in L:
            if i<min:
                min=i
            elif i>max:
                max=i
        return(min,max)  
# 测试
if findMinAndMax([]) != (None, None):
    print('测试失败!')
elif findMinAndMax([7]) != (7, 7):
    print('测试失败!')
elif findMinAndMax([7, 1]) != (1, 7):
    print('测试失败!')
elif findMinAndMax([7, 1, 3, 9, 5]) != (1, 9):
    print('测试失败!')
else:
    print('测试成功!')   

L=[x*x for x in range(1,10) if x%2==0] 
print(L) 

L1=[d for d in os.listdir('.')] 
print(L1)     

#大写转小写
L1 = ['Hello', 'World', 18, 'Apple', None]
L2=[i.lower() for i in L1 if isinstance(i,str)]
#测试
print(L2)
if L2 == ['hello', 'world', 'apple']:
    print('测试通过!')
else:
    print('测试失败!')
'''

#杨辉三角
def triangles():
    n=0
    while True:
        n+=1
        if n==1:           
            L=[1]
            yield L
        elif n==2:                   
            L=[1,1]
            yield L
        else:
            #z=1
            #L1=[tmpL[:n-2][i]+tmpL[1:n-1][i] for i in range(min(len(tmpL[:n-2]),len(tmpL[1:n-1])))]
            #for m in L1:
            #    tmpL[z]=m
            #    z+=1
            #tmpL.append(1)
            #L=tmpL
            #tmpL=[1]+[tmpL[n]+tmpL[n+1] for n in range(len(tmpL)-1)]+[1]
            #L=tmpL
            L=[1]+[L[n]+L[n+1] for n in range(len(L)-1)]+[1]
            yield L
#测试
n = 0
results = []
for t in triangles():
    print(t)
    results.append(t)
    n = n + 1
    if n == 10:
        break
if results == [
    [1],
    [1, 1],
    [1, 2, 1],
    [1, 3, 3, 1],
    [1, 4, 6, 4, 1],
    [1, 5, 10, 10, 5, 1],
    [1, 6, 15, 20, 15, 6, 1],
    [1, 7, 21, 35, 35, 21, 7, 1],
    [1, 8, 28, 56, 70, 56, 28, 8, 1],
    [1, 9, 36, 84, 126, 126, 84, 36, 9, 1]
]:
    print('测试通过!')
else:
    print('测试失败!')