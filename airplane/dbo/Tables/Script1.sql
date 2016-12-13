ALTER PROCEDURE [dbo].[GenerateSchedule]
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

RETURN 0