CREATE TABLE [dbo].[LogStokOpname] (
    [idLog]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Datetime]        NVARCHAR (50)  NULL,
    [NamaTenant]      NVARCHAR (MAX) NULL,
    [NamaItem]        NVARCHAR (MAX) NULL,
    [StockSebelumnya] FLOAT (53)     NULL,
    [StockUpdate]     FLOAT (53)     NULL,
    [UpdateBy]        NVARCHAR (50)  NULL,
    [Status]          INT            NULL,
    [StatusUpload]    INT            NULL
);

