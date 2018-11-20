import json
import requests
def pick(boxNo):
	content={'taskNo':'','SN':'','hubno':'AreaPick','outPort':''}
	content['SN']=boxNo
	str_content='['+json.dumps(content)+']'    
	#print (str_content)
	#print (type(str_content))
	url='http://172.16.5.191:9998/api/stationInfo'
	r=requests.post(url,data=str_content)
    #print(r.text)
	m=json.loads(r.text)       
	return(m['result'])