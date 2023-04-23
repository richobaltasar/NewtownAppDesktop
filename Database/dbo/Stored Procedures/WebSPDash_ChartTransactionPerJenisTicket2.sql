-- Batch submitted through debugger: dbewats.sql|3444|0|C:\Users\Administrator\Desktop\dbewats.sql
-- exec WebSPDash_ChartTransactionPerJenisTicket '10/11/2018','16/11/2018'
CREATE PROCEDURE [dbo].[WebSPDash_ChartTransactionPerJenisTicket2]
	@setAwal nvarchar(50),
	@setAkhir nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	select left(right(Datetime,8),2)+':00' Datetime,NamaTicket JenisTicket,sum(TotalAfterDiskon) Total
		into #Main
		from LogTicketDetail 
		where 
		--left(Datetime,10) between @setAwal and @setAkhir 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
		between 
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setAkhir,'/','-'), 105), 23),'-','')
		group by NamaTicket,Datetime

	if exists(select*from #Main)
	begin
		select distinct Datetime into #Row from #Main
		select distinct JenisTicket into #Col from #Main
		declare @Qry nvarchar(max)
		set @Qry = 'CREATE table Temp( datetime nvarchar(max), '
		while (select COUNT(*) from #Col) > 0
		begin
			declare @KolomName nvarchar(max)
			set @KolomName = (select Top 1 JenisTicket from #Col)
			
			if(select COUNT(*) from #Col) >1
			begin
				set @Qry =  @Qry +'['+@KolomName+'] float,'	
			end
			else
			begin
				set @Qry =  @Qry +'['+@KolomName+'] float'	
			end
			delete from #Col where JenisTicket = @KolomName
		end
		set @Qry =  @Qry +')'
		print @Qry
		EXECUTE sp_executesql @Qry

		while (select count(*) from #Row) > 0
		begin
			declare @Row nvarchar(max)
			set @Row = (select top 1* from #Row)
			while (select count(*) from #Main where Datetime = @Row) > 0
			begin	
				declare @jenisTicket nvarchar(max)
				declare @Total float
				declare @Qry2 nvarchar(max)
				set @jenisTicket = (select top 1 JenisTicket from #Main where Datetime = @Row )
				set @Total = (select top 1 Total from #Main where Datetime = @Row and JenisTicket = @jenisTicket)
				set @Qry2 = 'insert into Temp '
				set @Qry2 = @Qry2 + '(datetime,['+@jenisTicket+']) values('''+@Row+''','+Cast(@Total as nvarchar(50))+')'
				print @Qry2
				EXECUTE sp_executesql @Qry2
				delete from #Main where Datetime = @Row and JenisTicket = @jenisTicket and Total = @Total		
			end
			delete from #Row where Datetime = @Row
		end
		--select*from Temp
		SELECT COLUMN_NAME into #ColName
		FROM ewats.INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = N'Temp' and COLUMN_NAME != 'datetime'

		declare @Qry3 nvarchar(max)
		set @Qry3 = 'select datetime,'
		while (select count(*) from #ColName ) > 0
		begin
			declare @ccName nvarchar(max)
			set @ccName = (select top 1 COLUMN_NAME from #ColName)
			if(select count(*) from #ColName) > 1
			begin
				set @Qry3 = @Qry3 + 'sum(isnull(['+@ccName+'],0)) ['+@ccName+'],'	
			end
			else
			begin
				set @Qry3 = @Qry3 + 'sum(isnull(['+@ccName+'],0)) ['+@ccName+']'	
			end
			delete from #ColName where COLUMN_NAME = @ccName
		end
		set @Qry3 = @Qry3 +' from Temp group by datetime'
		print @Qry3
		EXECUTE sp_executesql @Qry3
		drop table Temp
	end
END













