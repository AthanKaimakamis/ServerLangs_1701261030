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
        BEGIN TRANSACTION;
        INSERT INTO [wh].[T_ProductTypes] ([Name]) VALUES (@Name);
        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @Error = 'An unexpected error occured. Please contact an Administrator!';
        EXECUTE [dbo].[Log_DbError]
            @Message = ERROR_MESSAGE,
            @Line = ERROR_LINE,
            @Severity = ERROR_SEVERITY,
            @ErrorNumber = CAST(select ERROR_NUMBER() as INT);
    END CATCH
END