import unittest
import string


class LearnTest(unittest.TestCase):

	def test_split(self):
		s="123 456"
		self.assertEqual(s.split(),["123","456"])
		a=1	

		#调用非本文件的方法（函数）时，用以下写法
		with self.assertRaises(AttributeError):
			s.split()#运行出错，因为不会抛出该异常
		with self.assertRaises(AttributeError):
			a.split()
		with self.assertRaises(TypeError):
			s.split(1)

		#调用本文件的方法（函数）时，可使用以下写法或上面的写法。第一个参数为错误类型，第二个参数为函数名，后面参数为函数的参数
		#self.assertRaises(AttributeError,splits,"a b")#运行出错，因为不会抛出该异常	
		#self.assertRaises(AttributeError,splits,1,1)	
		#self.assertRaises(TypeError,splits,"a b",1)	

def splits(s,c=" "):
	if not isinstance(s,str):
		raise AttributeError("wrong param type")	
	return s.split(c)


if __name__=="__main__":
	#suit=unittest.TestSuite()
	#testload=unittest.TestLoader()
	#suit=testload.loadTestsFromTestCase(LearnTest)
	#runner=unittest.TextTestRunner()
	#runner.run(suit)
	unittest.main()