CREATE TABLE [dbo].[Country] (
    [idCountry] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [name]      VARCHAR (50)     NULL,
    PRIMARY KEY CLUSTERED ([idCountry] ASC)
);

