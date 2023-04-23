CREATE PROCEDURE [dbo].[SP_RekapanTicketPeriodik]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;	

	select
		NamaTicket,Harga,Qty,NamaDiskon,((Harga*Qty)*(Diskon/100)) TotalDiskon,
			((Harga*Qty)-((Harga*Qty)*(Diskon/100))) TotalAfterDiskon,JenisTransaksi
	from
	(
		select a.NamaTicket,a.Harga,sum(a.Qty)Qty, 
		a.NamaDiskon,b.JenisTransaksi,Diskon 

		from [dbo].[LogTicketDetail] a
		left join LogRegistrasiDetail b on b.IdTicketTrx = a.IdTicket
		where 
		--left(a.Datetime,10) = @setTanggal
		replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(a.Datetime,10),'/','-'), 105), 23),'-','') 
		between replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
		and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','') 
		group by a.NamaTicket,a.Harga,a.NamaDiskon,b.JenisTransaksi,Diskon
	) q
	order by q.JenisTransaksi
END













