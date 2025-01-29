
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/28/2025 15:10:35
-- Generated from EDMX file: C:\Users\ersan\source\repos\HRDemoAPI\HRDemoAPI.Data\HRDemoApiDb.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [HRDemoApi];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_EmployeeDepartment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_EmployeeDepartment];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentManager]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Departments] DROP CONSTRAINT [FK_DepartmentManager];
GO
IF OBJECT_ID(N'[dbo].[FK_UserEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeAttendance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Attendances] DROP CONSTRAINT [FK_EmployeeAttendance];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeLeave]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Leaves] DROP CONSTRAINT [FK_EmployeeLeave];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeePayroll]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payrolls] DROP CONSTRAINT [FK_EmployeePayroll];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Departments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Departments];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Attendances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Attendances];
GO
IF OBJECT_ID(N'[dbo].[Leaves]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Leaves];
GO
IF OBJECT_ID(N'[dbo].[Payrolls]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payrolls];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [EmployeeID] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NOT NULL,
    [JobTitle] nvarchar(max)  NOT NULL,
    [DateOfHire] datetimeoffset  NOT NULL,
    [Status] smallint  NOT NULL,
    [Address_Line1] nvarchar(max)  NOT NULL,
    [Address_Line2] nvarchar(max)  NOT NULL,
    [Address_City] nvarchar(max)  NOT NULL,
    [Address_State] nvarchar(max)  NOT NULL,
    [Address_PostalCode] nvarchar(max)  NOT NULL,
    [Address_Country] nvarchar(max)  NOT NULL,
    [DepartmentID] int  NULL,
    [Salary] float  NOT NULL
);
GO

-- Creating table 'Departments'
CREATE TABLE [dbo].[Departments] (
    [DepartmentID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [ManagerID] int  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [EmployeeID] int  NULL,
    [Role] smallint  NOT NULL
);
GO

-- Creating table 'Attendances'
CREATE TABLE [dbo].[Attendances] (
    [AttendanceID] int IDENTITY(1,1) NOT NULL,
    [Date] datetimeoffset  NOT NULL,
    [CheckInTime] datetimeoffset  NOT NULL,
    [CheckOutTime] datetimeoffset  NOT NULL,
    [Status] smallint  NOT NULL,
    [EmployeeID] int  NULL
);
GO

-- Creating table 'Leaves'
CREATE TABLE [dbo].[Leaves] (
    [LeaveID] int IDENTITY(1,1) NOT NULL,
    [Type] smallint  NOT NULL,
    [StartDate] datetimeoffset  NOT NULL,
    [EndDate] datetimeoffset  NOT NULL,
    [Reason] nvarchar(max)  NOT NULL,
    [Status] smallint  NOT NULL,
    [EmployeeID] int  NULL
);
GO

-- Creating table 'Payrolls'
CREATE TABLE [dbo].[Payrolls] (
    [PayrollID] int IDENTITY(1,1) NOT NULL,
    [Month] smallint  NOT NULL,
    [Year] smallint  NOT NULL,
    [Status] smallint  NOT NULL,
    [EmployeeID] int  NULL,
    [Salary_GrossAmount] float  NULL,
    [Salary_PreTaxDeduction] float  NULL,
    [Salary_TaxDeduction] float  NULL,
    [Salary_PostTaxDeduction] float  NULL,
    [Salary_NetAmount] float  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [EmployeeID] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([EmployeeID] ASC);
GO

-- Creating primary key on [DepartmentID] in table 'Departments'
ALTER TABLE [dbo].[Departments]
ADD CONSTRAINT [PK_Departments]
    PRIMARY KEY CLUSTERED ([DepartmentID] ASC);
GO

-- Creating primary key on [UserID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [AttendanceID] in table 'Attendances'
ALTER TABLE [dbo].[Attendances]
ADD CONSTRAINT [PK_Attendances]
    PRIMARY KEY CLUSTERED ([AttendanceID] ASC);
GO

-- Creating primary key on [LeaveID] in table 'Leaves'
ALTER TABLE [dbo].[Leaves]
ADD CONSTRAINT [PK_Leaves]
    PRIMARY KEY CLUSTERED ([LeaveID] ASC);
GO

-- Creating primary key on [PayrollID] in table 'Payrolls'
ALTER TABLE [dbo].[Payrolls]
ADD CONSTRAINT [PK_Payrolls]
    PRIMARY KEY CLUSTERED ([PayrollID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DepartmentID] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_EmployeeDepartment]
    FOREIGN KEY ([DepartmentID])
    REFERENCES [dbo].[Departments]
        ([DepartmentID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeDepartment'
CREATE INDEX [IX_FK_EmployeeDepartment]
ON [dbo].[Employees]
    ([DepartmentID]);
GO

-- Creating foreign key on [ManagerID] in table 'Departments'
ALTER TABLE [dbo].[Departments]
ADD CONSTRAINT [FK_DepartmentManager]
    FOREIGN KEY ([ManagerID])
    REFERENCES [dbo].[Employees]
        ([EmployeeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentManager'
CREATE INDEX [IX_FK_DepartmentManager]
ON [dbo].[Departments]
    ([ManagerID]);
GO

-- Creating foreign key on [EmployeeID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserEmployee]
    FOREIGN KEY ([EmployeeID])
    REFERENCES [dbo].[Employees]
        ([EmployeeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserEmployee'
CREATE INDEX [IX_FK_UserEmployee]
ON [dbo].[Users]
    ([EmployeeID]);
GO

-- Creating foreign key on [EmployeeID] in table 'Attendances'
ALTER TABLE [dbo].[Attendances]
ADD CONSTRAINT [FK_EmployeeAttendance]
    FOREIGN KEY ([EmployeeID])
    REFERENCES [dbo].[Employees]
        ([EmployeeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeAttendance'
CREATE INDEX [IX_FK_EmployeeAttendance]
ON [dbo].[Attendances]
    ([EmployeeID]);
GO

-- Creating foreign key on [EmployeeID] in table 'Leaves'
ALTER TABLE [dbo].[Leaves]
ADD CONSTRAINT [FK_EmployeeLeave]
    FOREIGN KEY ([EmployeeID])
    REFERENCES [dbo].[Employees]
        ([EmployeeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeLeave'
CREATE INDEX [IX_FK_EmployeeLeave]
ON [dbo].[Leaves]
    ([EmployeeID]);
GO

-- Creating foreign key on [EmployeeID] in table 'Payrolls'
ALTER TABLE [dbo].[Payrolls]
ADD CONSTRAINT [FK_EmployeePayroll]
    FOREIGN KEY ([EmployeeID])
    REFERENCES [dbo].[Employees]
        ([EmployeeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeePayroll'
CREATE INDEX [IX_FK_EmployeePayroll]
ON [dbo].[Payrolls]
    ([EmployeeID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------