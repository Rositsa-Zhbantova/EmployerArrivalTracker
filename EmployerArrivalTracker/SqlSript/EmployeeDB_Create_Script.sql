CREATE DATABASE EmployeeDB

create table Employee 
(
EmployeeID int identity(1,1) not null primary key, 
FirstName varchar(50), 
LastName varchar(50),
Email varchar(50)
)

create table Arrival 
(
ArrivalID int identity(1,1) not null primary key,
EmployeeID int foreign key references Employee (EmployeeID), 
Time varchar(50)
)

create table #employee (firstName varchar(50), lastName varchar(50), email varchar(50))

bulk insert #employee from 'c:\temp_data\employees.txt'
with (rowterminator = '\n', fieldterminator = '\t')

insert into Employee 
select * from #employee

create table Token
(
TokenID int identity(1,1) not null primary key, 
TokenNumber varchar(40), 
ExpirationTime varchar(30)
)
