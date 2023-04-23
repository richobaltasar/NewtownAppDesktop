CREATE TABLE [dbo].[DataChasierBox] (
    [IdModal]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Datetime]         NVARCHAR (50)  NULL,
    [NamaComputer]     NVARCHAR (MAX) NULL,
    [DanaModalSebelum] FLOAT (53)     NULL,
    [DanaModalSetelah] FLOAT (53)     NULL,
    [TotalUangDiBox]   FLOAT (53)     NULL,
    [TotalUangMasuk]   FLOAT (53)     NULL,
    [TotalUangKeluar]  FLOAT (53)     NULL,
    [Status]           INT            NULL,
    [OpenBy]           NVARCHAR (MAX) NULL,
    [UpdateBy]         NVARCHAR (MAX) NULL,
    [CloseBy]          NVARCHAR (MAX) NULL
);

