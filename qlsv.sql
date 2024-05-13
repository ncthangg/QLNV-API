select * from dbo.[User]
select * from Salary
select * from dbo.[Role]
select * from JobPosition
select * from UserRequests
select * from ResponseLogin
select * from RefreshTokens

INSERT INTO [User] (UserID, Name, Email, Password, Sex, BirthDay, Address, JobID, RoleID)
VALUES ('ad1', 'Coco', 'coco@gmail.com', '12345', 'Male', '2022-02-02', '123 Man City', 1, 1);

INSERT INTO [User] (UserID, Name, Email, Password, Sex, BirthDay, Address, JobID, RoleID)
VALUES ('us1', 'John Doe', 'john@example.com', '12345', 'Male', '1990-01-01', '123 Main Street', 1, 1);

INSERT INTO JobPosition ( JobName)
VALUES 
    ('Manager'),
    ('Developer');

INSERT INTO dbo.[Role] ( RoleName)
VALUES 
    ('Admin'),
    ('Employee');

DROP TABLE dbo.[User]
DROP TABLE Salary
DROP TABLE dbo.[Role]
DROP TABLE JobPosition
DROP TABLE UserRequests

ALTER TABLE JobPosition
DROP COLUMN UserUserID;

Drop database QuanLiNhanVien

Create Database QuanLiNhanVien

CREATE TABLE [User] 
(UserID varchar(255) NOT NULL,
Name varchar(255) NULL,
Email varchar(255)NOT NULL,
Password varchar(255)NOT NULL,
Sex varchar(255) NULL,
BirthDay date NULL, 
Address varchar(255) NULL,
JobID int NOT NULL, 
RoleID int NOT NULL, 
PRIMARY KEY (UserID));

CREATE TABLE Salary 
(ID int IDENTITY NOT NULL,
UserID varchar(255) NOT NULL,
Month int NOT NULL,
ContractSalary int NULL,
DayOff int NULL, 
TotalSalary decimal null;


CREATE TABLE JobPosition 
(JobID int IDENTITY NOT NULL,
JobName varchar(255) NOT NULL,
PRIMARY KEY (JobID));

CREATE TABLE Claims 
(ClaimID int IDENTITY NOT NULL,
ClaimName varchar(255) NOT NULL,
PRIMARY KEY (ClaimID));

CREATE TABLE Role (
RoleID int IDENTITY NOT NULL, 
RoleName varchar(255) NOT NULL, PRIMARY KEY (RoleID));

CREATE TABLE Claims_User (
UserID varchar(255) NOT NULL,
ClaimID int NOT NULL,
PRIMARY KEY (UserID, ClaimID));

ALTER TABLE Salary ADD CONSTRAINT FKSalary567207 FOREIGN KEY (UserID) REFERENCES [User] (UserID);

ALTER TABLE [User] ADD CONSTRAINT FKUser872629 FOREIGN KEY (JobID) REFERENCES JobPosition (JobID);

ALTER TABLE [User] ADD CONSTRAINT FKUser68364 FOREIGN KEY (RoleID) REFERENCES Role (RoleID);

ALTER TABLE Claims_User ADD CONSTRAINT FKClaims_Use53699 FOREIGN KEY (ClaimID) REFERENCES Claims (ClaimID);

ALTER TABLE Claims_User ADD CONSTRAINT FKClaims_Use763544 FOREIGN KEY (UserID) REFERENCES [User] (UserID);

CREATE TABLE UserRequests (
    ID INT PRIMARY KEY IDENTITY,
    UserID varchar(255) NOT NULL,
	Email varchar(255) NOT NULL,
    Reason NVARCHAR(MAX),
    Attachment NVARCHAR(MAX),
    DayTime [datetime],
    CONSTRAINT FK_UserRequests_UserId FOREIGN KEY (UserID) REFERENCES dbo.[User](UserID)
);