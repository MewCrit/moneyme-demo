USe MoneyMe;
GO

CREATE PROCEDURE sp_CreateClientLoan
    @ID nvarchar(50),
    @AmountRequired decimal,
    @Term nvarchar(5),
    @Title nvarchar(20),
    @FirstName nvarchar(20),
    @LastName nvarchar(20),
    @DateOfBirth datetime,
    @PhoneNumber nvarchar(20),
    @Email nvarchar(20),
    @Product nvarchar(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ClientLoan (ID, AmountRequired, Term, Title, FirstName, LastName, DateOfBirth, PhoneNumber, Email, Product )
           VALUES (@ID, @AmountRequired, @Term, @Title, @FirstName, @LastName, @DateOfBirth, @PhoneNumber, @Email, @Product);
END
GO   


CREATE PROCEDURE sp_FindClientLoanByID
    @ID nvarchar(50)
    AS
    BEGIN
        SET NOCOUNT ON;
        SELECT ID, AmountRequired, Term, Title, FirstName, LastName, DateOfBirth, PhoneNumber, Email, Product FROM ClientLoan
        WHERE ID=@ID
END
GO


CREATE PROCEDURE sp_ValidateBlackList
    @KeyValue nvarchar(50),
    @KeyType nvarchar(50)
    AS
    BEGIN
         SET NOCOUNT ON;
         SELECT 1 FROM BlackList WHERE KeyValue=@KeyValue AND KeyType=@KeyType
    END
    GO



CREATE PROCEDURE sp_UpdateClientLoan
    @ID nvarchar(50),
    @AmountRequired decimal,
    @Term nvarchar(5),
    @Title nvarchar(20),
    @FirstName nvarchar(20),
    @LastName nvarchar(20),
    @DateOfBirth datetime,
    @PhoneNumber nvarchar(20),
    @Email nvarchar(50),
    @Product nvarchar(20)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ClientLoan  SET AmountRequired = @AmountRequired,
            Term = @Term, Title = @Title,
            FirstName = @FirstName, LastName = @LastName,
            DateOfBirth = @DateOfBirth, PhoneNumber = @PhoneNumber,
            Email = @Email, Product = @Product
        WHERE ID = @ID;
END
GO


CREATE PROCEDURE sp_CreateClientLoan
    @ID nvarchar(50),
    @LoanAMount decimal,
    @Term nvarchar(5),
    @Title nvarchar(20),
    @FirstName nvarchar(20),
    @LastName nvarchar(20),
    @DateOfBirth datetime,
    @PhoneNumber nvarchar(20),
    @Email nvarchar(20),
    @Product nvarchar(20)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ClientLoan (ID, AmountRequired, Term, Title, FirstName, LastName, DateOfBirth, PhoneNumber, Email, Product )
           VALUES (@ID, @AmountRequired, @Term, @Title, @FirstName, @LastName, @DateOfBirth, @PhoneNumber, @Email, @Product);
END
GO



CREATE PROCEDURE sp_CreateFinalLoan
    @ID nvarchar(50),
    @ClientLoanID nvarchar(30),
    @LoanAmount decimal(18, 2),
    @Term nvarchar(100),
    @Title nvarchar(20),
    @FirstName nvarchar(20),
    @LastName nvarchar(20),
    @DateOfBirth datetime,
    @PhoneNumber nvarchar(20),
    @Email nvarchar(20),
    @Product nvarchar(20),
    @Repayment decimal(18, 2),
    @TotalPayment decimal(18, 2)
AS
BEGIN
SET NOCOUNT ON;
    INSERT INTO FinalLoan (ID, ClientLoanID, LoanAmount, Term, Title, FirstName, LastName, DateOfBirth, PhoneNumber, Email, Product,    Repayment, TotalPayment)
    VALUES (@ID, @ClientLoanID, @LoanAmount, @Term, @Title, @FirstName, @LastName, @DateOfBirth, @PhoneNumber, @Email, @Product, @Repayment, @TotalPayment);
END;
GO
