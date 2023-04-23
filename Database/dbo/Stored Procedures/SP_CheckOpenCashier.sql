-- Batch submitted through debugger: dbewats.sql|222|0|C:\Users\Administrator\Desktop\dbewats.sql

CREATE PROCEDURE [dbo].[SP_CheckOpenCashier]
	@ComputerName nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
	if exists(select*from [dbo].[DataChasierBox] where 
	NamaComputer = @ComputerName and 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','') = FORMAT(GETDATE() , 'yyyyMMdd')
	and status =1)
	begin 
		select 'TRUE' as Success, 'Status Merchant Open' as _Message
	end
	else
	begin
		select 'FALSE' as Success, 'Silahkan memasukan Dana Modal' as _Message
	end

END











