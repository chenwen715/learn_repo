from _functools import reduce
from audioop import reverse
'''
def normalize(name):
    return name[0].upper()+name[1:].lower()

#test
L1 = ['adam', 'LISA', 'barT']
L2 = list(map(normalize, L1))
print(L2)


def mul(x,y):
    return x*y

def prod(L):
    if len(L)==1:
        return L[0]
    else:
        return reduce(mul,L)

#test
print('3 * 5 * 7 * 9 =', prod([3, 5, 7, 9]))
if prod([3, 5, 7, 9]) == 945:
    print('测试成功!')
else:
    print('测试失败!')


dic={'0':0,'1':1,'2':2,'3':3,'4':4,'5':5,'6':6,'7':7,'8':8,'9':9}
def chrtonum(x):
    return dic[x]
def zheshu(x,y):
    return 10*x+y
def xiaoshu(x,y):
    return 0.1*x+y
def str2float(s):
    if('.'in s):
        L=s.split('.')
        return reduce(zheshu,map(chrtonum,L[0]))+reduce(xiaoshu,map(chrtonum,L[1][::-1]))/10
    else:
        return s

#test
print('str2float(\'123.456\') =', str2float('123.456'))
if abs(str2float('123.456') - 123.456) < 0.00001:
    print('测试成功!')
else:
    print('测试失败!')

#name=" 123"
#print(name and name.strip())

#求1000以内的素数
L=list(range(2,1001))
def notDivide(n):
    return lambda x: x % n > 0
def isSuShu(x):
    n=0
    while(n<len(x)):
        x=x[0:n+1]+list(filter(notDivide(x[n]),x[n+1:]))
        n+=1
    return x
print(isSuShu(L))

#求1000以内的回数
#复杂
#def is_palindrome(n):
#    strn=str(n)
#    if(len(strn)%2==0):
#        i=int(len(strn)/2)
#    else:
#        i=int((len(strn)-1)/2)
#    while i>0:
#        if strn[i-1]!=strn[len(strn)-i]:
#            i-=1
#            return 0
#        else:
#            i-=1
#    return 1

#简洁
def is_palindrome(n):
    s=str(n)
    return s==s[::-1]  

output = filter(is_palindrome, range(1, 1000))
print('1~1000:', list(output))
if list(filter(is_palindrome, range(1, 200))) == [1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 22, 33, 44, 55, 66, 77, 88, 99, 101, 111, 121, 131, 141, 151, 161, 171, 181, 191]:
    print('测试成功!')
else:
    print('测试失败!')
'''

#L = [('Bob', 75), ('Adam', 92), ('Bart', 66), ('Lisa', 88)]
#def by_name(t):
#    return t[0]
#L2 = sorted(L, key=by_name)
#print(L2)
#def by_score(t):
#    return t[1]
#L3 = sorted(L, key=by_score,reverse=True)
#print(L3)

