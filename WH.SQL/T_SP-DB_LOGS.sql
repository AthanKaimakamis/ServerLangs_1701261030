IF OBJECT_ID('[wh].[DB_LOGS]', 'U') IS NOT NULL
DROP TABLE [wh].[DB_LOGS]
GO
CREATE TABLE [wh].[DB_LOGS]
(
    l_ID INT IDENTITY NOT NULL PRIMARY KEY,
    l_DateTime DATETIME NOT NULL,
    l_ErrorNumber INT NOT NULL,
    l_Message NVARCHAR(255) NOT NULL,
    l_Line INT,
    l_Severity NVARCHAR(80)
);

GO
CREATE OR ALTER PROCEDURE [dbo].[SP_Log_DbError]
    @ErrorNumber INT,
    @Message NVARCHAR(255),
    @Line  INT,
    @Severity NVARCHAR(80)
AS
BEGIN
    INSERT INTO [dbo].[DB_LOGS] 
        ([l_DateTime], [l_ErrorNumber], [l_Message], [l_Line], [l_Severity])
    VALUES
        (GETDATE(),CAST(@ErrorNumber AS INT),@Message,CAST(@Line AS INT),@Severity);
END
GO

--SELECT * FROM 