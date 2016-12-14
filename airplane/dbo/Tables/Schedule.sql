CREATE TABLE [dbo].[Schedule] (
    [idSchedule]  UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [idFlight]    UNIQUEIDENTIFIER NULL,
    [departureDT] DATETIME         NULL,
    [arrivalDT]   DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([idSchedule] ASC),
    FOREIGN KEY ([idFlight]) REFERENCES [dbo].[Flight] ([idFlight])
);

