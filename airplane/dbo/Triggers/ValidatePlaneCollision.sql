--validate plane collision
--@maxLandingCapacity - change for how many planes can land at the same time
--when collision occures, find free time (iterate by 5 minutes)

CREATE TRIGGER [dbo].[ValidatePlaneCollision]
ON [dbo].[Schedule]
INSTEAD OF INSERT, UPDATE
AS
BEGIN
	SET NOCOUNT ON
	--max 4 planes landing at the same time
	DECLARE @maxLandingCapacity INT = 4
	DECLARE @arrivalDT DATETIME
	--get inserted arrival datetime
	SELECT @arrivalDT= (SELECT arrivalDT
						FROM inserted)

	--check how many planes are landing at the same time
	DECLARE @planeCount INT = @maxLandingCapacity +1	

	--if there are to many planes landing at the same time
	DECLARE @timeCounter DATETIME = @arrivalDT
	WHILE (@planeCount>@maxLandingCapacity)
	BEGIN
		PRINT 'too many planes landing'
		SET @timeCounter = DATEADD(MINUTE,5,@timeCounter)
		EXEC @planeCount = [dbo].[GetCountOfLandingPlanes] @landingDT=@timeCounter
	END
	--insert data, but change inserted.arrivalDT to @timeCounter
	--INSERT INTO  [dbo].[Schedule] (idSchedule,idFlight,departureDT,arrivalDT)
	--inserted.
	--FROM inserted

	INSERT INTO [dbo].[Schedule]
	SELECT *
	FROM inserted
END