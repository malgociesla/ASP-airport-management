CREATE PROCEDURE [dbo].[GenerateSchedule]
	@startDate date,
	@endDate date,
	@flightId uniqueidentifier =NULL
AS
	WHILE (@startDate < @endDate)
	BEGIN
	SET @startDate=DATEADD(day,1,@startDate); 
	PRINT @startDate
	END

RETURN 0

	--select DATEADD(day,1,'2006-12-13');

	--SELECT @dateFrom, @dateTo
	--select idFlight from [dbo].[Arrival] a inner join [dbo].[Flight] f on a.idArrival=f.idArrival where arrivalTime>@dateTo;

	--today day no
	--SELECT DATEPART(dw,GETDATE());
	--today day name
	--SELECT DATENAME(dw,GETDATE());

	--EXECUTE
	--execute [dbo].[GenerateSchedule] @startDate='2006-12-13', @endDate='2006-12-28';

