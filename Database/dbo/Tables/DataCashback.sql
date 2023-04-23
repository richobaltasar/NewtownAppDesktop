CREATE TABLE [dbo].[DataCashback] (
    [IdCashback]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [NamaCashback]    NVARCHAR (MAX) NULL,
    [NominalCashback] FLOAT (53)     NULL,
    [Status]          INT            NULL
);

