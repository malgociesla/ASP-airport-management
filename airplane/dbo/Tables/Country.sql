CREATE TABLE [dbo].[Country] (
    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Name]      VARCHAR (50)     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

