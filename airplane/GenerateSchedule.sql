CREATE PROCEDURE [dbo].[GenerateSchedule]
	@startDate date,
	@endDate date,
	@flightId uniqueidentifier =NULL,
	@startDateDoW  int=0
AS
	WHILE (@startDate < @endDate)
	BEGIN
	
	SET @startDateDoW = DATEPART(dw,@startDate);
	PRINT @startDateDoW;
	--select idFlight from [dbo].[Arrival] a inner join [dbo].[Flight] f on a.idArrival=f.idArrival where arrivalTime>@dateTo;
	SET @startDate=DATEADD(day,1,@startDate); 
	PRINT @startDate
	END


	--select DATEADD(day,1,'2006-12-13');

	--SELECT @dateFrom, @dateTo
	--select idFlight from [dbo].[Arrival] a inner join [dbo].[Flight] f on a.idArrival=f.idArrival where arrivalTime>@dateTo;

	--today day no
	--SELECT DATEPART(dw,GETDATE());
	--today day name
	--SELECT DATENAME(dw,GETDATE());

	--EXECUTE
	--execute [dbo].[GenerateSchedule] @startDate='2006-12-13', @endDate='2006-12-28';

