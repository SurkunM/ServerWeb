CREATE DATABASE Shop;
GO

USE Shop;
GO

CREATE TABLE Category (
    Id INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Product (
    Id INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    CategoryId INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,    
   
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryId)
    REFERENCES Category(Id)
);