--validate plane collision
--max 4 planes landing at the same time
--when collision occures, find free time (iterate by 5 minutes)
CREATE TRIGGER [dbo].[ValidatePlaneCollision]
ON [dbo].[Schedule]
INSTEAD OF INSERT, UPDATE
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @arrivalDT datetime;
	DECLARE @arrivalTime time;
	--get inserted arrival datetime
	SELECT @arrivalDT= (select arrivalDT from inserted)
	--convert arrival datetime to landing time
	SET @arrivalTime= cast(@arrivalDT as time);
	--check how many planes are landing at the same time
	DECLARE @result int;
	exec @result = [dbo].[GetCountOfLandingPlanes] @landingTime=@arrivalTime;
	--if there are more than 4 planes landing at the same time
	IF (@result>4)
		BEGIN
		print 'more than 4 planes landing'
		--select (n-4) planes that must look for free landing time
		--push (n-4) planes to cursor
		DECLARE @providedDT datetime;
		DECLARE @collidedPlanesCursor CURSOR;
		SET @collidedPlanesCursor = CURSOR FOR select @arrivalDT from [dbo].[Schedule] s where s.arrivalDT=s.arrivalDT
		OPEN @collidedPlanesCursor
		FETCH NEXT FROM @collidedPlanesCursor 
		INTO @thisIdFlight
		WHILE @@FETCH_STATUS = 0
			BEGIN
			

			
				SET @timeCounter = @providedTime;
				--for each of them search for free landing time
				SET @timeCounter = DATEADD(minutes,5,@providedDT)
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
		print 'no collision'
		--here just insert/update data
		END
	--for now insert data anyway
	INSERT INTO [dbo].[Schedule] SELECT * FROM inserted;
END