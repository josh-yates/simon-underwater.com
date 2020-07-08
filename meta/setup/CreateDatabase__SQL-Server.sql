USE master;
GO
CREATE LOGIN SimonUnderwaterLocal WITH PASSWORD = 'Password.123abc';    
GO
CREATE DATABASE SimonUnderwaterLocal;
GO

USE [SimonUnderwaterLocal]
GO
CREATE USER SimonUnderwaterLocal FOR LOGIN SimonUnderwaterLocal;
GO
EXEC sp_addrolemember N'db_owner', N'SimonUnderwaterLocal';
GO
CREATE SCHEMA [SimonUnderwaterLocal] AUTHORIZATION [SimonUnderwaterLocal]
GO