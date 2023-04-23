-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetParamPrinter]
AS
BEGIN
	SET NOCOUNT ON;
	select val2 size from DataParam where NamaParam='PrintSetting' and val1= 'Size'
END








