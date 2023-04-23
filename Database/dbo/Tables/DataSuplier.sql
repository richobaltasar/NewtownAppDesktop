CREATE TABLE [dbo].[DataSuplier] (
    [idSuplier]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [NamaSuplier] NVARCHAR (MAX) NULL,
    [Alamat]      NVARCHAR (MAX) NULL,
    [Email]       NVARCHAR (50)  NULL,
    [NoHp]        NVARCHAR (50)  NULL,
    [NoTelpon]    NVARCHAR (50)  NULL,
    [NameSales]   NVARCHAR (MAX) NULL,
    [PIC]         NVARCHAR (MAX) NULL,
    [status]      NVARCHAR (50)  NULL,
    [Keterangan]  NVARCHAR (MAX) NULL
);

