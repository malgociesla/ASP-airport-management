CREATE PROCEDURE [dbo].[GenerateSchedule]
	@dateFrom date,
	@dateTo date,
	@flightId uniqueidentifier =NULL
AS
	--SELECT @dateFrom, @dateTo
	select idFlight from [dbo].[Arrival] a inner join [dbo].[Flight] f on a.idArrival=f.idArrival where arrivalTime>@dateTo;
RETURN 0
