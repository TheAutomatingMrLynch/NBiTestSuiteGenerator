SELECT [dimension] ,[perspective]
FROM ( VALUES 
	('DimA','PerspectiveX'),
	('DimB','PerspectiveX'),
	('DimA','PerspectiveY')
) Dimensions(dimension,perspective)