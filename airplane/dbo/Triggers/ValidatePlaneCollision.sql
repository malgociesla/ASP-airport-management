CREATE TRIGGER [dbo].[ValidatePlaneCollision]
ON [dbo].[Schedule]
INSTEAD OF INSERT, UPDATE
AS
BEGIN
	SET NOCOUNT ON
	--max 4 planes landing at the same time
	DECLARE @maxLandingCapacity INT = 1
	DECLARE @arrivalDT DATETIME
	--get inserted arrival datetime
	SELECT @arrivalDT= (SELECT arrivalDT
						FROM inserted)

	DECLARE @timeCounter DATETIME = @arrivalDT
	--check how many planes are landing at the same time
	DECLARE @planeCount INT	
	EXEC @planeCount = [dbo].[GetCountOfLandingPlanes] @landingDT=@timeCounter

	--if there are to many planes landing at the same time
	IF (@planeCount>@maxLandingCapacity)
	BEGIN
		SET @timeCounter = DATEADD(MINUTE,5,@timeCounter)
		--insert data
		--if we changed arrival time due to collision -> insert @timeCounter insted of arrivalDT
		INSERT INTO [dbo].[Schedule]
		SELECT i.idSchedule,i.idFlight,i.departureDT,@timeCounter
		FROM inserted AS i
	END
	--if planes can land
	ELSE
	BEGIN
		INSERT INTO [dbo].[Schedule]
		SELECT *
		FROM inserted
	END
END