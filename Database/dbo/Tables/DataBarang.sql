CREATE TABLE [dbo].[DataBarang] (
    [idMenu]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [IdTenant]       BIGINT         NOT NULL,
    [NamaMenu]       NVARCHAR (MAX) NULL,
    [Category]       NVARCHAR (MAX) NULL,
    [Harga]          FLOAT (53)     NULL,
    [HargaJual]      FLOAT (53)     NULL,
    [HargaKaryawan]  FLOAT (53)     NULL,
    [Stok]           FLOAT (53)     NULL,
    [ImgLink]        NVARCHAR (MAX) NULL,
    [DatetimeUpdate] NVARCHAR (20)  NULL,
    [DatetimeCreate] NVARCHAR (20)  NULL,
    [CreateBy]       NVARCHAR (MAX) NULL,
    [Status]         NVARCHAR (50)  NULL
);

