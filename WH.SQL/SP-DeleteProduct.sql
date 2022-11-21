CREATE OR ALTER PROCEDURE wh.SP_DeleteProduct
    @pID INT,
    @Error NVARCHAR(255) OUTPUT
AS
BEGIN
    IF NOT EXISTS (SELECT TOP 1 [Id] FROM [wh].[T_Products] WHERE [Id] = @pID)
    BEGIN
        SET @Error = 'Product dose not exist. Operation canceled.';
        RETURN 1;
    END
    BEGIN TRY
        BEGIN TRANSACTION;
        DELETE FROM [wh].[T_Products] WHERE [Id] = @pID;
        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Error = 'An unexpected error occured. Please contact an Administrator!';
        EXECUTE [dbo].[SP_Log_DbError]
        @Message = ERROR_MESSAGE,
        @Line = ERROR_LINE,
        @Severity = ERROR_SEVERITY,
        @ErrorNumber = CAST(SELECT ERROR_NUMBER() AS INT);
    END CATCH
END