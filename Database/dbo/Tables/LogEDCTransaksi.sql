CREATE TABLE [dbo].[LogEDCTransaksi] (
    [IdLog]          BIGINT         NOT NULL,
    [Datetime]       NVARCHAR (50)  NULL,
    [TotalBelanja]   FLOAT (53)     NULL,
    [CodeBank]       NVARCHAR (50)  NULL,
    [NamaBank]       NVARCHAR (MAX) NULL,
    [DiskonBank]     FLOAT (53)     NULL,
    [NominalDiskon]  FLOAT (53)     NULL,
    [AdminCharges]   FLOAT (53)     NULL,
    [TotalDebit]     FLOAT (53)     NULL,
    [NoATM]          NVARCHAR (MAX) NULL,
    [NoReffEddPrint] NVARCHAR (MAX) NULL,
    [ComputerName]   NVARCHAR (MAX) NULL,
    [CashierBy]      NVARCHAR (MAX) NULL,
    [status]         INT            NULL,
    [StatusUpload]   INT            NULL
);

