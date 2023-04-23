CREATE TABLE [dbo].[LogFoodcourtTransaksi] (
    [IdTrx]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [Datetime]         NVARCHAR (50)  NULL,
    [AccountNumber]    NVARCHAR (50)  NULL,
    [SaldoEmoney]      FLOAT (53)     NULL,
    [SaldoEmoneyAfter] FLOAT (53)     NULL,
    [IdItemsKeranjang] BIGINT         NULL,
    [JenisTransaksi]   NVARCHAR (MAX) NULL,
    [TotalBayar]       FLOAT (53)     NULL,
    [PayEmoney]        FLOAT (53)     NULL,
    [PayCash]          FLOAT (53)     NULL,
    [TerimaUang]       FLOAT (53)     NULL,
    [Kembalian]        FLOAT (53)     NULL,
    [ComputerName]     NVARCHAR (MAX) NULL,
    [CashierBy]        NVARCHAR (MAX) NULL,
    [Status]           INT            NULL,
    [StatusUpload]     INT            NULL
);

