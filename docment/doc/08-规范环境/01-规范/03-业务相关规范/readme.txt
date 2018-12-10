-------------------- 代码模板说明 --------------------

* 每个项目使用6个基本模块进行开发；
    owms-client:soa客户端调用工具；
    owms-common:service和dao实现层；
    owms-dto:数据库表结构model；
    owms-interface:soa接口；
    owms-service:soa服务端；
    owms-web:web项目；
* oxxx-common工程：spring和ibatis文件放到oxxx-common文件夹下面；
* oxxx-service/oxxx-web：spring-web.xml和spring-service.xml放到根目录
* 在spring-service.xml中引用其他相关spring配置文件；
* oxxx-service/oxxx-web：项目相关的vo不要放到dto中
* web.xml：不引用其他spring配置文件；
* spring-web.xml：引用拦截器等和web环境有关的配置文件；

-------------------- maven项目结构说明 --------------------

项目名称：oxxx
groupId：com.odianyun.xxx
artifactId：oxxx
子项目groupId：继承父项目的groupId
子项目artifactId：oxxx-client/common/interface/service/web
项目version格式：X.Y-SNAPSHOT
version说明：
    1. X：大版本号（对应大的功能升级）；Y：小版本号（bug修复、小的升级）
    2. 整个项目使用同一个版本号；
    3. 每次项目上线需要打一个tag；
oxxx-client/oxxx-dto/oxxx-interface：上传源代码
oxxx-common/oxxx-service/oxxx-web：不上传源代码

-------------------- doc说明 --------------------
abatorConfig.xml：用来生成代码的工具；
create.sql：创建数据库脚本；
init.sql：数据库初始化脚本；

-------------------- Archetype 生成说明 --------------------
在当前目录owms下面：
cd owms
mvn archetype:create-from-project

cd target/generated-sources/archetype
mvn install
mvn deploy
之后就可以在 工具中发现这个 archetype 了


<dependency>
  <groupId>com.odianyun.owms</groupId>
  <artifactId>owms-archetype</artifactId>
  <version>2.0-SNAPSHOT</version>
</dependency>
