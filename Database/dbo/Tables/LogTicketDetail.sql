CREATE TABLE [dbo].[LogTicketDetail] (
    [Datetime]         NVARCHAR (50)  NULL,
    [IdTicket]         BIGINT         NULL,
    [AccountNumber]    NVARCHAR (MAX) NULL,
    [NamaTicket]       NVARCHAR (MAX) NULL,
    [Harga]            FLOAT (53)     NULL,
    [Qty]              FLOAT (53)     NULL,
    [Total]            FLOAT (53)     NULL,
    [IdDiskon]         BIGINT         NULL,
    [NamaDiskon]       NVARCHAR (MAX) NULL,
    [Diskon]           FLOAT (53)     NULL,
    [TotalDiskon]      FLOAT (53)     NULL,
    [TotalAfterDiskon] FLOAT (53)     NULL,
    [Status]           INT            NULL,
    [ChasierBy]        NVARCHAR (MAX) NULL,
    [ComputerName]     NVARCHAR (MAX) NULL,
    [StatusUpload]     INT            NULL
);

