﻿CREATE PROCEDURE [dbo].[WebReportSP_TopupTransaksiDetailFooter]
	@SetAwal nvarchar(max),
	@SetAkhir nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

    select
	sum(NominalTopup) NominalTopup,
	sum(TotalBayar) TotalBayar,
	sum(PayCash) PayCash,	
	sum(TerimaUang) TerimaUang,
	sum(Kembalian) Kembalian,
	sum(SaldoSebelum) SaldoSebelum,
	sum(SaldoSetelah) SaldoSetelah
	from LogTopupDetail
	where 
	--left(Datetime,10) between @SetAwal and @SetAkhir
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(left(Datetime,10),'/','-'), 105), 23),'-','')
	between 
	replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAwal,'/','-'), 105), 23),'-','') 
	and replace(CONVERT(VARCHAR(10), CONVERT(date, replace(@SetAkhir,'/','-'), 105), 23),'-','')


	--and status = 1

END













