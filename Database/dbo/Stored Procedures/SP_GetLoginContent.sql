CREATE PROCEDURE [dbo].[SP_GetLoginContent]
AS
BEGIN
	SET NOCOUNT ON;
	declare @RightDesc nvarchar(max)
	declare @RightTitle nvarchar(max)
	declare @Logo nvarchar(max)

	--select @RightTitle= val2 from DataParam where NamaParam = 'LoginContent' and val1 = 'RightTitle'
	--select @RightDesc = val2 from DataParam where NamaParam = 'LoginContent' and val1 = 'RightDesc'
	--select * from DataParam where NamaParam = 'Company Profil'
	select
	val8 NamaCompany,
	val1 Alamat,
	val2 Email,
	val3 NamaWahana,
	val4 NoHp,
	val5 NoTelpon,
	val6 Status,
	val7 ImgLink
	from DataParam
	where NamaParam = 'Company Profil'

	--select*from DataParam where NamaParam = 'Company Profil'
	--select @RightTitle RightTitle, @RightDesc RightDesc,@Logo Logo
END










