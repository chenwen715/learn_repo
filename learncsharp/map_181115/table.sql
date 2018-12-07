USE [master]
GO
/****** Object:  Database [ACSNEW]    Script Date: 2018/12/6 9:10:45 ******/
CREATE DATABASE [ACSNEW]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ACSNEW', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\ACSNEW.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ACSNEW_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\ACSNEW_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ACSNEW].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ACSNEW] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ACSNEW] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ACSNEW] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ACSNEW] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ACSNEW] SET ARITHABORT OFF 
GO
ALTER DATABASE [ACSNEW] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ACSNEW] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ACSNEW] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ACSNEW] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ACSNEW] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ACSNEW] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ACSNEW] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ACSNEW] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ACSNEW] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ACSNEW] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ACSNEW] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ACSNEW] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ACSNEW] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ACSNEW] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ACSNEW] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ACSNEW] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ACSNEW] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ACSNEW] SET RECOVERY FULL 
GO
ALTER DATABASE [ACSNEW] SET  MULTI_USER 
GO
ALTER DATABASE [ACSNEW] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ACSNEW] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ACSNEW] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ACSNEW] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ACSNEW', N'ON'
GO
USE [ACSNEW]
GO
/****** Object:  Table [dbo].[T_PathPoint]    Script Date: 2018/12/6 9:10:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_PathPoint](
	[PointNo] [nvarchar](50) NOT NULL,
	[PointX] [int] NOT NULL,
	[PointY] [int] NOT NULL,
	[x] [int] NOT NULL,
	[y] [int] NOT NULL,
	[pointType] [int] NULL,
	[intStationCode] [int] NULL,
	[wsType] [int] NULL,
	[beforex] [int] NULL,
	[beforey] [int] NULL,
	[afterx] [int] NULL,
	[aftery] [int] NULL,
	[isOccupy] [bit] NULL,
	[occupyAgv] [nvarchar](50) NULL,
	[area] [int] NULL
) ON [PRIMARY]

GO
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (N'0101', 1, 1, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (N'0102', 1, 2, 1, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (N'0103', 1, 3, 1, 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (N'0201', 2, 1, 2, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (N'0202', 2, 2, 2, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (N'0203', 2, 3, 2, 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (N'0301', 3, 1, 3, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (N'0302', 3, 2, 3, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[T_PathPoint] ([PointNo], [PointX], [PointY], [x], [y], [pointType], [intStationCode], [wsType], [beforex], [beforey], [afterx], [aftery], [isOccupy], [occupyAgv], [area]) VALUES (N'0303', 3, 3, 3, 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
USE [master]
GO
ALTER DATABASE [ACSNEW] SET  READ_WRITE 
GO
