CREATE TABLE [dbo].[City] (
    [idCity]    UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [idCountry] UNIQUEIDENTIFIER NOT NULL,
    [name]      VARCHAR (50)     NULL,
    PRIMARY KEY CLUSTERED ([idCity] ASC),
    FOREIGN KEY ([idCountry]) REFERENCES [dbo].[Country] ([idCountry]) ON DELETE CASCADE
);

