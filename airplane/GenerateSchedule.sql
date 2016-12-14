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
		DECLARE @thisIdFlight uniqueidentifier;
		DECLARE @thisDepartureTime time;
		DECLARE @thisDepartuteDT datetime;
		DECLARE @thisArrivalTime time;
		DECLARE @thisArrivalDT datetime;

		SELECT @thisIdFlight = (select idFlight from [dbo].[Flight] where fDayofWeek=@startDateDoW)
		WHILE (@thisIdFlight) IS NOT NULL
		BEGIN
			SELECT @thisDepartureTime = (select departureTime from [dbo].Departure d inner join [dbo].[Flight] f on d.idDeparture=f.idDeparture where f.idFlight=@thisIdFlight)
			SET @thisDepartuteDT= cast(@startDate as datetime) + cast (@thisDepartureTime as datetime);
			SELECT @thisArrivalTime = (select arrivalTime from [dbo].[Arrival] a inner join [dbo].[Flight] f on a.idArrival=f.idArrival where f.idFlight=@thisIdFlight)
			SET @thisArrivalDT= cast(@startDate as datetime) + cast (@thisArrivalTime as datetime);
			INSERT INTO [dbo].[Schedule] (idSchedule,idFlight,departureDT,arrivalDT) values (NEWID(),@thisIdFlight,@thisDepartureDT,@thisArrivalDT)
			SET @startDate=DATEADD(day,1,@startDate); 
			PRINT @startDate
		END
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

