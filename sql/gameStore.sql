USE [master]
GO
/****** Object:  Database [GS]   ******/
CREATE DATABASE [GS]

GO
ALTER DATABASE [GS] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GS] SET ARITHABORT OFF 
GO
ALTER DATABASE [GS] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [GS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GS] SET  MULTI_USER 
GO
ALTER DATABASE [GS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GS] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [GS] SET DELAYED_DURABILITY = DISABLED 
GO
USE [GS]
GO
/****** Object:  User [fred]    Script Date: 2017-12-29 8:08:53 PM ******/
CREATE USER [fred] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [fred]
GO

/****** Object:  Table [dbo].[employee]   ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[employee](
	[employeeId] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NULL,
	[pwHash] [nvarchar](30) NULL
) ON [PRIMARY]

SET IDENTITY_INSERT [dbo].[employee] ON 
INSERT [dbo].[employee] ([employeeId], [name], [pwHash]) VALUES (1, N'Mike', N'password1')
INSERT [dbo].[employee] ([employeeId], [name], [pwHash]) VALUES (2, N'Steve', N'password2')
SET IDENTITY_INSERT [dbo].[employee] OFF

/****** Object:  Index [aaaaaemployee_PK]    Script Date: 2017-12-29 8:08:53 PM ******/
ALTER TABLE [dbo].[employee] ADD  CONSTRAINT [aaaaaemployee_PK] PRIMARY KEY NONCLUSTERED 
(
	[employeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'17' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'employeeId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'employeeId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'employee' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'employeeId'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'generic employee planted' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'employeeName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'employeeName' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'employee' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'name'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'????????' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'picture file-name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'pwHash' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'30' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'pwHash' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'employee' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee', @level2type=N'COLUMN',@level2name=N'pwHash'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'DateCreated', @value=N'04/05/2009 8:31:26 AM' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'DisplayViewsOnSharePointSite', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'FilterOnLoad', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'HideNewField', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'LastUpdated', @value=N'23/08/2009 4:41:42 PM' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DefaultView', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_OrderByOn', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Orientation', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'employee' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'OrderByOnLoad', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'RecordCount', @value=N'8' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'TotalsRow', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO
EXEC sys.sp_addextendedproperty @name=N'Updatable', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'employee'
GO

USE [master]
GO
ALTER DATABASE [gs] SET  READ_WRITE 
GO