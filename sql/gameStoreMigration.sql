info: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core 2.1.3-rtm-32065 initialized 'GcdGameStoreContext' using provider 'Microsoft.EntityFrameworkCore.SqlServer' with options: None
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001152213_GameModel')
BEGIN
    CREATE TABLE [employee] (
        [employeeId] int NOT NULL IDENTITY,
        [name] nvarchar(255) NULL,
        [pwHash] nvarchar(30) NULL,
        CONSTRAINT [PK_employee] PRIMARY KEY NONCLUSTERED ([employeeId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001152213_GameModel')
BEGIN
    CREATE TABLE [Game] (
        [GameID] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NULL,
        [ReleaseDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Game] PRIMARY KEY ([GameID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001152213_GameModel')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181001152213_GameModel', N'2.1.3-rtm-32065');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181001153433_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181001153433_InitialCreate', N'2.1.3-rtm-32065');
END;

GO


