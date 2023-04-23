CREATE TABLE [dbo].[DataTicket] (
    [IdTicket]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [namaticket] NVARCHAR (MAX) NULL,
    [Monday]     FLOAT (53)     NULL,
    [Tuesday]    FLOAT (53)     NULL,
    [Wednesday]  FLOAT (53)     NULL,
    [Thursday]   FLOAT (53)     NULL,
    [Friday]     FLOAT (53)     NULL,
    [Saturday]   FLOAT (53)     NULL,
    [Sunday]     FLOAT (53)     NULL,
    [status]     NVARCHAR (50)  NULL
);

