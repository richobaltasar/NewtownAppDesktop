
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--exec SP_RekapanTicket '14/03/2020'
CREATE PROCEDURE [dbo].[SP_RekapanTicket]
	@setTanggal nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;	

	
	select
		NamaTicket,Harga,Qty,NamaDiskon,((Harga*Qty)*(Diskon/100)) TotalDiskon,JenisTransaksi,	
		((Harga*Qty)-((Harga*Qty)*(Diskon/100))) TotalAfterDiskon
	from
	(
	select a.NamaTicket,a.Harga,sum(a.Qty)Qty, 
	a.NamaDiskon,a.Diskon,
		b.JenisTransaksi 
	from [dbo].[LogTicketDetail] a
	left join LogRegistrasiDetail b on b.IdTicketTrx = a.IdTicket and b.AccountNumber = a.AccountNumber
	where 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(a.Datetime,10),'/','-'), 105), 23),'-','') = replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@setTanggal,'/','-'), 105), 23),'-','') 
	group by a.NamaTicket,a.Harga,a.NamaDiskon,b.JenisTransaksi,a.Diskon
	) q
	order by q.JenisTransaksi
END


