-- Create the stored procedure in the specified schema
CREATE OR ALTER PROCEDURE wh.LoginVerify
    @username NVARCHAR(15),
    @password  NVARCHAR(20),
    @errorMessage  NVARCHAR(255) OUTPUT
-- add more stored procedure parameters here
AS
BEGIN
    SET NOCOUNT ON
    DECLARE @userID INT
    -- body of the stored procedure
    IF EXISTS (SELECT TOP 1 u.uID FROM wh.Users u WHERE uLogin = @username)
    BEGIN
        SET @userID = (SELECT u.uID 
                        FROM  wh.Users u 
                        WHERE u.uLogin = @username 
                        AND u.uPasswordHash = HASHBYTES('SHA2_512',@password+CAST(uPwdSalt AS NVARCHAR(36)))
                    )
        IF(@userID IS NULL)
            SET @errorMessage = 'Incorrect Password!';
    END
    ELSE
        SET @errorMessage = 'User with username (' + @username + ') dosn' + CHAR(39) +  't exist!';
END