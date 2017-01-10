CREATE TABLE [dbo].[Schedule] (
    [Id]  UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [IdFlight]    UNIQUEIDENTIFIER NOT NULL,
	[IdFlightState]   UNIQUEIDENTIFIER NOT NULL,
    [DepartureDT] DATETIME         NULL,
    [ArrivalDT]   DATETIME         NULL,
    [Comment] VARCHAR(50) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdFlight]) REFERENCES [dbo].[Flight] ([Id]) ON DELETE CASCADE,
	FOREIGN KEY ([IdFlightState]) REFERENCES [dbo].[FlightState] ([Id]) ON DELETE CASCADE
);

