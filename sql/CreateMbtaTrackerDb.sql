CREATE DATABASE [MbtaTracker]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MbtaTracker', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\MbtaTracker.mdf' , SIZE = 4096KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MbtaTracker_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\MbtaTracker_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MbtaTracker] SET COMPATIBILITY_LEVEL = 120
GO
ALTER DATABASE [MbtaTracker] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MbtaTracker] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MbtaTracker] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MbtaTracker] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MbtaTracker] SET ARITHABORT OFF 
GO
ALTER DATABASE [MbtaTracker] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MbtaTracker] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MbtaTracker] SET AUTO_CREATE_STATISTICS ON(INCREMENTAL = OFF)
GO
ALTER DATABASE [MbtaTracker] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MbtaTracker] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MbtaTracker] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MbtaTracker] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MbtaTracker] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MbtaTracker] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MbtaTracker] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MbtaTracker] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MbtaTracker] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MbtaTracker] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MbtaTracker] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MbtaTracker] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MbtaTracker] SET  READ_WRITE 
GO
ALTER DATABASE [MbtaTracker] SET RECOVERY FULL 
GO
ALTER DATABASE [MbtaTracker] SET  MULTI_USER 
GO
ALTER DATABASE [MbtaTracker] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MbtaTracker] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [MbtaTracker] SET DELAYED_DURABILITY = DISABLED 
GO
USE [MbtaTracker]
GO
IF NOT EXISTS (SELECT name FROM sys.filegroups WHERE is_default=1 AND name = N'PRIMARY') ALTER DATABASE [MbtaTracker] MODIFY FILEGROUP [PRIMARY] DEFAULT
GO


CREATE SCHEMA GtfsStatic;
GO

CREATE SCHEMA MbtaRt;
GO

CREATE SCHEMA Display;
GO
