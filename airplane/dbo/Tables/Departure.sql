CREATE TABLE [dbo].[Departure] (
    [idDeparture]   UNIQUEIDENTIFIER NOT NULL,
    [idCity]        UNIQUEIDENTIFIER NULL,
    [departureTime] TIME (7)         NULL,
    PRIMARY KEY CLUSTERED ([idDeparture] ASC),
    FOREIGN KEY ([idCity]) REFERENCES [dbo].[City] ([idCity])
);

