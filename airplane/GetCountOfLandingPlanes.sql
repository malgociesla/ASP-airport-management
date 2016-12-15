--DECLARE @result INT;
--EXEC @result = [dbo].[GetCountOfLandingPlanes] @landingTime='2016-12-14';
--PRINT @result;

CREATE FUNCTION [dbo].[GetCountOfLandingPlanes]
(
	@landingTime TIME
)
RETURNS INT
AS
BEGIN
	DECLARE @result INT;
	--should we chceck arrival location?
	SET @result=(SELECT COUNT(arrivalTime)
				FROM Flight f
					INNER JOIN Arrival a
					ON a.idArrival=f.idArrival
				WHERE @landingTime=arrivalTime);
	RETURN @result;
END
