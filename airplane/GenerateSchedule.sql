﻿--execute [dbo].[GenerateSchedule] @startDate='2016-12-11', @endDate='2016-12-24';
--execute [dbo].[GenerateSchedule] @startDate='2016-12-11', @endDate='2016-12-24', @flightId='376d7ce7-f1c1-e611-b353-d017c293d790';

CREATE PROCEDURE [dbo].[GenerateSchedule]
	@startDate DATE,
	@endDate DATE,
	@flightId UNIQUEIDENTIFIER = NULL
AS
	--walk through date range <startDate,endDate>
	DECLARE @fromDate DATE = @startDate;
	
	WHILE (@fromDate <= @endDate)
	BEGIN
	--get day of the week from date
		DECLARE @startDateDoW INT = DATEPART(dw, @startDate);
		DECLARE @thisIdFlight UNIQUEIDENTIFIER;
		DECLARE @thisDepartureTime TIME;
		DECLARE @thisDepartureDT DATETIME;
		DECLARE @thisArrivalTime TIME;
		DECLARE @thisArrivalDT DATETIME;
		--cursor
		DECLARE @flightCursor CURSOR;
		
		--schedule for flight specified by @flightId
		BEGIN TRY
		IF(@flightId) IS NOT NULL
		BEGIN
			SET @flightCursor = CURSOR FOR
								SELECT idFlight
								FROM [dbo].[Flight] f
								WHERE fDayofWeek=@startDateDoW
								AND f.idFlight=@flightId;
		END
		ELSE
		--schedule for all flights
		BEGIN
			--BEGIN TRY
			SET @flightCursor = CURSOR FOR
								SELECT idFlight
								FROM [dbo].[Flight]
								WHERE fDayofWeek=@startDateDoW
			END
		OPEN @flightCursor
		FETCH NEXT FROM @flightCursor 
		INTO @thisIdFlight

		WHILE @@FETCH_STATUS = 0
		BEGIN
			--get flight data
			SELECT @thisDepartureTime = (SELECT departureTime
											FROM [dbo].Departure d
												INNER JOIN [dbo].[Flight] f 
												ON d.idDeparture=f.idDeparture
											WHERE f.idFlight=@thisIdFlight)
			SET @thisDepartureDT= CAST(@startDate AS DATETIME) + CAST(@thisDepartureTime AS DATETIME);
			SELECT @thisArrivalTime = (SELECT arrivalTime
										FROM [dbo].[Arrival] a
											INNER JOIN [dbo].[Flight] f
											ON a.idArrival=f.idArrival
										WHERE f.idFlight=@thisIdFlight)
			SET @thisArrivalDT= CAST(@startDate AS DATETIME) + CAST(@thisArrivalTime AS DATETIME);
			--generate schedule
			INSERT INTO [dbo].[Schedule] (idSchedule,idFlight,departureDT,arrivalDT)
			VALUES (NEWID(),@thisIdFlight,@thisDepartureDT,@thisArrivalDT)
			--cursor
			FETCH NEXT FROM @flightCursor
			INTO @thisIdFlight 
		END; 
		--clean cursor
		END TRY
		BEGIN CATCH
			CLOSE @flightCursor ;
			DEALLOCATE @flightCursor;
		END CATCH
		--increment date range
		SET @fromDate=DATEADD(day,1,@fromDate);
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

