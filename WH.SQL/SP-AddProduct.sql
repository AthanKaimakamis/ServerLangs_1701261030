CREATE OR ALTER PROCEDURE [wh].[SP_AddProduct]
    @pTypeId INT,
    @pName NVARCHAR(50),
    @pDescription NVARCHAR(2000),
    @pImageB64 NVARCHAR(64),
    @pBoughtPrice FLOAT,
    @pSellPrice FLOAT,
    @pAmount INT,
    @Error NVARCHAR(255) OUTPUT
AS
DECLARE
    @pID INT = NULL
BEGIN
    SET @pID = (SELECT [Id] FROM [wh].[T_Products] WHERE [Name] = @pName);
    IF @pID IS NOT NULL
    BEGIN
        SET @pAmount = @pAmount + (SELECT [Amount] FROM [wh].[T_Products] WHERE [Id] = @pID);
        EXECUTE [wh].[SP_UpdateProduct] @pID, 
                                        @pAmount = @pAmount,
                                        @Error = @Error;
        RETURN;
    END
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO [wh].[T_Products]
            ([TypeID],[Name],[Description],[ImageB64],[BoughtPrice],[SellPrice],[Amount]) 
        VALUES 
            (@pTypeID,@pName,@pDescription,@pImageB64,@pBoughtPrice,@pSellPrice,@pAmount);
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