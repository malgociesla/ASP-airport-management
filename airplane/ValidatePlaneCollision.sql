--validate plane collision
--max 4 planes landing at the same time
--when collision occures, find free time (iterate by 5 minutes)

CREATE TRIGGER [dbo].[ValidatePlaneCollision]
ON [dbo].[Schedule]
INSTEAD OF INSERT, UPDATE
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @arrivalDT DATETIME;
	DECLARE @arrivalTime TIME;
	--get inserted arrival datetime
	SELECT @arrivalDT= (SELECT arrivalDT
						FROM inserted)
	--convert arrival datetime to landing time
	SET @arrivalTime= CAST(@arrivalDT AS TIME);
	--check how many planes are landing at the same time
	DECLARE @result INT;
	EXEC @result = [dbo].[GetCountOfLandingPlanes] @landingTime=@arrivalTime;
	--if there are more than 4 planes landing at the same time
	IF (@result>4)
		BEGIN
		PRINT 'more than 4 planes landing'
		--select (n-4) planes that must look for free landing time
		--push (n-4) planes to cursor
		DECLARE @providedDT DATETIME;
		DECLARE @collidedPlanesCursor CURSOR;
		SET @collidedPlanesCursor = CURSOR FOR
									SELECT @arrivalDT
									FROM [dbo].[Schedule] s
									WHERE s.arrivalDT=s.arrivalDT
		OPEN @collidedPlanesCursor
		FETCH NEXT FROM @collidedPlanesCursor 
		INTO @thisIdFlight
		WHILE @@FETCH_STATUS = 0
			BEGIN
			

			
				SET @timeCounter = @providedTime;
				--for each of them search for free landing time
				SET @timeCounter = DATEADD(MINUTES,5,@providedDT)
				--insert data with changed landingTime
				--cursor
				FETCH NEXT FROM @collidedPlanesCursor
				INTO @thisIdFlight 
			END; 
		--clean cursor
		CLOSE @collidedPlanesCursor;
		DEALLOCATE @collidedPlanesCursor;
		END
	ELSE
		BEGIN
		PRINT 'no collision'
		--here just insert/update data
		END
	--for now insert data anyway
	INSERT INTO [dbo].[Schedule]
	SELECT *
	FROM inserted;
END