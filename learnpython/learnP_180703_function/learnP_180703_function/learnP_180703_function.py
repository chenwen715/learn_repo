#def createCounter():
#    i=0
#    def counter():
#        nonlocal i
#        i=i+1
#        return i
#    return counter

##test
#counterA = createCounter()
#print(counterA(), counterA(), counterA(), counterA(), counterA()) # 1 2 3 4 5
#counterB = createCounter()
#if [counterB(), counterB(), counterB(), counterB()] == [1, 2, 3, 4]:
#    print('测试通过!')
#else:
#    print('测试失败!')

#def build(x, y):
#    return lambda: x * x + y * y
#a=build(1,2)
#print(a())

#def build1(x, y):
#    return x * x + y * y
#print(build1(1, 2))

#def is_odd(n):
#    return n % 2 == 1

#L = list(filter(is_odd, range(1, 20)))
#print(L)

#L1=list(filter(lambda x: x%2==1,range(1,20)))
#print(L1)

#import time, functools
#def metric(fn):
#    @functools.wraps(fn)
#    def wrapper(*args, **kw):
#        startTime = time.time()
#        fn(*args, **kw)
#        diffTime = (time.time() - startTime)*1000
#        print('%s 运行了 %s ms' % (fn.__name__, diffTime))
#        return fn(*args, **kw)
#    return wrapper


#def log(func):
#    @functools.wraps(func)
#    def wrapper(*args,**kw):
#        print('begin call:%s' %func.__name__)
#        func(*args,**kw)
#        print('end call:%s' %func.__name__)
#        return func(*args,**kw)
#    return wrapper
## 测试
##@metric
#@log
#def fast(x, y):
#    time.sleep(0.0012)
#    return x + y;

##@metric
#@log
#def slow(x, y, z):
#    time.sleep(0.1234)
#    return x * y * z;

##f = fast(11, 22)
##s = slow(11, 22, 33)
##if f != 33:
##    print('测试失败!')
##elif s != 7986:
##    print('测试失败!')
#print(fast(11, 22))

#print(int('12345', 10))

#from functools import reduce
#import ast

#def str2num(s):
#    return ast.literal_eval(s.strip())


#def calc(exp):
#    ss = exp.split('+')
#    ns = map(str2num, ss)
#    return reduce(lambda acc, x: acc + x, ns)

#def main():
#    r = calc('100 + 200 + 345')
#    print('100 + 200 + 345 =', r)
#    r = calc('99 + 88 + 7.6')
#    print('99 + 88 + 7.6 =', r)

#main()

#def fact(n):
#    '''
#    Calculate 1*2*...*n

#    >>> fact(1)
#    1
#    >>> fact(10)
#    ?
#    >>> fact(-1)
#    ?
#    '''
#    if n < 1:
#        raise ValueError()
#    if n == 1:
#        return 1
#    return n * fact(n - 1)

#print(fact(-1))

class Student(object):
    def __init__(self, name, gender):
        self.__name = name
        self.__gender = gender

    def get_gender(self):
        return self.__gender
    def set_gender(self,gender):
        self.__gender=gender

# 测试:
bart = Student('Bart', 'male')
if bart.get_gender() != 'male':
    print('测试失败!')
else:
    bart.set_gender('female')
    if bart.get_gender() != 'female':
        print('测试失败!')
    else:
        print('测试成功!')