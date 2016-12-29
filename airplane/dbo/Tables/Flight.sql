CREATE TABLE [dbo].[Flight] (
    [idFlight]    UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [idCompany]   UNIQUEIDENTIFIER NOT NULL,
    [name]        VARCHAR (20)     NULL,
    [fDayofWeek]  INT              NULL,
    [idCityDeparture] UNIQUEIDENTIFIER NOT NULL, 
    [idCityArrival] UNIQUEIDENTIFIER NOT NULL, 
    [departureTime] TIME NULL, 
    [arrivalTime] TIME NULL, 
    PRIMARY KEY CLUSTERED ([idFlight] ASC),
    FOREIGN KEY ([idCompany]) REFERENCES [dbo].[Company] ([idCompany]) ON DELETE CASCADE,
    FOREIGN KEY ([idCityDeparture]) REFERENCES [dbo].[City] ([idCity]),
    FOREIGN KEY ([idCityArrival]) REFERENCES [dbo].[City] ([idCity])
);

