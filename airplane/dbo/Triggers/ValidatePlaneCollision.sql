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
	SELECT @arrivalDT= (SELECT arrivalDT
						FROM inserted)

	DECLARE @timeCounter DATETIME = @arrivalDT
	--check how many planes are landing at the same time
	DECLARE @planeCount INT	
	EXEC @planeCount = [dbo].[GetCountOfLandingPlanes] @landingDT=@timeCounter

	--check if plane was added before to the schedule
	DECLARE @exist INT;
	SELECT @exist = (SELECT count(s.idFlight)
					FROM inserted i
						INNER JOIN Schedule s
						ON s.idFlight=i.idFlight
					WHERE s.idFlight=i.idFlight AND s.arrivalDT=i.arrivalDT)

	IF(@exist<>0) PRINT 'this flight has already been inserted to schedule'
	ELSE
	BEGIN
	--if there are to many planes landing at the same time
	WHILE (@planeCount>@maxLandingCapacity)
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
		INSERT INTO [dbo].[Schedule]
		SELECT i.idSchedule,i.idFlight,i.departureDT,@timeCounter
		FROM inserted AS i
	END
END