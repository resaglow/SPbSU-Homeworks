﻿CREATE TABLE [Client]
(
    [Id] INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    [Name] NVARCHAR(MAX) NOT NULL,
    [Address] NVARCHAR(MAX) NOT NULL, 
    [District] NVARCHAR(30) NOT NULL,
    [Phone] NCHAR(11) NOT NULL, 
    [IsPermanent] BIT NOT NULL
)