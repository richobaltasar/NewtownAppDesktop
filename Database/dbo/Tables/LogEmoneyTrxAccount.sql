CREATE TABLE [dbo].[LogEmoneyTrxAccount] (
    [IdLog]           BIGINT        NULL,
    [AcountNumber]    NVARCHAR (50) NULL,
    [Datetime]        NVARCHAR (50) NULL,
    [SaldoSebelumnya] FLOAT (53)    NULL,
    [Credit]          FLOAT (53)    NULL,
    [Debit]           FLOAT (53)    NULL,
    [SisaSaldo]       FLOAT (53)    NULL,
    [Status]          INT           NULL,
    [StatusUpload]    INT           NULL
);

