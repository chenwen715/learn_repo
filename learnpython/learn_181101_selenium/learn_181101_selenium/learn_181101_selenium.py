from selenium import webdriver
from selenium.webdriver.common.by import By
import time
import random
from selenium.webdriver.common.action_chains import ActionChains

brower=webdriver.Chrome()
brower.get("https://www.taobao.com")
time.sleep(5)
#寻找单个元素
searchb=brower.find_element_by_class_name("tb-bg")
#searchb1=brower.find_element_by_css_selector("#J_TSearchForm > div.search-button > button")
#searchb2=brower.find_element_by_xpath('//*[@id="J_TSearchForm"]/div[@class="search-button"]/button')
#searchb3=brower.find_element_by_xpath('//*[@class="btn-search tb-bg"]')
#searchb4=brower.find_element_by_css_selector("[class='btn-search tb-bg']")
#searchb5=brower.find_element_by_css_selector("button.tb-bg")
#searchb6=brower.find_element_by_css_selector(".tb-bg")
#print(dir(searchb))
print(searchb.text)
#寻找多个元素
#lis=brower.find_elements_by_css_selector(".service-bd li")
#for aa in lis:
#	print(aa.text)
#mz=brower.find_element_by_xpath("//*[@class='service-bd']/li[5]/a[1]")
#print(mz.text)
#mz.click()
inputbox=brower.find_element_by_id("q")
inputbox.clear()
inputbox.send_keys("ipad")
inputbox.clear()
inputbox.send_keys("小米")
searchb.click()
brower.back()

try:
	game=brower.find_element_by_class_name("play-normal")
	game.click()
	time.sleep(20)
	#切换到指定窗口
	for win in brower.window_handles:
		brower.switch_to_window(win)
		if brower.title=="2018狂欢城":
			break
	print(brower.title)
	stageall=brower.find_elements_by_xpath("//div[@class='khc-stage']/div")
	print("共有："+str(len(stageall)))
	stageno=random.randint(1,len(stageall))
	print("选中:"+str(stageno))
	stage=brower.find_element_by_xpath("//div[@class='khc-stage']/div[%d]"%stageno)
	buildall=brower.find_elements_by_xpath("//div[@class='khc-stage']/div[%d]/a"%stageno)
	print("共有："+str(len(buildall)))
	buildno=random.randint(1,len(buildall))
	print("选中:"+str(buildno))
	build=brower.find_element_by_xpath("//div[@class='khc-stage']/div[%d]/a[%d]"%(stageno,buildno))
	#将鼠标移至指定元素
	ActionChains(brower).move_to_element(build).perform()
	time.sleep(5)
	print(dir(build))
	build.click()
	hd=brower.find_element_by_id("banner")
	time.sleep(10)
except Exception as e:
	print(e)
finally:
	#关闭窗口
	brower.close()
	#关闭浏览器
	brower.quit()

#切换到指定窗口
def switch_to_win(driver,title):
	for handle in driver.window_handles:
		driver.switch_to_window(handle)
		if driver.title==title:
			return True 
		else:
			#返回父frame
			driver.switch_to_default_content()