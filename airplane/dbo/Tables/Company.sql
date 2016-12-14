CREATE TABLE [dbo].[Company] (
    [idCompany] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [name]      VARCHAR (100)    NULL,
    PRIMARY KEY CLUSTERED ([idCompany] ASC)
);

