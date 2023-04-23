CREATE TABLE [dbo].[DataAccount] (
    [Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [AccountNumber] NVARCHAR (50) NOT NULL,
    [Balanced]      FLOAT (53)    NULL,
    [Ticket]        FLOAT (53)    NULL,
    [UangJaminan]   FLOAT (53)    NULL,
    [CreateDate]    NVARCHAR (50) NULL,
    [ExpiredDate]   NVARCHAR (50) NULL,
    [UpdateDate]    NVARCHAR (50) NULL,
    [RefundDate]    NVARCHAR (50) NULL,
    [Status]        NVARCHAR (50) NULL,
    CONSTRAINT [PK_DataAccount] PRIMARY KEY CLUSTERED ([Id] ASC)
);

