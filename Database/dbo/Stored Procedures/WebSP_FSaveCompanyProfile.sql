CREATE PROCEDURE [dbo].[WebSP_FSaveCompanyProfile]
	@NamaCompany nvarchar(max),
	@Alamat nvarchar(max),
	@Email nvarchar(max),
	@NamaWahana nvarchar(max),
	@NoHp nvarchar(max),
	@NoTelpon nvarchar(max),
	@Status nvarchar(max),
	@ImgLink nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;
BEGIN TRY
	if exists(select*from DataParam where NamaParam = 'Company Profil')
	begin
	    update DataParam set 
			val1=@Alamat,
			val2=@Email,
			val3=@NamaWahana,
			val4=@NoHp,
			val5=@NoTelpon,
			val6=@Status,
			val7=@ImgLink,
			val8=@NamaCompany
			where NamaParam = 'Company Profil'

		select 'Info' as title, 'Data Company profile berhasil disimpan' as message,
		'success' as icon
	end
	else
	begin
		insert into DataParam (NamaParam,val1,val2,val3,val4,val5,val6,val7,val8)
		values
		('Company Profil',@Alamat,@Email,@NamaWahana,@NoHp,@NoTelpon,@Status,@ImgLink,@NamaCompany)
		
		select 'Info' as title, 'Data Company profile berhasil disimpan' as message,
		'success' as icon
	end
END TRY
BEGIN CATCH
	select 'Info' as title, 'Data Company profile gagal disimpan' as message,
		'error' as icon
END CATCH
END








