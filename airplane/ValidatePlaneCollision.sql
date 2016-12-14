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
	DECLARE @landingTime time;
	--get inserted arrival datetime
	SELECT @arrivalDT= (select arrivalDT from inserted)
	--convert arrival datetime to landing time
	SET @landingTime= cast(@arrivalDT as time);
	--check how many planes are landing at the same time
	DECLARE @result int;
	exec @result = [dbo].[GetCountOfLandingPlanes] @landingTime=@landingTime;
	--if there are more than 4 planes landing at the same time
	IF (@result>4)
		BEGIN
		print 'more than 4 planes landing'
		END
		ELSE
		print 'no collision'
		--here just insert/update data

END