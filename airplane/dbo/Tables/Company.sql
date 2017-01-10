CREATE TABLE [dbo].[Company] (
    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Name]      VARCHAR (100)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

