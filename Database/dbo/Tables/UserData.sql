CREATE TABLE [dbo].[UserData] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [username]    NVARCHAR (MAX) NULL,
    [password]    NVARCHAR (MAX) NULL,
    [hakakses]    NVARCHAR (MAX) NULL,
    [NamaLengkap] NVARCHAR (MAX) NULL,
    [Email]       NVARCHAR (MAX) NULL,
    [Gender]      NVARCHAR (50)  NULL,
    [NoHp]        NVARCHAR (50)  NULL,
    [ImgLink]     NVARCHAR (MAX) NULL,
    [Alamat]      NVARCHAR (MAX) NULL,
    [Status]      NVARCHAR (50)  NULL
);

