CREATE TABLE [dbo].[Flight] (
    [Id]    UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [IdCompany]   UNIQUEIDENTIFIER NOT NULL,
    [Name]        VARCHAR (20)     NULL,
    [FDayofWeek]  INT              NULL,
    [IdCityDeparture] UNIQUEIDENTIFIER NOT NULL, 
    [IdCityArrival] UNIQUEIDENTIFIER NOT NULL, 
    [DepartureTime] TIME NULL, 
    [ArrivalTime] TIME NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdCompany]) REFERENCES [dbo].[Company] ([Id]) ON DELETE CASCADE,
    FOREIGN KEY ([IdCityDeparture]) REFERENCES [dbo].[City] ([Id]),
    FOREIGN KEY ([IdCityArrival]) REFERENCES [dbo].[City] ([Id])
);

