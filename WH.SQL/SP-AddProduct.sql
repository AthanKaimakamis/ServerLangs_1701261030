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
    BEGIN TRY
        IF @pID IS NOT NULL
        BEGIN
            SET @pAmount = @pAmount + (SELECT [Amount] FROM [wh].[T_Products] WHERE [Id] = @pID);
            EXECUTE [wh].[SP_UpdateProduct] @pID, 
                                            @pAmount = @pAmount,
                                            @Error = @Error;
            RETURN;
        END
        INSERT INTO [wh].[T_Products]
            ([TypeID],[Name],[Description],[ImageB64],[BoughtPrice],[SellPrice],[Amount]) 
        VALUES 
            (@pTypeID,@pName,@pDescription,@pImageB64,@pBoughtPrice,@pSellPrice,@pAmount);
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