CREATE TABLE [dbo].[Schedule] (
    [idSchedule]  UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [idFlight]    UNIQUEIDENTIFIER NOT NULL,
    [departureDT] DATETIME         NULL,
    [arrivalDT]   DATETIME         NULL,
    [comment] VARCHAR(50) NULL, 
    PRIMARY KEY CLUSTERED ([idSchedule] ASC),
    FOREIGN KEY ([idFlight]) REFERENCES [dbo].[Flight] ([idFlight]) ON DELETE CASCADE
);

