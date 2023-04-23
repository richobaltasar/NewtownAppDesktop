CREATE TABLE [dbo].[DataTenant] (
    [idTenant]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [NamaTenant]        NVARCHAR (MAX) NULL,
    [PemilikTenant]     NVARCHAR (MAX) NULL,
    [OpenDateTenant]    NVARCHAR (MAX) NULL,
    [StatusKepemilikan] NVARCHAR (MAX) NULL,
    [StatusJual]        NVARCHAR (MAX) NULL,
    [FollowTenant]      NVARCHAR (MAX) NULL,
    [MonitoringStock]   INT            NULL,
    [Status]            INT            NULL
);

