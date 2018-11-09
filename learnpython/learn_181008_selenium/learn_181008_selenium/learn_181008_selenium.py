# -*-coding:utf-8 -*-
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.support.wait import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import time
from selenium.webdriver.support.select import Select
from logging import exception
import logging
dr=webdriver.Chrome()
p=dr.get("https://www.baidu.com/")
#print(dir(p))#dir查看p的属性和方法
#print(dr.page_source)#page_source查看源代码
inputbox=dr.find_element_by_id("kw")
inputbox.send_keys("python")
dr.find_element_by_id("su").click()
#wait=WebDriverWait(dr,10)
#wait.until(EC.visibility_of_element_located(By.CLASS_NAME,"nums_text"))
WebDriverWait(dr, 10).until(EC.visibility_of_element_located((By.CLASS_NAME, "nums_text")))#注意括号个数，否则报错
while "百度一下，你就知道" in dr.title:
    time.sleep(1)
try:
	if "python" in dr.title:
		print("true")
		#txt1=dr.find_element_by_xpath('//*[@id="s_tab"]/div/a[2]')
		txt1=dr.find_element_by_xpath('//*[@class="s_tab_inner"]/a[2]')
		txt1.click()
		WebDriverWait(dr,3).until(lambda x: x.find_element_by_class_name("search_logo"))
		dr.find_element_by_class_name("senior-search-link").click()
		WebDriverWait(dr,3).until(lambda x: x.find_element_by_class_name("titlgl"))
		s=Select(dr.find_element_by_name("sm"))
		#print(dir(dr.find_element_by_name("sm")))
		s1=Select(dr.find_element_by_name("sm")).options
		for a in s1:
			print(a.text)
		print(s.first_selected_option.text)
		s.select_by_visible_text("按相关性排序")
		print(s.first_selected_option.text)
	else:
		print("false")
except Exception as e:
	logging.exception("wrong:",e)
finally:
    dr.close()