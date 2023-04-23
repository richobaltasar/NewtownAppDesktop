CREATE TABLE [dbo].[DataBank] (
    [idLog]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [KodeBank]     NVARCHAR (50)  NULL,
    [NamaBank]     NVARCHAR (MAX) NULL,
    [DiskonBank]   FLOAT (53)     NULL,
    [AdminCharges] FLOAT (53)     NULL,
    [status]       NVARCHAR (50)  NULL
);

