CREATE TABLE PI_Provider(
P_ID INT IDENTITY NOT NULL,
P_Name NVARCHAR(100) UNIQUE NOT NULL,
P_Business_Name NVARCHAR(100),
P_NIT NVARCHAR(11),
P_Address NVARCHAR(200),
P_PhoneNumber NVARCHAR(15),
P_Rating_Number DECIMAL(10,1),
P_Creation_Date DATE NOT NULL,
P_Modification_Date DATE NULL
)
GO

CREATE PROCEDURE PI_Provider_Insert (
@Name NVARCHAR(100),
@Business_Name NVARCHAR(100) = '',
@NIT NVARCHAR(11) = '',
@Address NVARCHAR(200) = '',
@PhoneNumber NVARCHAR(15) = '' ,
@Rating_Number NVARCHAR(3) = '0.0'
)
AS 
BEGIN
	INSERT INTO PI_Provider (
		P_Name,
		P_Business_Name,
		P_NIT,
		P_Address,
		P_PhoneNumber,
		P_Rating_Number,
		P_Creation_Date
	) 
	VALUES (
		@Name, 
		@Business_Name, 
		@NIT, 
		@Address, 
		@PhoneNumber,
		CAST(@Rating_Number AS DECIMAL(10,1)), 
		CAST(GETDATE() AS DATE)
	);

	SELECT @@IDENTITY AS ID;
END
GO

CREATE PROCEDURE PI_Provider_Update (
@Id INT,
@Name NVARCHAR(100),
@Business_Name NVARCHAR(100) = '',
@NIT NVARCHAR(11) = '',
@Address NVARCHAR(200) = '',
@PhoneNumber NVARCHAR(15) = '' ,
@Rating_Number NVARCHAR(3) = '0.0'
)
AS 
BEGIN
	UPDATE PI_Provider 
	SET	
		P_Name = @Name,
		P_Business_Name = @Business_Name,
		P_NIT = @NIT,
		P_Address = @Address,
		P_PhoneNumber = @PhoneNumber,
		P_Rating_Number = @Rating_Number,
		P_Modification_Date = CAST(GETDATE() AS DATE)
	WHERE 
		P_ID = @Id;

	SELECT @@IDENTITY AS ID;
END
GO

CREATE PROCEDURE PI_Provider_Delete (
@Id INT
)
AS 
BEGIN
	DELETE FROM PI_Provider WHERE P_ID = @Id;

	SELECT @@IDENTITY AS ID;
END
GO

CREATE VIEW PI_Provider_Select
AS 
SELECT ROW_NUMBER() OVER(ORDER BY P_ID ASC) AS P_Order,P.* FROM PI_Provider P
GO
