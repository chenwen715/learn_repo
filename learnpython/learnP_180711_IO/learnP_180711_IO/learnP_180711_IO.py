import json
d=dict(name="小小",age=12,score=88)
j=json.dumps(d,ensure_ascii=False)
print(j)


#j1='[{\
#"taskNo":"201711140001",\
#"taskType":"OUT",\
#"taskState":"NORMAL",\
#"priority":4,\
#"OUTNO":"0001",\
#"planningNo":"0000",\
#"SN":"CX170114000001",\
#"hubno":"AA-01-4A",\
#"isUnpackTray":"TRUE",\
#"isBoxLable":"FALSE",\
#"isMinpackLable":"FALSE",\
#"isPipeline":"FALSE",\
#"SN_OLD":"CX170113000001",\
#"opTime":"2017-09-10 16:31:16"\
#}]'
#print(json.loads(j1))
#class Student(object):
#    def __init__(self, name, age, score):
#        self.name = name
#        self.age = age
#        self.score = score

#def dict2student(d):
#    return Student(d['name'], d['age'], d['score'])
#json_str = '{"age": 20, "score": 88, "name": "小小"}'
#s=json.loads(json_str, object_hook=dict2student)
#print(s.name)