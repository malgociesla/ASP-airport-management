CREATE TABLE [dbo].[Schedule] (
    [idSchedule] UNIQUEIDENTIFIER NOT NULL,
    [idFlight]   UNIQUEIDENTIFIER NULL,
    [Fdate]      DATE             NULL,
    PRIMARY KEY CLUSTERED ([idSchedule] ASC),
    FOREIGN KEY ([idFlight]) REFERENCES [dbo].[Flight] ([idFlight])
);

