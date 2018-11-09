from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver import ActionChains
from selenium.common.exceptions import NoAlertPresentException
import time
import random
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC

def main():
	driver=webdriver.Chrome()
	driver.get("http://www.runoob.com/try/try.php?filename=jqueryui-api-droppable")
	driver.maximize_window()
	#print(type(driver))

	#rep=driver.find_element_by_xpath("/html/body/div[1]/div/div[1]/div/div[2]/div/div[6]/div[1]/div/div/div/div[5]/pre[17]/span/span[2]")
	#rep.text="300px"
	#rep1=driver.find_element_by_xpath("/html/body/div[1]/div/div[1]/div/div[2]/div/div[6]/div[1]/div/div/div/div[5]/pre[18]/span/span[2]")
	#rep1.clear()
	#rep1.send_keys("300px")
	#driver.find_element_by_id("submitBTN").click()

	frame=driver.find_element_by_id("iframeResult")
	frames=frame.size
	print(frame.size)#{'height':629,'width':870}
	count=0
	a=True
	while a:
		WebDriverWait(driver,10).until(EC.frame_to_be_available_and_switch_to_it((By.ID,"iframeResult")))
		#driver.switch_to_frame("iframeResult")
		start=driver.find_element_by_id("draggable")
		startl=start.location
		starts=start.size
		#print(starts)
		end=driver.find_element_by_id("droppable")
		endl=end.location
		ends=end.size
		#print(endl)#{'x':250,'y':0}
		#print(ends)#{'height':145,'width':145}
		x=random.randint(-startl['x'],frames['width']-starts['width']-startl['x'])
		y=random.randint(-startl['y'],frames['height']-starts['height']-startl['y'])
		#ActionChains(driver).drag_and_drop(start,end).perform()
		ActionChains(driver).drag_and_drop_by_offset(start,x,y).perform()
		count+=1
		#print(type(driver))
		try:
			al=driver.switch_to_alert()
			print(al.text+str(count))
			time.sleep(2)
			al.accept()
			a=False
		except (NoAlertPresentException ,AttributeError):
			print("times:"+str(count))
		time.sleep(2)		
		driver.switch_to_default_content()
		WebDriverWait(driver,10).until(EC.frame_to_be_available_and_switch_to_it((By.ID,"iframeResult")))
		#driver.switch_to_frame("iframeResult")
		startp=driver.find_element_by_id("draggable")
		print(startp.location)
		driver.switch_to_default_content()
	driver.close()


main()