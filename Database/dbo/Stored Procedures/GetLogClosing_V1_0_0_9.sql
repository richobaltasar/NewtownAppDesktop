-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetLogClosing_V1_0_0_9]
	@LogId bigint
AS
BEGIN
	
	SET NOCOUNT ON;

    select*from LogClosingV2 where LogId = @LogId
END
