CREATE TABLE [dbo].[LogSetoranDepositExpired] (
    [LogId]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Datetime]       NVARCHAR (50)  NULL,
    [AccountNumber]  NVARCHAR (MAX) NULL,
    [Saldo]          FLOAT (53)     NULL,
    [UangJaminan]    FLOAT (53)     NULL,
    [TotalDeposit]   FLOAT (53)     NULL,
    [TanggalExpired] NVARCHAR (50)  NULL,
    [NamaPenyetor]   NVARCHAR (MAX) NULL,
    [TanggalSetor]   NVARCHAR (50)  NULL,
    [StatusSetor]    INT            NULL,
    [StatusUpload]   INT            NULL
);

