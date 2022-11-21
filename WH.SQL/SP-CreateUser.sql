CREATE OR ALTER PROCEDURE [wh].[SP_CreateUser]
    @Username NVARCHAR(15),
    @Password NVARCHAR(20),
    @Email NVARCHAR(320),
    @Phone NVARCHAR(30),
    @Error NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    DECLARE 
        @salt UNIQUEIDENTIFIER = NEWID()

    IF EXISTS (SELECT TOP 1 [uID] FROM [wh].[Users] WHERE [uUsername] LIKE @Username)
        BEGIN
            SET @Error = 'Username is already taken';
            RETURN
        END
    
    IF EXISTS (SELECT TOP 1 [uID] FROM [wh].[Users] WHERE [uEmail] like @Email)
        BEGIN
            SET @Error = 'This email is used on another account';
            RETURN
        END

    BEGIN TRY
        INSERT INTO [wh].[Users]
            ([uUsername], [uPasswordHash], [uPwdSalt], [uEmail], [uPhone])
        VALUES
            (@Username, HASHBYTES('SHA2_512', @Password+CAST(@salt AS NVARCHAR(36))), @salt, @Email,@Phone)
    END TRY
    BEGIN CATCH
        SET @Error = ERROR_MESSAGE();
        EXECUTE [dbo].[Log_DbError]
            @Message = ERROR_MESSAGE,
            @Line = ERROR_LINE,
            @Severity = ERROR_SEVERITY,
            @ErrorNumber = CAST(select ERROR_NUMBER() as INT);
    END  CATCH
END
GO