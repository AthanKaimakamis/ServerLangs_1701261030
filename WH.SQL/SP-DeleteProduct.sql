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
        DELETE FROM [wh].[T_Products] WHERE [Id] = @pID;
    END TRY
    BEGIN CATCH
        SET @Error = 'Error occured in Database. Please contact an Administrator!';
        DECLARE 
            @ErrorNumber SMALLINT = ERROR_NUMBER(),
            @Severity SMALLINT = ERROR_SEVERITY(),
            @State SMALLINT = ERROR_STATE(),
            @Procedure NVARCHAR(128) = ERROR_PROCEDURE(),
            @Line SMALLINT = ERROR_LINE(),
            @Message NVARCHAR(MAX) = ERROR_MESSAGE()
        EXECUTE [dbo].[SP_Log_DbError] @ErrorNumber, @Severity, @State, @Procedure, @Line, @Message;
    END CATCH
END