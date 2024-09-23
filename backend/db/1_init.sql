CREATE DATABASE MoneyMe;
GO
USE MoneyMe;
GO

CREATE TABLE ClientLoan (
    ID nvarchar(50) NOT NULL PRIMARY KEY,
    AmountRequired decimal NOT NULL,
    Term nvarchar(100),
    Title nvarchar(20),
    FirstName nvarchar(20),
    LastName nvarchar(20),
    DateOfBirth datetime,
    PhoneNumber nvarchar(20),
    Email nvarchar(20),
    Product nvarchar(20),
    Payment decimal(18, 2)
);
GO


CREATE TABLE BlackList (
    ID nvarchar(50) NOT NULL PRIMARY KEY,
    KeyType nvarchar(20),
    KeyValue nvarchar(30),
    Remarks nvarchar(50)
);
GO

CREATE TABLE FinalLoan (
    ID nvarchar(50) NOT NULL PRIMARY KEY,
    ClientLoanID nvarchar(30),
    LoanAmount decimal(18, 2) NOT NULL,
    Term nvarchar(100),
    Title nvarchar(20),
    FirstName nvarchar(20),
    LastName nvarchar(20),
    DateOfBirth datetime,
    PhoneNumber nvarchar(20),
    Email nvarchar(20),
    Product nvarchar(20),
    Repayment decimal(18, 2),   
    TotalPayment decimal(18, 2) 
);
GO
