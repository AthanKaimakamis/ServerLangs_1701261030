CREATE OR ALTER PROCEDURE wh.SP_UpdateProduct
    @pID INT,
    @pTypeId INT = NULL,
    @pName NVARCHAR(50) = NULL,
    @pDesc NVARCHAR(2000) = null,
    @pImg NVARCHAR(64) = null,
    @pBPrice FLOAT = NULL,
    @pSPrice FLOAT = Null,
    @pAmount INT = NULL,
    @Error NVARCHAR(255) OUTPUT
AS
DECLARE
 @Prod Table (pID INT , pTypeId INT,
            pName NVARCHAR(50), pDesc NVARCHAR(2000), pImg NVARCHAR(64),
            pBPrice FLOAT, pSPrice FLOAT, pAmount INT)
BEGIN
    IF EXISTS (SELECT TOP 1 [Id] FROM [wh].[T_Products] WHERE [Id] = @pID)
        SELECT * INTO [@Prod] FROM [wh].[T_Products] WHERE [Id] = @pID;
    ELSE BEGIN
        SET @Error = 'Product dose not exist. Operation canceled.';
        RETURN 1;
    END
    BEGIN TRY
        IF (@pTypeId IS NOT NULL AND (SELECT pTypeId FROM @Prod) <> @pTypeId)
            UPDATE @Prod SET pTypeID = @pTypeID;
        IF (@pName IS NOT NULL AND (SELECT pName FROM @Prod) <> @pName)
            UPDATE @Prod SET pName = @pName;
        IF (@pDesc IS NOT NULL AND (SELECT pDesc FROM @Prod) <> @pDesc)
            UPDATE @Prod SET pDesc = @pDesc;
        IF (@pImg IS NOT NULL AND (SELECT pImg FROM @Prod) <> @pImg)
            UPDATE @Prod SET pImg = @pImg;
        IF (@pBPrice IS NOT NULL AND (SELECT pBPrice FROM @Prod) <> @pBPrice)
            UPDATE @Prod SET pBPrice = @pBPrice;
        IF (@pSPrice IS NOT NULL AND (SELECT pSPrice FROM @Prod) <> @pSPrice)
            UPDATE @Prod SET pSPrice = @pSPrice;
        IF (@pAmount IS NOT NULL AND (SELECT pAmount FROM @Prod) <> @pAmount)
            UPDATE @Prod SET pAmount = @pAmount;
        BEGIN TRANSACTION;
        UPDATE [wh].[T_Products]
           SET [TypeId] = (SELECT pTypeId FROM @Prod),
               [Name] = (SELECT pName FROM @Prod),
               [Description] = (SELECT pDesc FROM @Prod),
               [ImageB64] = (SELECT pImg FROM @Prod),
               [BoughtPrice] = (SELECT pBPrice FROM @Prod),
               [SellPrice] = (SELECT pSPrice FROM @Prod),
               [Amount] = (SELECT pAmount FROM @Prod)
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