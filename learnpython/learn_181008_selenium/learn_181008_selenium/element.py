# -*- coding:utf-8 -*-
from selenium.webdriver.support.ui import WebDriverWait

class BasePageElement(object):
	def __set__(self,obj,value):
		driver=obj.driver
		WebDriverWait(driver,100).until(lambda dr:dr.find_element_by_name(self.locator))
		driver.find_element_by_name(self.locator).send_keys(value)

	def __get__(self,obj):
		driver=obj.driver
		WebDriverWait(driver,100).until(lambda dr:dr.find_element_by_name(self.loactor))
		element=driver.find_element_by_name(self.loactor)
		return element.get_attribute("value")
