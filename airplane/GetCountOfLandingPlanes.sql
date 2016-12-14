CREATE FUNCTION [dbo].[GetCountOfLandingPlanes]
(
	@landingTime time
)
RETURNS INT
AS
BEGIN
	DECLARE @result int;
	--should we chceck arrival location?
	SET @result=(select count(arrivalTime) from Flight f inner join Arrival a on a.idArrival=f.idArrival where @landingTime=arrivalTime);
	RETURN @result;
END
