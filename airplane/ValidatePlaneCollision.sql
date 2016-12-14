--validate plane collision
--max 4 planes landing at the same time
--when collision occures, find free time (iterate by 5 minutes)
CREATE TRIGGER [dbo].[ValidatePlaneCollision]
ON Schedule
INSTEAD OF INSERT, UPDATE
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @result int;
	exec @result = [dbo].[GetCountOfLandingPlanes] @landingTime='18:00:00.0000000';
	print @result;

END
