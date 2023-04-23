CREATE TABLE [dbo].[LogCashierTambahModal] (
    [idLog]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [Datetime]           NVARCHAR (50)  NULL,
    [NamaComputer]       NVARCHAR (MAX) NULL,
    [NamaUser]           NVARCHAR (MAX) NULL,
    [NominalTambahModal] FLOAT (53)     NULL,
    [Status]             INT            NULL,
    [StatusUpload]       INT            NULL
);

