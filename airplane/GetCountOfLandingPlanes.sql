--DECLARE @result INT
--EXEC @result = [dbo].[GetCountOfLandingPlanes] @landingDT='12/18/2016 6:00:00 PM'
--PRINT @result

CREATE FUNCTION [dbo].[GetCountOfLandingPlanes]
(
	@landingDT DATETIME
)
RETURNS INT
AS
BEGIN
	DECLARE @result INT;
	--landingtime range <+-5>	
	DECLARE @timeFrom DATETIME= DATEADD(MINUTE,-5,@landingDT)
	DECLARE @timeTo DATETIME= DATEADD(MINUTE,5,@landingDT)

SELECT @result = (SELECT COUNT(arrivalDT)
				FROM Schedule s
					INNER JOIN Flight f
					ON s.idFlight=f.idFlight
					INNER JOIN Arrival a
					ON a.idArrival=f.idArrival
				WHERE arrivalDT
					BETWEEN @timeFrom
					AND @timeTo
				--city hardcoded
				AND idCity='ecf5b8ed-f0c1-e611-b353-d017c293d790')

	RETURN @result;
END
