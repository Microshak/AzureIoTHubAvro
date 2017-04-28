WITH test AS
(
SELECT
    *
    ,UDF.ParseJSON(TestProp)
FROM
  AvroIotHub
  )
  
  select ParseJSON.car.name from test