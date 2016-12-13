CREATE TABLE [dbo].[City] (
    [idCity]    UNIQUEIDENTIFIER NOT NULL,
    [idCountry] UNIQUEIDENTIFIER NULL,
    [name]      VARCHAR (50)     NULL,
    PRIMARY KEY CLUSTERED ([idCity] ASC),
    FOREIGN KEY ([idCountry]) REFERENCES [dbo].[Country] ([idCountry])
);

