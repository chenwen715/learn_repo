#_*_coding: utf-8_*_
import re
html='''<div id="songs-list>
<h2 class="title">经典老歌</h2>
<p class="introduction">
经典老歌列表
</p>
<ul id="list" class="list-group">
    <li data-view="2">一路上有你</li>
    <li data-view="7">
    <a href="/2.mp3" singer="任贤齐">沧海一声笑</a>
    </li>
    <li data-view="4" class="active">
    <a href="/3.mp3" singer="齐秦">往事随风</a>
    </li>
    <li data-view="6"><a href="/4.mp3" singer="beyond">光辉岁月</a></li>
    <li data-view="5"><a href="/5.mp3" singer="陈慧林">记事本</a></li>
    <li data-view="5">
    <a href="/3.mp3" singer="邓丽君"><i class="fa fa-user"></i>但愿人长久</a>
    </li>
    </ul>
<div>'''

#re.search查找，能用search就不要用match
#result=re.search('<li.*?active.*?singer="(.*?)">(.*?)</a>',html,re.S)
#re.match从字符串起始位置开始查找，如果不是起始位置匹配的话，match()就返回none 
result=re.match('<div.*?active.*?singer="(.*?)">(.*?)</a>',html,re.S)
#re.sub使用第二个参数替换第三个参数中的指定字符串（第一个参数），下例为去除html中的a标签
#html=re.sub('<a.*?>|</a>','',html)
if result:
    #print(result.group(1),result.group(2))
	#result[0]为整个正则表达式筛选出来的内容，result[1]为第一个小括号中的内容
	print(result[1],result[2])