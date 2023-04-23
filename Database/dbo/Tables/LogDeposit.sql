CREATE TABLE [dbo].[LogDeposit] (
    [LogId]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [Datetime]        NVARCHAR (50) NULL,
    [AccountNumber]   NVARCHAR (50) NULL,
    [TransactionType] NVARCHAR (50) NULL,
    [Nominal]         FLOAT (53)    NULL,
    [Status]          INT           NULL,
    [StatusUpload]    INT           NULL
);

