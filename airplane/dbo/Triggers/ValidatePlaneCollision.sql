CREATE TRIGGER [dbo].[ValidatePlaneCollision]
ON [dbo].[Schedule]
INSTEAD OF INSERT, UPDATE
AS
BEGIN
	PRINT 'trigger working'
	SET NOCOUNT ON
	--max 4 planes landing at the same time
	DECLARE @maxLandingCapacity INT = 4
	DECLARE @arrivalDT DATETIME
	--get inserted arrival datetime
	SELECT @arrivalDT= (SELECT [ArrivalDT]
						FROM inserted)

	DECLARE @timeCounter DATETIME = @arrivalDT
	--check how many planes are landing at the same time
	DECLARE @planeCount INT	
	EXEC @planeCount = [dbo].[GetCountOfLandingPlanes] @landingDT=@timeCounter
	BEGIN
	--if there are to many planes landing at the same time
	WHILE (@planeCount>=@maxLandingCapacity)
	BEGIN
	PRINT 'too many'
	PRINT @timeCounter
		SET @timeCounter = DATEADD(MINUTE,5,@timeCounter)
		EXEC @planeCount = [dbo].[GetCountOfLandingPlanes] @landingDT=@timeCounter
	PRINT 'incremented by 5min'
	PRINT @timeCounter
	END
	--if planes can land
		--insert data
		--if we changed arrival time due to collision -> insert @timeCounter insted of arrivalDT

		--if flight wasn't inserted before
		IF(NOT EXISTS
		(SELECT s.[Id]
		FROM Schedule s, inserted
		WHERE s.[Id] = inserted.[Id]))
		
		BEGIN
			IF(NOT EXISTS 	(SELECT s.[IdFlight]
					FROM inserted i
						INNER JOIN Schedule s
						ON s.[IdFlight]=i.[IdFlight]
					WHERE s.[IdFlight]=i.[IdFlight] AND s.[ArrivalDT]=i.[ArrivalDT]))
			BEGIN
				PRINT 'trigger on insert'
				INSERT INTO [dbo].[Schedule]
				SELECT i.[Id],i.[IdFlight],i.[IdFlightState],i.[DepartureDT],@timeCounter,i.[Comment]
				FROM inserted AS i
			END
			ELSE PRINT 'schedule for that flight has already been generated'
		END
		ELSE
		--update
		BEGIN
			PRINT 'trigger on update'
			UPDATE Schedule
			SET [IdFlight] = i.[IdFlight],
				[IdFlightState] = i.[IdFlightState],
				[DepartureDT] = i.[DepartureDT],
				[ArrivalDT] = @timeCounter,
				[Comment] = i.[Comment]
			FROM Schedule s, inserted i
			WHERE s.[Id] = i.[Id]
		END
	END
END