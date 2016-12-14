CREATE TABLE [dbo].[Flight] (
    [idFlight]    UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [idCompany]   UNIQUEIDENTIFIER NULL,
    [idFStatus]   UNIQUEIDENTIFIER NULL,
    [idDeparture] UNIQUEIDENTIFIER NULL,
    [idArrival]   UNIQUEIDENTIFIER NULL,
    [name]        VARCHAR (20)     NULL,
    [fDayofWeek]  INT              NULL,
    PRIMARY KEY CLUSTERED ([idFlight] ASC),
    FOREIGN KEY ([idCompany]) REFERENCES [dbo].[Company] ([idCompany]),
    FOREIGN KEY ([idFStatus]) REFERENCES [dbo].[FStatus] ([idFStatus]),
    FOREIGN KEY ([idDeparture]) REFERENCES [dbo].[Departure] ([idDeparture]),
    FOREIGN KEY ([idArrival]) REFERENCES [dbo].[Arrival] ([idArrival])
);

