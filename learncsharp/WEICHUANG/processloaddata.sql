USE [WEIC]
GO
/****** Object:  StoredProcedure [dbo].[analysisUnloadData]    Script Date: 2018/11/9 15:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[analysisUnloadData] 
	
AS
BEGIN
declare @tmpTableName nvarchar(100),@tmpTableNameNew nvarchar(100),@tmpLine nvarchar(50),@sql nvarchar(max),@sql1 nvarchar(max),@tmpTableNameNew1 nvarchar(100),@tmpTableName1 nvarchar(100),
@tmptime nvarchar(50),@tmpca nvarchar(50),@tmpqu int,@tmptime1 nvarchar(50),@tmpqa int,@tmpql int,@tmpqs int,@TI int
	DECLARE cursor4 CURSOR FOR 
		SELECT DISTINCT LINE FROM dbo.T_BASE_SimulateData ;
	OPEN cursor4;
	FETCH NEXT FROM cursor4 into @tmpLine
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @TI=2
		while @TI>0 begin
		IF @TI>1 BEGIN
			set @tmpTableName='dbo.T_BASE_SimulateData_'+@tmpLine+'_S_3'
			set @tmpTableNameNew='T_'+@tmpLine+'_S_Analysis'
		end
		ELSE BEGIN
			set @tmpTableName='dbo.T_BASE_SimulateData_'+@tmpLine+'_L_3'
			set @tmpTableNameNew='T_'+@tmpLine+'_L_Analysis'
		END
				
		set @sql=''
		set @sql+=' IF OBJECT_ID ('''+@tmpTableNameNew+''') IS NOT NULL '
		set @sql+=' DROP TABLE '+ @tmpTableNameNew
		--set @sql+=' CREATE TABLE '+@tmpTableNameNew
		--set @sql+=' (
		--	Times				NVARCHAR (20),   
		--	CATEGORY			NVARCHAR (20),
		--	BOXNO				INT) '
		exec(@sql)

		--SET @sql1='SELECT row_number() OVER(ORDER BY CAST(substring(TIMES,2,len(TIMES)) as INT)) AS ID, TIMES,category,count(category) AS boxNo INTO '+@tmpTableNameNew +' FROM '+ @tmpTableName
		--+' WHERE isnull(remarks,'''')='''' GROUP BY Times,category '
		SET @sql1='SELECT row_number() OVER(ORDER BY CAST(substring(TIMES,2,len(TIMES)) as INT)) AS ID, TIMES,category,count(category) AS boxNo INTO '+@tmpTableNameNew +' FROM '+ @tmpTableName
		+'  GROUP BY Times,category '
		EXEC(@sql1)
		SET @TI-=1
		END
	FETCH NEXT FROM cursor4 into @tmpLine
	END
	CLOSE cursor4; DEALLOCATE cursor4;
	
	set @tmpTableNameNew1='dbo.T_Analys'
	set @sql=''
	set @sql+=' IF OBJECT_ID ('''+@tmpTableNameNew1+''') IS NOT NULL '
	set @sql+=' DROP TABLE '+ @tmpTableNameNew1
	set @sql+=' CREATE TABLE '+@tmpTableNameNew1
	set @sql+=' (
		DATA				NVARCHAR (20),   
		TOTALNUMBER			INT,
		LNUMBER				INT,
		SNUMBER				INT) '
	exec(@sql)
	
	DECLARE cursor6 CURSOR FOR 
		SELECT name FROM sysobjects WHERE type ='u' AND name LIKE '%analysis';
	OPEN cursor6;
	FETCH NEXT FROM cursor6 into @tmpTableName1
	WHILE @@FETCH_STATUS = 0
	BEGIN		
		set @sql=''
		set @sql+='DECLARE cursor7 CURSOR FOR 
		select TIMES, category, boxNo from ' +@tmpTableName1;
		exec(@sql)
		OPEN cursor7;
		FETCH NEXT FROM cursor7 into @tmptime,@tmpca,@tmpqu
		WHILE @@FETCH_STATUS = 0
		BEGIN		
			SELECT @tmptime1=DATA,@tmpqa=TOTALNUMBER,@tmpql=LNUMBER, @tmpqs=SNUMBER FROM dbo.T_Analys where DATA=@tmptime
			if isnull(@tmptime1,'')<>''
			begin
				set @tmpqa+=@tmpqu
				if @tmpca='小件' begin
					set @tmpqs+=@tmpqu
				end
				else begin
					set @tmpql+=@tmpqu
				end
				update dbo.T_Analys set TOTALNUMBER=@tmpqa,LNUMBER=@tmpql,SNUMBER=@tmpqs where DATA=@tmptime 
			end
			else				
			begin
				if @tmpca='小件' begin
					INSERT INTO dbo.T_Analys (DATA,TOTALNUMBER,LNUMBER,SNUMBER) VALUES(@tmptime,@tmpqu,0,@tmpqu)
				end
				else begin
					INSERT INTO dbo.T_Analys (DATA,TOTALNUMBER,LNUMBER,SNUMBER) VALUES(@tmptime,@tmpqu,@tmpqu,0)
				end				
			END
			set @tmptime1=''
			FETCH NEXT FROM cursor7 into @tmptime,@tmpca,@tmpqu
		END
		CLOSE cursor7; DEALLOCATE cursor7;
		FETCH NEXT FROM cursor6 into @tmpTableName1
	END
	CLOSE cursor6; DEALLOCATE cursor6;
END

GO
/****** Object:  StoredProcedure [dbo].[deletetable]    Script Date: 2018/11/9 15:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[deletetable]

AS
BEGIN
	declare @tmpLine nvarchar(50),@tmpTableName nvarchar(100),@sql nvarchar(max),@sql1 nvarchar(max)
	DECLARE cursor8 CURSOR FOR 
		SELECT DISTINCT LINE FROM dbo.T_BASE_SimulateData;
	OPEN cursor8;
	FETCH NEXT FROM cursor8 into @tmpLine
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		DECLARE cursor9 CURSOR FOR 
			SELECT name FROM sysobjects WHERE type ='u' AND name LIKE '%'+@tmpLine+'%';
		OPEN cursor9;
		FETCH NEXT FROM cursor9 into @tmpTableName
		WHILE @@FETCH_STATUS = 0
		BEGIN
			set @sql=''
			set @sql+=' IF OBJECT_ID ('''+@tmpTableName+''') IS NOT NULL '
			set @sql+=' DROP TABLE '+ @tmpTableName
			exec(@sql)
			FETCH NEXT FROM cursor9 into @tmpTableName
		end
		CLOSE cursor9; DEALLOCATE cursor9;
		FETCH NEXT FROM cursor8 into @tmpLine
	end
	CLOSE cursor8; DEALLOCATE cursor8;
	DELETE FROM dbo.T_TMP_DATA
	DELETE FROM dbo.T_Analys
			
END

GO
/****** Object:  StoredProcedure [dbo].[GetBoxNo]    Script Date: 2018/11/9 15:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetBoxNo]
@tmpFIRSTPICKTIME nvarchar(50),
@tmpBoxNo nvarchar(50) OUT
AS
BEGIN
	DECLARE @ExecText2 nvarchar(100), @tmpStr nvarchar(50),@tmpYY nvarchar(50),@tmpMM nvarchar(50),@tmpDD nvarchar(50),@tmpNumber INT ,@tmpStr1 nvarchar(50),@tmpNumber1 INT
	--SELECT @tmpYY=SUBSTRING(CAST(FIRSTPICKTIME AS NVARCHAR),7,CHARINDEX(' ',FIRSTPICKTIME)+1),
	--@tmpMM=SUBSTRING(CAST(FIRSTPICKTIME AS NVARCHAR),4,CHARINDEX(' ',FIRSTPICKTIME)),
	--@tmpDD=SUBSTRING(CAST(FIRSTPICKTIME AS NVARCHAR),0,CHARINDEX(' ',FIRSTPICKTIME))
	--FROM dbo.T_BASE_SimulateData_A4A_2 
BEGIN TRY
	SET @tmpStr1=''
	SET @tmpNumber=0
	--SET @tmpYY=SUBSTRING(CAST(@tmpFIRSTPICKTIME AS NVARCHAR),7,CHARINDEX(' ',@tmpFIRSTPICKTIME)+1)
	--SET @tmpMM=SUBSTRING(CAST(@tmpFIRSTPICKTIME AS NVARCHAR),0,CHARINDEX(' ',@tmpFIRSTPICKTIME))
	--SET @tmpDD=SUBSTRING(CAST(@tmpFIRSTPICKTIME AS NVARCHAR),4,CHARINDEX(' ',@tmpFIRSTPICKTIME))
	--SET @tmpStr=REPLICATE('0', 4 - LEN(LTRIM(RTRIM(@tmpYY)))) + LTRIM(RTRIM(@tmpYY))+REPLICATE('0', 2 - LEN(LTRIM(RTRIM(@tmpMM)))) + LTRIM(RTRIM(@tmpMM))
	--+REPLICATE('0', 2 - LEN(LTRIM(RTRIM(@tmpDD)))) + LTRIM(RTRIM(@tmpDD))
	SET @tmpStr=REPLICATE('0', 6 - LEN(LTRIM(RTRIM(@tmpFIRSTPICKTIME)))) + LTRIM(RTRIM(@tmpFIRSTPICKTIME))
	SELECT @tmpStr1=DATA,@tmpNumber=NUMBER FROM dbo.T_TMP_DATA WHERE DATA=@tmpFIRSTPICKTIME
	IF ISNULL(@tmpStr1,'')='' BEGIN
		SET @tmpNumber1=1
		SET @tmpBoxNo='C'+@tmpStr+REPLICATE('0', 6 - LEN(cast(@tmpNumber1 as nvarchar))) + cast(@tmpNumber1 as nvarchar)
		INSERT INTO dbo.T_TMP_DATA (DATA, NUMBER) VALUES (@tmpFIRSTPICKTIME, @tmpNumber1)
	END 
	ELSE
	BEGIN
		SET @tmpNumber1=@tmpNumber+1
		SET @tmpBoxNo='C'+@tmpStr+REPLICATE('0', 6 - LEN(cast(@tmpNumber1 as nvarchar))) + cast(@tmpNumber1 as nvarchar)
		UPDATE dbo.T_TMP_DATA SET NUMBER=@tmpNumber1 WHERE DATA=@tmpFIRSTPICKTIME
	END
END TRY
BEGIN CATCH
	SET @ExecText2=ERROR_MESSAGE(); RAISERROR(@ExecText2,16,1);
END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[manageUnloadData_split]    Script Date: 2018/11/9 15:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[manageUnloadData_split]
	
AS
BEGIN
	declare @tmpLine nvarchar(50),@tmpTableName nvarchar(100),@sql nvarchar(max),@sql1 nvarchar(max),@tmpTableName1 nvarchar(100)
	DECLARE cursor2 CURSOR FOR 
		SELECT DISTINCT LINE FROM dbo.T_BASE_SimulateData;
	OPEN cursor2;
	FETCH NEXT FROM cursor2 into @tmpLine
	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @sql=''
		set @sql1=''
		set @tmpTableName='dbo.T_BASE_SimulateData_'+@tmpLine+'_S'

		set @sql+=' IF OBJECT_ID ('''+@tmpTableName+''') IS NOT NULL '
		set @sql+=' DROP TABLE '+ @tmpTableName
		set @sql+=' CREATE TABLE '+@tmpTableName
		set @sql+=' (
			MO                  NVARCHAR (255),
			FIRSTPICKTIME       DATETIME,
			[ simulation time]  DATETIME,
			LOT                 FLOAT,
			CPN                 NVARCHAR (255),
			CATE            NVARCHAR (255),
			DESCRIPTION         NVARCHAR (255),
			QUANTITY            FLOAT,
			LINE                NVARCHAR (255),
			CUSTOMERPROCESSTYPE NVARCHAR (255),
			[关务备案品名]      NVARCHAR (255),
			vendor              NVARCHAR (255),
			CATEGORY			NVARCHAR (255)
			) '
			
		set @tmpTableName1='dbo.T_BASE_SimulateData_'+@tmpLine+'_L'
		set @sql+=' IF OBJECT_ID ('''+@tmpTableName1+''') IS NOT NULL '
		set @sql+=' DROP TABLE '+ @tmpTableName1
		set @sql+=' CREATE TABLE '+@tmpTableName1
		set @sql+=' (
			MO                  NVARCHAR (255),
			FIRSTPICKTIME       DATETIME,
			[ simulation time]  DATETIME,
			LOT                 FLOAT,
			CPN                 NVARCHAR (255),
			CATE				NVARCHAR (255),
			DESCRIPTION         NVARCHAR (255),
			QUANTITY            FLOAT,
			LINE                NVARCHAR (255),
			CUSTOMERPROCESSTYPE NVARCHAR (255),
			[关务备案品名]      NVARCHAR (255),
			vendor              NVARCHAR (255),
			CATEGORY			NVARCHAR (255)
			) '
		exec(@sql)
		set @sql1+=' insert into '+ @tmpTableName+' SELECT * FROM dbo.T_BASE_SimulateData where Line='''+ @tmpLine+ ''' AND CATEGORY=''小件'' '
		set @sql1+=' insert into '+ @tmpTableName1+' SELECT * FROM dbo.T_BASE_SimulateData where Line='''+ @tmpLine+ ''' AND CATEGORY=''大件'' '
		exec(@sql1)
		FETCH NEXT FROM cursor2 into @tmpLine
	END
	CLOSE cursor2; DEALLOCATE cursor2;
END

GO
/****** Object:  StoredProcedure [dbo].[manageUnloadData_split1]    Script Date: 2018/11/9 15:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[manageUnloadData_split1]
	
AS
BEGIN
	declare @ExecText1  nvarchar(100),@tmpLine nvarchar(50),@tmpTableName nvarchar(100),@tmpTableNameNew nvarchar(100),@tmpTableNameNew1 nvarchar(100),@sql nvarchar(max),
	@sql1 nvarchar(max),@n INT,@nn INT,@TI INT
BEGIN TRY
	DELETE FROM dbo.T_TMP_DATA
	DECLARE cursor2 CURSOR FOR 
		SELECT DISTINCT LINE FROM dbo.T_BASE_SimulateData WHERE LINE='A1B';
	OPEN cursor2;
	FETCH NEXT FROM cursor2 into @tmpLine
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @TI=2
		WHILE @TI>0 BEGIN

		set @n=1
		set @sql=''
		IF @TI>1 BEGIN
		set @tmpTableName='dbo.T_BASE_SimulateData_'+@tmpLine+'_S'
		END
		ELSE
		BEGIN
		set @tmpTableName='dbo.T_BASE_SimulateData_'+@tmpLine+'_L'
		END
		
		set @tmpTableNameNew=@tmpTableName+'_1'

		set @sql+=' IF OBJECT_ID ('''+@tmpTableNameNew+''') IS NOT NULL '
		set @sql+=' DROP TABLE '+ @tmpTableNameNew
		set @sql+=' CREATE TABLE '+@tmpTableNameNew
		set @sql+=' (
			MO                  NVARCHAR (100),
			LOT                 FLOAT,
			FIRSTPICKTIME       DATETIME '
		while(@n<801) begin
			set @sql+=' ,T'+CAST(@n AS nvarchar)+'	INT '
			set @n+=1
		end
		set @sql+=' )'
		exec(@sql)

		set @sql=''
		set @tmpTableNameNew1=@tmpTableName+'_2'
		set @sql+=' IF OBJECT_ID ('''+@tmpTableNameNew1+''') IS NOT NULL '
		set @sql+=' DROP TABLE '+ @tmpTableNameNew1
		set @sql+=' CREATE TABLE '+@tmpTableNameNew1
		set @sql+=' (
			Times				NVARCHAR (20),   
			MO                  NVARCHAR (100),
			FIRSTPICKTIME       DATETIME,
			LOT                 FLOAT,
			CPN					NVARCHAR (100),
			TOTALQUANTITY		FLOAT,
			OUTQUANTITY			FLOAT,
			RATE				INT,
			LINE				NVARCHAR (20),
			CATEGORY			NVARCHAR (20),
			BOXORPALLETNO		NVARCHAR (50),
			REMARKS				NVARCHAR (255),
			CREATETIME          DATETIME )'
		exec(@sql)

		set @sql1='insert into '+ @tmpTableNameNew+' (MO,LOT,FIRSTPICKTIME) SELECT DISTINCT MO,LOT,FIRSTPICKTIME FROM ' +@tmpTableName+' ORDER BY FIRSTPICKTIME'
		exec(@sql1)
		SET @nn=@@ROWCOUNT

		EXEC manageUnloadData_split2 
		@tmpTableName=@tmpTableName
		,@tmpTableNameNew=@tmpTableNameNew
		,@tmpLine=@tmpLine
		
		SET @TI-=1

		END
		FETCH NEXT FROM cursor2 into @tmpLine
	END
	CLOSE cursor2; DEALLOCATE cursor2;
END TRY
BEGIN CATCH
	CLOSE cursor2; DEALLOCATE cursor2;
	SET @ExecText1=ERROR_MESSAGE(); RAISERROR(@ExecText1,16,1);
END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[manageUnloadData_split2]    Script Date: 2018/11/9 15:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[manageUnloadData_split2]
@tmpTableName nvarchar(100)
,@tmpTableNameNew nvarchar(100),
@tmpLine nvarchar(100)
AS
BEGIN
	declare @ExecText nvarchar(500),@tmpMO nvarchar(50),@tmpLOT FLOAT,@tmpTime datetime ,@sql nvarchar(max),@sql1 nvarchar(max),@tmpTableNameNew1 nvarchar(100),
	@n INT,--列
	@n1 INT,--tmp
	@n2 INT,@n3 INT,@n4 INT,@n5 INT,@number int,
	@n6 INT--单位   
	SET @n=1
	
	--SET @n6=75
BEGIN TRY
	set @sql=''
	SET @n1=0
	SET @n4=0
	set @sql1='DECLARE cursor1 CURSOR FOR SELECT DISTINCT MO,LOT,FIRSTPICKTIME FROM ' +@tmpTableName+' ORDER BY FIRSTPICKTIME'
	exec(@sql1)
	OPEN cursor1;
	FETCH NEXT FROM cursor1 into @tmpMO,@tmpLOT,@tmpTime
	WHILE @@FETCH_STATUS = 0
	BEGIN
	IF @tmpLine='A5C' OR @tmpLine='A5D' OR @tmpLine='ACL' OR @tmpLine='ADL' OR @tmpLine='AEL' OR @tmpLine='AFL' BEGIN
		SET  @n6=100
	END
	ELSE BEGIN
		SET  @n6=120
	END
	IF CHARINDEX('_L',@tmpTableName)>0
	BEGIN
		SET  @n6/=2
	END
	ELSE BEGIN
		SET  @n6*=2
	END

	SET @n4+=@tmpLOT
	SET @n2=@n4/@n6
	SET @n5=@n2
	SET @n3=@n4%@n6	
	
	IF @n2>0 BEGIN 
		DECLARE @FLAG BIT
		SET @FLAG=0
		IF @n2>1 BEGIN
			WHILE @n2>0 BEGIN
				IF @FLAG=0 BEGIN
					--IF @n6-@n1<>0 
					--BEGIN
						SET @sql+=' UPDATE '+ @tmpTableNameNew +' SET T'+CAST(@n AS nvarchar)+ ' ='+CAST((@n6-@n1) AS nvarchar) + ' WHERE MO= '''+@tmpMO+''''
						SET @FLAG=1	
						SET @number=@n6-@n1
						EXEC manageUnloadData_split4  --manageUnloadData_split3
						@tmpMO=@tmpMO,
						@n=@n,
						@number=@number,
						@tmpTableName=@tmpTableName
					--END				
				END
				ELSE BEGIN
					--IF @n<>0
					--BEGIN
						SET @sql+=' UPDATE '+ @tmpTableNameNew +' SET T'+CAST(@n AS nvarchar)+ ' ='+ CAST(@n6 AS nvarchar)+ 'WHERE MO= '''+@tmpMO+''''
						SET @number=@n6
						EXEC manageUnloadData_split4
						@tmpMO=@tmpMO,
						@n=@n,
						@number=@number,
						@tmpTableName=@tmpTableName
					--END
				END
				SET @n2-=1
				SET @n+=1
			END
			set @FLAG=0
		END
		IF @n2=1 BEGIN
			SET @sql+=' UPDATE '+ @tmpTableNameNew +' SET T'+CAST(@n AS nvarchar)+ ' ='+CAST((@n6-@n1) AS nvarchar) + ' WHERE MO= '''+@tmpMO+''''
			SET @number=@n6-@n1
			EXEC manageUnloadData_split4
			@tmpMO=@tmpMO,
			@n=@n,
			@number=@number,
			@tmpTableName=@tmpTableName
			SET @n2-=1
			SET @n+=1
		END 
		SET @n4=@n4-@n6*@n5
		IF @n4<>0 BEGIN
			SET @sql+=' UPDATE '+ @tmpTableNameNew +' SET T'+CAST(@n AS nvarchar)+ ' ='+CAST(@n4 AS nvarchar) + ' WHERE MO= '''+@tmpMO+''''
			SET @number=@n4
			EXEC manageUnloadData_split4
			@tmpMO=@tmpMO,
			@n=@n,
			@number=@number,
			@tmpTableName=@tmpTableName
		END	
	END
	ELSE BEGIN
		SET @sql+=' UPDATE '+ @tmpTableNameNew +' SET T'+CAST(@n AS nvarchar)+ ' = '+CAST(@tmpLOT AS nvarchar)+ ' WHERE MO= '''+@tmpMO+''''
		SET @number=@tmpLOT
		EXEC manageUnloadData_split4
		@tmpMO=@tmpMO,
		@n=@n,
		@number=@number,
		@tmpTableName=@tmpTableName
	END
	set @n1=@n3
	
	
	FETCH NEXT FROM cursor1 into @tmpMO,@tmpLOT,@tmpTime	
	END
	CLOSE cursor1; DEALLOCATE cursor1;
	exec(@sql)
	
	SET @tmpTableNameNew=@tmpTableName+'_2'
		set @sql=''
		set @tmpTableNameNew1=@tmpTableName+'_3'
		set @sql+=' IF OBJECT_ID ('''+@tmpTableNameNew1+''') IS NOT NULL '
		set @sql+=' DROP TABLE '+ @tmpTableNameNew1
		set @sql+=' CREATE TABLE '+@tmpTableNameNew1
		set @sql+=' (
			Times				NVARCHAR (20),   
			CPN					NVARCHAR (100),
			QUANTITY			FLOAT,
			LINE				NVARCHAR (20),
			CATEGORY			NVARCHAR (20),
			QPB					INT,
			BOXORPALLETNO		NVARCHAR (50),
			CREATETIME          DATETIME )'
		exec(@sql)
		
		EXEC manageUnloadData_split5
		@tmpTableName=@tmpTableNameNew,
		@tmpTableNameNew=@tmpTableNameNew1
END TRY	

BEGIN CATCH
	exec(@sql)
	CLOSE cursor1; DEALLOCATE cursor1;
	SET @ExecText=ERROR_MESSAGE(); RAISERROR(@ExecText,16,1);
END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[manageUnloadData_split3]    Script Date: 2018/11/9 15:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[manageUnloadData_split3]
@tmpMO nvarchar(50),
@n int,
@number int,
@tmpTableName nvarchar(100)
AS
BEGIN
declare @sql nvarchar(max), @sql1 nvarchar(MAX) ,@tmpTime datetime,@tmpTime1 nvarchar(255),@tmpLOT INT ,@tmpCPN nvarchar(255),@tmpQUANTITY INT,@tmpOUTQUANTITY INT,
@tmpLINE nvarchar(255),@tmpQPB INT,@tmpCATEGORY nvarchar(255),@tmpRate int,@tmpBOXORPALLETNO nvarchar(255),@tmpBoxN INT,@tmpLastN INT,@remarks nvarchar(255),@tmpLast INT
	
	SET @sql1=' DECLARE cursor3 CURSOR FOR SELECT a.MO, a.FIRSTPICKTIME, a.LOT, a.CPN, a.QUANTITY, a.LINE ,b.QPB,b.CATEGORY
FROM ' +@tmpTableName+' a
LEFT JOIN dbo.T_Base_MaterialSpec b ON a.CPN=b.CPN
WHERE MO= '''+@tmpMO+''''
	EXEC(@sql1)
	OPEN cursor3;
	FETCH NEXT FROM cursor3 into @tmpMO,@tmpTime,@tmpLOT,@tmpCPN,@tmpQUANTITY,@tmpLINE,@tmpQPB,@tmpCATEGORY
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @sql=''
		SET @remarks=''
		SET @tmpTime1=CAST(@tmpTime AS nvarchar)
		IF @tmpLOT>0 BEGIN
			SET @tmpRate=@tmpQUANTITY/@tmpLOT
			SET @tmpLast=@tmpQUANTITY%@tmpLOT
			IF @tmpLast<>0 BEGIN
				SET @remarks='QUANTITY和LOT的比例不为整数，原数量为:'+CAST(@tmpQUANTITY AS nvarchar)+'，现暂调整为：'+CAST(@tmpLOT*(@tmpRate+1) AS nvarchar)
				SET @tmpRate=@tmpRate+1
				SET @tmpQUANTITY=@tmpLOT*@tmpRate
			END
		END 
		ELSE BEGIN
			SET @tmpRate=0
			SET @remarks='LOT值为：'+CAST(@tmpLOT AS nvarchar)+'，无法处理'
		END
		SET @tmpOUTQUANTITY=@tmpRate*@number
		SET @tmpBoxN=@tmpOUTQUANTITY/@tmpQPB
		SET @tmpLastN=@tmpOUTQUANTITY%@tmpQPB
		WHILE @tmpBoxN>0 BEGIN
			EXEC GetBoxNo
			@tmpFIRSTPICKTIME=@tmpTime1,
			@tmpBoxNo=@tmpBOXORPALLETNO OUT

			SET @tmpOUTQUANTITY=@tmpQPB

			SET @sql+='INSERT INTO '+ @tmpTableName+'_2 (Times, MO, FIRSTPICKTIME, LOT, CPN, TOTALQUANTITY, OUTQUANTITY, RATE, LINE, CATEGORY, BOXORPALLETNO, CREATETIME)
	VALUES (''T'+cast(@n as nvarchar)
			SET @sql+=''','''+ @tmpMO
			SET @sql+=''','''+CAST(@tmpTime AS nvarchar)
			SET @sql+=''','+ CAST(@tmpLOT as nvarchar)
			SET @sql+=','''+ @tmpCPN
			SET @sql+=''','+ CAST(@tmpQUANTITY as nvarchar)
			SET @sql+=','+ cast(@tmpOUTQUANTITY as nvarchar)
			SET @sql+=','+ CAST(@number AS nvarchar)
			SET @sql+=','''+ @tmpLINE
			SET @sql+=''','''+ @tmpCATEGORY
			SET @sql+=''','''+ @tmpBOXORPALLETNO+''',getdate())'
			SET @tmpBoxN-=1
		END
		IF @tmpLastN<>0 OR (@tmpLastN=0 AND @tmpRate=0)BEGIN
			IF @tmpLastN<>0  BEGIN
				EXEC GetBoxNo
				@tmpFIRSTPICKTIME=@tmpTime1,
				@tmpBoxNo=@tmpBOXORPALLETNO OUT
			END
			ELSE
			BEGIN
				SET @tmpBOXORPALLETNO=''
			END
			SET @tmpOUTQUANTITY=@tmpLastN
			SET @sql+='INSERT INTO '+ @tmpTableName+'_2 (Times, MO, FIRSTPICKTIME, LOT, CPN, TOTALQUANTITY, OUTQUANTITY,RATE, LINE, CATEGORY, BOXORPALLETNO, CREATETIME)
	VALUES (''T'+cast(@n as nvarchar)
			SET @sql+=''','''+ @tmpMO
			SET @sql+=''','''+ CAST(@tmpTime AS nvarchar)
			SET @sql+=''','+ CAST(@tmpLOT as nvarchar)
			SET @sql+=','''+ @tmpCPN
			SET @sql+=''','+ CAST(@tmpQUANTITY as nvarchar)
			SET @sql+=','+ cast(@tmpOUTQUANTITY as nvarchar)
			SET @sql+=','+ CAST(@number AS nvarchar)
			SET @sql+=','''+ @tmpLINE
			SET @sql+=''','''+ @tmpCATEGORY
			SET @sql+=''','''+ @tmpBOXORPALLETNO+''',getdate())'
		END
	EXEC(@sql)
	SET @sql1=''	
	IF ISNULL(@remarks,'')<>'' BEGIN
		SET @sql1+='UPDATE '+@tmpTableName+'_2 SET REMARKS = '''+@remarks+'''WHERE CPN = '''+@tmpCPN+''' AND MO = '''+@tmpMO+''''
		EXEC(@sql1)
	END
	FETCH NEXT FROM cursor3 into @tmpMO,@tmpTime,@tmpLOT,@tmpCPN,@tmpQUANTITY,@tmpLINE,@tmpQPB,@tmpCATEGORY				
	END 
	CLOSE cursor3; DEALLOCATE cursor3;
END

GO
/****** Object:  StoredProcedure [dbo].[manageUnloadData_split4]    Script Date: 2018/11/9 15:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[manageUnloadData_split4]
@tmpMO nvarchar(50),
@n int,
@number int,
@tmpTableName nvarchar(100)
AS
BEGIN
declare @sql nvarchar(max), @sql1 nvarchar(MAX) ,@tmpTime datetime,@tmpTime1 nvarchar(255),@tmpLOT INT ,@tmpCPN nvarchar(255),@tmpQUANTITY INT,@tmpOUTQUANTITY INT,
@tmpLINE nvarchar(255),@tmpQPB INT,@tmpCATEGORY nvarchar(255),@tmpRate int,@tmpBOXORPALLETNO nvarchar(255),@remarks nvarchar(255),@tmpTableNameNew nvarchar(100),@tmpLast INT
	SET @sql1=' DECLARE cursor3 CURSOR FOR SELECT a.MO, a.FIRSTPICKTIME, a.LOT, a.CPN, a.QUANTITY, a.LINE ,b.QPB,b.CATEGORY
FROM ' +@tmpTableName+' a
LEFT JOIN dbo.T_Base_MaterialSpec b ON a.CPN=b.CPN
WHERE MO= '''+@tmpMO+''''
	EXEC(@sql1)
	OPEN cursor3;
	FETCH NEXT FROM cursor3 into @tmpMO,@tmpTime,@tmpLOT,@tmpCPN,@tmpQUANTITY,@tmpLINE,@tmpQPB,@tmpCATEGORY
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @sql=''
		SET @remarks=''
		SET @tmpTime1=CAST(@tmpTime AS nvarchar)
		IF @tmpLOT>0 BEGIN
			SET @tmpRate=@tmpQUANTITY/@tmpLOT
			SET @tmpLast=@tmpQUANTITY%@tmpLOT
			IF @tmpLast<>0 BEGIN
				SET @remarks='QUANTITY和LOT的比例不为整数，原数量为:'+CAST(@tmpQUANTITY AS nvarchar)+'，现暂调整为：'+CAST(@tmpLOT*(@tmpRate+1) AS nvarchar)
				SET @tmpRate=@tmpRate+1
				SET @tmpQUANTITY=@tmpLOT*@tmpRate
			END
		END 
		ELSE BEGIN
			SET @tmpRate=0
			SET @remarks='LOT值为：'+CAST(@tmpLOT AS nvarchar)+'，无法处理'
		END
		SET @tmpOUTQUANTITY=@tmpRate*@number
		SET @tmpBOXORPALLETNO=''
		SET @sql+='INSERT INTO '+ @tmpTableName+'_2 (Times, MO, FIRSTPICKTIME, LOT, CPN, TOTALQUANTITY, OUTQUANTITY,RATE, LINE, CATEGORY, BOXORPALLETNO, CREATETIME)
VALUES (''T'+cast(@n as nvarchar)
		SET @sql+=''','''+ @tmpMO
		SET @sql+=''','''+ CAST(@tmpTime AS nvarchar)
		SET @sql+=''','+ CAST(@tmpLOT as nvarchar)
		SET @sql+=','''+ @tmpCPN
		SET @sql+=''','+ CAST(@tmpQUANTITY as nvarchar)
		SET @sql+=','+ cast(@tmpOUTQUANTITY as nvarchar)
		SET @sql+=','+ CAST(@number AS nvarchar)
		SET @sql+=','''+ @tmpLINE
		SET @sql+=''','''+ @tmpCATEGORY
		SET @sql+=''','''+ @tmpBOXORPALLETNO+''',getdate())'
		EXEC(@sql)
		SET @sql1=''	
		IF ISNULL(@remarks,'')<>'' BEGIN
			SET @sql1+='UPDATE '+@tmpTableName+'_2 SET REMARKS = '''+@remarks+'''WHERE CPN = '''+@tmpCPN+''' AND MO = '''+@tmpMO+''''
			EXEC(@sql1)
		END
	FETCH NEXT FROM cursor3 into @tmpMO,@tmpTime,@tmpLOT,@tmpCPN,@tmpQUANTITY,@tmpLINE,@tmpQPB,@tmpCATEGORY				
	END 
	CLOSE cursor3; DEALLOCATE cursor3;
	
END

GO
/****** Object:  StoredProcedure [dbo].[manageUnloadData_split5]    Script Date: 2018/11/9 15:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[manageUnloadData_split5]
@tmpTableName nvarchar(100),
@tmpTableNameNew nvarchar(100)	
AS
BEGIN
declare @sql nvarchar(max), @sql1 nvarchar(MAX) ,@tmpTime nvarchar(255),@tmpLOT INT ,@tmpCPN nvarchar(255),@tmpQUANTITY INT,@tmpOUTQUANTITY INT,
@tmpLINE nvarchar(255),@tmpQPB INT,@tmpCATEGORY nvarchar(255),@tmpRate int,@tmpBOXORPALLETNO nvarchar(255),@tmpBoxN INT,@tmpLastN INT,@remarks nvarchar(255),@tmpLast INT
	SET @sql1=' DECLARE cursor5 CURSOR FOR SELECT a.* ,b.QPB FROM
(SELECT Times,  CPN ,SUM(OUTQUANTITY) AS TOTALOUTQ,LINE,CATEGORY
FROM '+@tmpTableName+' GROUP BY Times, CPN ,LINE,CATEGORY ) a
LEFT JOIN dbo.T_Base_MaterialSpec b ON a.CPN=b.CPN
ORDER BY CAST(substring(TIMES,2,len(TIMES)) as INT)'
	EXEC(@sql1)
	OPEN cursor5;
	FETCH NEXT FROM cursor5 into @tmpTime,@tmpCPN,@tmpQUANTITY,@tmpLINE,@tmpCATEGORY,@tmpQPB
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @sql=''
		SET @tmpBoxN=@tmpQUANTITY/@tmpQPB
		SET @tmpLastN=@tmpQUANTITY%@tmpQPB
		WHILE @tmpBoxN>0 BEGIN
			EXEC GetBoxNo
			@tmpFIRSTPICKTIME=@tmpTime,
			@tmpBoxNo=@tmpBOXORPALLETNO OUT

			SET @tmpOUTQUANTITY=@tmpQPB

			SET @sql+='INSERT INTO '+ @tmpTableNameNew+'(Times,  CPN, QUANTITY,  LINE, CATEGORY, QPB,BOXORPALLETNO, CREATETIME)
	VALUES ('''+@tmpTime
			SET @sql+=''','''+ @tmpCPN
			SET @sql+=''','+CAST(@tmpOUTQUANTITY AS nvarchar)
			SET @sql+=','''+ @tmpLINE
			SET @sql+=''','''+ @tmpCATEGORY
			SET @sql+=''','+ CAST(@tmpQPB AS nvarchar)
			SET @sql+=','''+ @tmpBOXORPALLETNO+''',getdate())'
			SET @tmpBoxN-=1
		END
		IF @tmpLastN<>0 BEGIN
			EXEC GetBoxNo
			@tmpFIRSTPICKTIME=@tmpTime,
			@tmpBoxNo=@tmpBOXORPALLETNO OUT			
			SET @tmpOUTQUANTITY=@tmpLastN
			SET @sql+='INSERT INTO '+ @tmpTableNameNew+'(Times,  CPN, QUANTITY,  LINE, CATEGORY, QPB,BOXORPALLETNO, CREATETIME)
	VALUES ('''+@tmpTime
			SET @sql+=''','''+ @tmpCPN
			SET @sql+=''','+CAST(@tmpOUTQUANTITY AS nvarchar)
			SET @sql+=','''+ @tmpLINE
			SET @sql+=''','''+ @tmpCATEGORY
			SET @sql+=''','+ CAST(@tmpQPB AS nvarchar)
			SET @sql+=','''+ @tmpBOXORPALLETNO+''',getdate())'
		END
		EXEC(@sql)
		FETCH NEXT FROM cursor5 into @tmpTime,@tmpCPN,@tmpQUANTITY,@tmpLINE,@tmpCATEGORY,@tmpQPB
	END
	CLOSE cursor5; DEALLOCATE cursor5;
END

GO
/****** Object:  StoredProcedure [dbo].[selecttable]    Script Date: 2018/11/9 15:02:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	查询所有表名含“_L_3”的数据
-- =============================================
CREATE PROCEDURE [dbo].[selecttable]

AS
BEGIN
	declare @tmpLine nvarchar(50),@tmpTableName nvarchar(100),@sql nvarchar(max),@sql1 nvarchar(max)
	set @sql=''
		
	DECLARE cursor10 CURSOR FOR 
		SELECT name FROM sysobjects WHERE type ='u' AND name LIKE '%_L_3%';
	OPEN cursor10;
	FETCH NEXT FROM cursor10 into @tmpTableName
	WHILE @@FETCH_STATUS = 0
	BEGIN		
		set @sql+=' select SUBSTRING('''+@tmpTableName+''',21,5) AS LINE1,* from '+@tmpTableName
		
		FETCH NEXT FROM cursor10 into @tmpTableName
	end
	CLOSE cursor10; DEALLOCATE cursor10;
	exec(@sql)
			
END

GO
