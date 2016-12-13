CREATE TABLE [dbo].[Flight] (
    [idFlight]     UNIQUEIDENTIFIER NOT NULL,
    [idCompany]    UNIQUEIDENTIFIER NULL,
    [idFStatus]    UNIQUEIDENTIFIER NULL,
    [idDeparture]  UNIQUEIDENTIFIER NULL,
    [idArrival]    UNIQUEIDENTIFIER NULL,
	[fDayofWeek] [int] NULL,
    [name]         VARCHAR (20)     NULL,
    PRIMARY KEY CLUSTERED ([idFlight] ASC),
    FOREIGN KEY ([idArrival]) REFERENCES [dbo].[Arrival] ([idArrival]),
    FOREIGN KEY ([idCompany]) REFERENCES [dbo].[Company] ([idCompany]),
    FOREIGN KEY ([idDeparture]) REFERENCES [dbo].[Departure] ([idDeparture]),
    FOREIGN KEY ([idFStatus]) REFERENCES [dbo].[FStatus] ([idFStatus])
);

