#class Student(object):
#    count=0
#    name=[]
#    def __init__(self, name):
#        self.name=name
#        if len(name)!=0:
#            Student.count+=1
#            Student.name.append(self.name)

## 测试:
#if Student.count != 0:
#    print('测试失败!')
#else:
#    bart = Student('Bart')
#    if Student.count != 1:
#        print('测试失败!')
#    else:
#        lisa = Student('Bart')
#        if Student.count != 2:
#            print('测试失败!')
#        else:
#            print('Students:', Student.count)
#            print('测试通过!')

#class Screen(object):

#    @property
#    def width(self):
#        return self._width
#    @width.setter
#    def width(self,value):
#        self._width=value
#    @property
#    def height(self):
#        return self._height   
#    @height.setter
#    def height(self,value):
#        self._height=value
#    @property
#    def resolution(self):
#        return self.height*self.width

## 测试:
#s = Screen()
#s.width = 1024
#s.height = 768
#print('resolution =', s.resolution)
#if s.resolution == 786432:
#    print('测试通过!')
#else:
#    print('测试失败!')

#from enum import Enum

#Month=Enum("month",("Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"))
#for name,member in Month.__members__.items():
#    print(name, '=>', member, ',', member.value)

#from enum import Enum,unique
#@unique
#class Gender(Enum):
#    Male = 0
#    Female = 1

#class Student(object):
#    def __init__(self, name, gender):
#        self.name = name
#        self.gender = Gender(gender)


#bart = Student('Bart', Gender.Male)
#if bart.gender == Gender.Male:
#    print('测试通过!')
#else:
#    print('测试失败!')