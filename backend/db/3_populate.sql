USe MoneyMe;
GO


INSERT INTO BlackList (ID, KeyType, KeyValue, Remarks) VALUES ('87fe82b6-9df3-4ef0-91f0-5ff175c48770', 'MobileNumber', '639100000000', 'Fraudulent activity');
INSERT INTO BlackList (ID, KeyType, KeyValue, Remarks) VALUES ('ad19a2cf-5c61-4266-9cbd-f73da0a6c2a6', 'EmailDomain', 'yahoo.com', 'User blocked');
INSERT INTO BlackList (ID, KeyType, KeyValue, Remarks) VALUES ('1ee66a71-6aee-496e-86f3-5faae3132b95', 'MobileNumber', '639100000002', 'Spam');
INSERT INTO BlackList (ID, KeyType, KeyValue, Remarks) VALUES ('7504ae78-ced7-419c-a946-aefee7fdc31a', 'EmailDomain', 'mailgun.com', 'High risk');
INSERT INTO BlackList (ID, KeyType, KeyValue, Remarks) VALUES ('9578ce4f-e646-4862-a941-872c5e465785', 'MobileNumber', '639100000004', 'Violation of terms');
INSERT INTO BlackList (ID, KeyType, KeyValue, Remarks) VALUES ('2079eb04-8d5a-43c5-b670-4a96cf8513bd', 'EmailDomain', 'monkeymail.com', 'Fraudulent activity');
INSERT INTO BlackList (ID, KeyType, KeyValue, Remarks) VALUES ('34542c41-d0a2-4df5-862c-5503f55ca945', 'MobileNumber', '639100000006', 'User blocked');
INSERT INTO BlackList (ID, KeyType, KeyValue, Remarks) VALUES ('6d99b1e4-c9f6-45cc-86dc-6fe3467f3d47', 'EmailDomain', 'duterte.com', 'Spam');
INSERT INTO BlackList (ID, KeyType, KeyValue, Remarks) VALUES ('76f36527-6b83-4ac5-baef-2fab92954bbf', 'MobileNumber', '639100000008', 'High risk');
INSERT INTO BlackList (ID, KeyType, KeyValue, Remarks) VALUES ('c32cc501-f403-47d2-8370-7ef9a4fd208a', 'EmailDomain', 'sip.com', 'Violation of terms');





INSERT INTO ClientLoan (ID, AmountRequired, Term, Title, FirstName, LastName, DateOfBirth, PhoneNumber, Email, Product, Payment)
VALUES ('01FZ7DK3SP0P3TH6H9HZB89JBC', 5000.00, '24', 'Mr', 'Sharpedo', 'Doe', '1985-03-25', '+639171234567', 'sharpedo@.com', 'ProductA', 416.67);

INSERT INTO ClientLoan (ID, AmountRequired, Term, Title, FirstName, LastName, DateOfBirth, PhoneNumber, Email, Product, Payment)
VALUES ('01FZ7DK7AQPY4VRNMFQYT3HEDG', 3000.00, '12', 'Ms', 'Sharpedo', 'Smith', '1990-07-15', '+639171234568', 'sharpedo.smith@example.com', 'ProductB', 250.00);

INSERT INTO ClientLoan (ID, AmountRequired, Term, Title, FirstName, LastName, DateOfBirth, PhoneNumber, Email, Product, Payment)
VALUES ('01FZ7DKAB2TD4MF4TMGFW7RKY4', 10000.00, '36', 'Dr', 'Sharpedo', 'Johnson', '1978-11-30', '+639171234569', 'sharpedo.johnson@example.com', 'ProductC', 833.33);

INSERT INTO ClientLoan (ID, AmountRequired, Term, Title, FirstName, LastName, DateOfBirth, PhoneNumber, Email, Product, Payment)
VALUES ('01FZ7DKCMFEFJDEJPAMR9W4FEX', 7000.00, '18', 'Mr', 'Sharpedo', 'Brown', '1980-09-12', '+639171234570', 'sharpedo.brown@example.com', 'ProductB', 583.33);

INSERT INTO ClientLoan (ID, AmountRequired, Term, Title, FirstName, LastName, DateOfBirth, PhoneNumber, Email, Product, Payment)
VALUES ('01FZ7DKGJMNPKJK3QKQXJNF8GM', 4500.00, '6', 'Mrs', 'Sharpedo', 'Davis', '1995-02-10', '+639171234571', 'sharpedo.davis@example.com', 'ProductA', 750.00);
