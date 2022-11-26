-- Create the stored procedure in the specified schema
CREATE OR ALTER PROCEDURE [wh].[SP_LoginVerify]
    @username NVARCHAR(15),
    @password  NVARCHAR(20),
    @Error  NVARCHAR(255) OUTPUT
-- add more stored procedure parameters here
AS
BEGIN
    SET NOCOUNT ON
    DECLARE @userID INT
    -- body of the stored procedure
    IF EXISTS (SELECT TOP 1 [uID] FROM [wh].[Users] WHERE [uUsername] = @username)
    BEGIN
        SET @userID = (SELECT [uID] FROM  [wh].[Users] 
                        WHERE [uUsername] = @username 
                        AND [uPasswordHash] = HASHBYTES('SHA2_512',@password+CAST(uPwdSalt AS NVARCHAR(36)))
                    )
        IF(@userID IS NULL)
            SET @Error = 'Incorrect Password!';
    END
    ELSE
        SET @Error = 'User with username (' + @username + ') dosn' + CHAR(39) +  't exist!';
END