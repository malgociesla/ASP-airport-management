﻿CREATE TABLE [dbo].[Arrival] (
    [idArrival]   UNIQUEIDENTIFIER NOT NULL,
    [idCity]      UNIQUEIDENTIFIER NULL,
    [arrivalTime] TIME (7)         NULL,
    PRIMARY KEY CLUSTERED ([idArrival] ASC),
    FOREIGN KEY ([idCity]) REFERENCES [dbo].[City] ([idCity])
);

