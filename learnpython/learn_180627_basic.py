#this is comment to help understand
'''
L = [
    ['Apple', 'Google', 'Microsoft'],
    ['Java', 'Python', 'Ruby', 'PHP'],
    ['Adam', 'Bart', 'Lisa']
]
print(L[0][0])
'''

'''
#条件语句if...elif...else
age=10
if age>18:
    print("your age is",age)
    print("adult")
elif age<12:
    print("kid")
else:
    print("teenager")

birth=input("please input a year :")
year=int(birth)
if (year%4==0 and year%100!=0)or(year%400==0):
    print(year, "是闰年")
else:
    print(year, "不是闰年")
'''

'''
#循环语句for...in/while(break,continue)
print(range(101))
sum=0
for a in list(range(101)):
    sum+=a
print(sum)

L = ['Bart', 'Lisa', 'Adam']
for name in L:
    print("hello,",name)
n=0
while n<len(L):
    print("hello",L[n])
    n+=1
'''

'''
#dict key-value相对于list是空间换时间
dict={"nancy":80,"lucy":98,"tom":60}
print(dict["nancy"])
#set 不重复的key
s=set([1,3,5,7,9,6,5,3,7])
print(s)
s1=set((1,2,3))
print(s1)
'''

n1=255
n2=1000
print(hex(n1))
print(hex(n2))
hex()
