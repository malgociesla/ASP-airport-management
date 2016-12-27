CREATE TABLE [dbo].[FlightState] (
    [idFlightState] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [name]      VARCHAR (30)     NULL,
    PRIMARY KEY CLUSTERED ([idFlightState] ASC)
);

