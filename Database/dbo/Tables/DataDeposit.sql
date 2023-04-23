CREATE TABLE [dbo].[DataDeposit] (
    [LogID]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [Datetime]              NVARCHAR (50) NULL,
    [Deposit]               FLOAT (53)    NULL,
    [DepositHariSebelumnya] FLOAT (53)    NULL,
    [Credit]                FLOAT (53)    NULL,
    [Debit]                 FLOAT (53)    NULL,
    [Status]                INT           NULL
);

