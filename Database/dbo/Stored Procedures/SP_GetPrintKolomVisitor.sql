
CREATE PROCEDURE [dbo].[SP_GetPrintKolomVisitor]
AS
BEGIN
	SET NOCOUNT ON;
	select
	val1 Visible,val3 Title,val4 Nama,val5 MoKtp,val6 Alamat,val7 NoTelp
	from DataParam 
	where NamaParam = 'FormatKolomStrukVisitor'
END

