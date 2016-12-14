CREATE TABLE [dbo].[FStatus] (
    [idFStatus] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [name]      VARCHAR (30)     NULL,
    PRIMARY KEY CLUSTERED ([idFStatus] ASC)
);

