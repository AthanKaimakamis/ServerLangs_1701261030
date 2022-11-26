CREATE OR ALTER PROCEDURE [wh].[SP_AddProductType]
    @Name NVARCHAR(255),
    @Error NVARCHAR(255) OUTPUT
AS
BEGIN
    IF EXISTS (SELECT TOP 1 [Id] FROM [wh].[T_ProductTypes] WHERE [Name] LIKE @Name)
        BEGIN
            SET @Error = 'That product type already exists';
            RETURN;
        END
    BEGIN TRY
        INSERT INTO [wh].[T_ProductTypes] ([Name]) VALUES (@Name);
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