﻿CREATE PROCEDURE [dbo].[SP_GenCodeID]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT FORMAT(GETDATE() , 'yyyyMMddHHmmss') as tanggal
END





