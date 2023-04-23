CREATE TABLE [dbo].[DataPromo] (
    [idPromo]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [NamaPromo]     NVARCHAR (MAX) NULL,
    [CategoryPromo] NVARCHAR (MAX) NULL,
    [Diskon]        FLOAT (53)     NULL,
    [Status]        NVARCHAR (50)  NULL,
    [BerlakuDari]   NVARCHAR (50)  NULL,
    [BerlakuSampai] NVARCHAR (50)  NULL,
    [ImgLink]       NVARCHAR (MAX) NULL
);

