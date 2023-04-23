-- Batch submitted through debugger: dbewats.sql|207|0|C:\Users\Administrator\Desktop\dbewats.sql
CREATE PROCEDURE [dbo].[SP_CheckNowWeekend]
AS
BEGIN
	SET NOCOUNT ON;
	select datename(dw,getdate()) as HariNow
END











