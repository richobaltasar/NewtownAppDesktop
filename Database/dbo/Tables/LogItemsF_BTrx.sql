CREATE TABLE [dbo].[LogItemsF&BTrx] (
    [IdItemsKeranjang] BIGINT         NULL,
    [Datetime]         NVARCHAR (50)  NULL,
    [NamaTenant]       NVARCHAR (MAX) NULL,
    [KodeBarang]       BIGINT         NULL,
    [NamaItem]         NVARCHAR (MAX) NULL,
    [Harga]            FLOAT (53)     NULL,
    [Qtx]              FLOAT (53)     NULL,
    [Total]            FLOAT (53)     NULL,
    [Status]           INT            NULL,
    [Chasierby]        NVARCHAR (MAX) NULL,
    [ComputerName]     NVARCHAR (MAX) NULL,
    [AccountNumber]    NVARCHAR (50)  NULL,
    [StatusUpload]     INT            NULL
);

