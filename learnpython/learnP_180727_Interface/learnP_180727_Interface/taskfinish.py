import json
import requests
def pick(boxNo):
	content={'taskNo':'','SN':'','hubno':'AreaPick','outPort':''}
	content['SN']=boxNo
	str_content='['+json.dumps(content)+']'    
	#print (str_content)
	#print (type(str_content))
	url='http://172.16.5.51:9100/KSWCS.API/ESB/HHTOXN/stationInfo'
	r=requests.post(url,data=str_content)
    #print(r.text)
	m=json.loads(r.text)       
	return(m['StatusCode'])
	
	
def boxToShelf(taskNo,boxNo,hubNo,shelfNo):
	content={'taskNo':'','SN':'','hubno':'','shelfNo':'','location':'','remark':'','opTime':''}
	content['taskNo']=taskNo
	content['SN']=boxNo
	content['hubno']=hubNo
	content['shelfNo']=shelfNo
	str_content='['+json.dumps(content)+']'    
	#print (str_content)
	#print (type(str_content))
	url='http://172.16.5.51:9100/KSWCS.API/ESB/HHTOXN/loadTasksResult'
	r=requests.post(url,data=str_content)
    #print(r.text)
	m=json.loads(r.text)       
	return(m['StatusCode'])