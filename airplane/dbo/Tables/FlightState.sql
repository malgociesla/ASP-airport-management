CREATE TABLE [dbo].[FlightState] (
    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Name]      VARCHAR (30)     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

