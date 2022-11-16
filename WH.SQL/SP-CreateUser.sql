-- Create a new stored procedure called 'StoredProcedureName' in schema 'dbo'
-- Create the stored procedure in the specified schema
CREATE OR ALTER PROCEDURE wh.CreateUser
    @Username NVARCHAR(15),
    @Password NVARCHAR(20),
    @Email NVARCHAR(320),
    @Phone NVARCHAR(30),
    @Error BIT = 0 OUTPUT
-- add more stored procedure parameters here
AS
BEGIN
    SET NOCOUNT ON
    -- body of the stored procedure 
    DECLARE 
        @salt UNIQUEIDENTIFIER = NEWID()
    BEGIN TRY
        INSERT INTO wh.Users
            (uUsername, uPasswordHash, uPwdSalt, uEmail, uPhone)
        VALUES
            (@Username, HASHBYTES('SHA2_512', @Password+CAST(@salt AS NVARCHAR(36))), @salt, @Email,@Phone)
    END TRY
    BEGIN CATCH
        SET @Error = ERROR_STATE();
        EXECUTE [dbo].[Log_DbError]
            @Message = ERROR_MESSAGE,
            @Line = ERROR_LINE,
            @Severity = ERROR_SEVERITY,
            @ErrorNumber = CAST(select ERROR_NUMBER() as INT);
            
        --SET @Log = CONCAT('{"ErrorNumber":"',ERROR_NUMBER(),'", ',
        --                '"Message":"',ERROR_MESSAGE(),'", ',
        --                '"Procedure":"',ERROR_PROCEDURE(),'", ',
        --                '"Line":"',ERROR_LINE(),'", ',
        --                '"Severity":"',ERROR_SEVERITY(),'"}');

    END  CATCH
END
GO