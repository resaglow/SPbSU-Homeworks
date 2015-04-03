﻿CREATE TABLE [dbo].[Order]
(
    [Number] INT NOT NULL IDENTITY (1,1) PRIMARY KEY, 
    [Date] DATE NOT NULL, 
    [Time] TIME NOT NULL,
    [Cost] MONEY NOT NULL, 
    [PaymentType] NVARCHAR(MAX) NOT NULL,
    [ClientId] INT NULL FOREIGN KEY REFERENCES [Client]([Id]) ON DELETE SET NULL,    
    [CarNumber] NCHAR(10) NOT NULL FOREIGN KEY REFERENCES [Car]([Number]) ON UPDATE CASCADE
)