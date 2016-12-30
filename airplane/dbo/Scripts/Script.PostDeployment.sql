MERGE INTO FlightState AS Target 
USING (VALUES 
  ('5EBE4A2F-F0C1-E611-B353-D017C293D790',N'normal'), 
  ('B2006D41-7688-42D9-A6DB-6AFD8C58AC1C',N'delayed')
) 
AS Source (idFlightState, name) 
ON Target.idFlightState = Source.idFlightState
-- update matched rows 
WHEN MATCHED THEN 
UPDATE SET Name = Source.name 
-- insert new rows 
WHEN NOT MATCHED BY TARGET THEN 
INSERT (idFlightState, name) 
VALUES (idFlightState, name) 
-- delete rows that are in the target but not the source 
WHEN NOT MATCHED BY SOURCE THEN DELETE;
