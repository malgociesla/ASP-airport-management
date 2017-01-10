CREATE TABLE [dbo].[City] (
    [Id]    UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [IdCountry] UNIQUEIDENTIFIER NOT NULL,
    [Name]      VARCHAR (50)     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdCountry]) REFERENCES [dbo].[Country] ([Id]) ON DELETE CASCADE
);

