--execute [dbo].[GenerateSchedule] @startDate='2016-12-11', @endDate='2016-12-24';
--execute [dbo].[GenerateSchedule] @startDate='2016-12-11', @endDate='2016-12-24', @flightId='376d7ce7-f1c1-e611-b353-d017c293d790';
CREATE PROCEDURE [dbo].[GenerateSchedule]
	@startDate date,
	@endDate date,
	@flightId uniqueidentifier =NULL
AS
	--walk through date range <startDate,endDate>
	WHILE (@startDate <= @endDate)
	BEGIN
	--get day of the week from date
		DECLARE @startDateDoW int
		SET @startDateDoW = DATEPART(dw,@startDate);
		DECLARE @thisIdFlight uniqueidentifier;
		DECLARE @thisDepartureTime time;
		DECLARE @thisDepartureDT datetime;
		DECLARE @thisArrivalTime time;
		DECLARE @thisArrivalDT datetime;
		--cursor
		DECLARE @flightCursor CURSOR;
		--schedule for flight specified by @flightId
		IF(@flightId) IS NOT NULL
			BEGIN
			SET @flightCursor = CURSOR FOR select idFlight from [dbo].[Flight] f where fDayofWeek=@startDateDoW AND f.idFlight=@flightId;
			END
		ELSE
		--schedule for all flights
			BEGIN
			SET @flightCursor = CURSOR FOR select idFlight from [dbo].[Flight] where fDayofWeek=@startDateDoW
			END
		OPEN @flightCursor
		FETCH NEXT FROM @flightCursor 
		INTO @thisIdFlight

		WHILE @@FETCH_STATUS = 0
		BEGIN
			--get flight data
			SELECT @thisDepartureTime = (select departureTime from [dbo].Departure d inner join [dbo].[Flight] f on d.idDeparture=f.idDeparture where f.idFlight=@thisIdFlight)
			SET @thisDepartureDT= cast(@startDate as datetime) + cast (@thisDepartureTime as datetime);
			SELECT @thisArrivalTime = (select arrivalTime from [dbo].[Arrival] a inner join [dbo].[Flight] f on a.idArrival=f.idArrival where f.idFlight=@thisIdFlight)
			SET @thisArrivalDT= cast(@startDate as datetime) + cast (@thisArrivalTime as datetime);
			--generate schedule
			INSERT INTO [dbo].[Schedule] (idSchedule,idFlight,departureDT,arrivalDT) values (NEWID(),@thisIdFlight,@thisDepartureDT,@thisArrivalDT)
			--cursor
			FETCH NEXT FROM @flightCursor
			INTO @thisIdFlight 
		END; 
		--clean cursor
		CLOSE @flightCursor ;
		DEALLOCATE @flightCursor;
		--increment date range
		SET @startDate=DATEADD(day,1,@startDate);
END;
		 



	--select DATEADD(day,1,'2006-12-13');

	--SELECT @dateFrom, @dateTo
	--select idFlight from [dbo].[Arrival] a inner join [dbo].[Flight] f on a.idArrival=f.idArrival where arrivalTime>@dateTo;

	--today day no
	--SELECT DATEPART(dw,GETDATE());
	--today day name
	--SELECT DATENAME(dw,GETDATE());

	--EXECUTE
	--execute [dbo].[GenerateSchedule] @startDate='2006-12-13', @endDate='2006-12-28';

