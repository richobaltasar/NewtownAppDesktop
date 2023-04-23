CREATE TABLE [dbo].[LogRefundDetail] (
    [IdRefund]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Datetime]           NVARCHAR (50)  NOT NULL,
    [AccountNumber]      NVARCHAR (50)  NOT NULL,
    [SaldoEmoney]        FLOAT (53)     NULL,
    [SaldoJaminan]       FLOAT (53)     NULL,
    [TicketWeekDay]      FLOAT (53)     NULL,
    [TicketWeekEnd]      FLOAT (53)     NULL,
    [TotalNominalRefund] FLOAT (53)     NULL,
    [ChasierBy]          NVARCHAR (MAX) NULL,
    [ComputerName]       NVARCHAR (MAX) NULL,
    [Status]             INT            NOT NULL,
    [StatusUpload]       INT            NULL
);

