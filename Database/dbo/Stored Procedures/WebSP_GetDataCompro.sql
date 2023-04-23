
CREATE PROCEDURE [dbo].[WebSP_GetDataCompro]
AS
BEGIN
	SET NOCOUNT ON;
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
END







