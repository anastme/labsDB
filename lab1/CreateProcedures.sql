use PaymentOfUtilities;
go
create procedure GetRandomNumber
	@a int,
	@b int
as
return FLOOR(RAND()*(@b-@a+1))+@a;

go
create procedure AddRandomRate
as
declare @month int, @day int, @priceInt int, @price money;
exec @month = GetRandomNumber 1, 12;
exec @day = GetRandomNumber 1, 28;
exec @priceInt = GetRandomNumber 50, 100;
set @price = cast(@priceInt as money);
insert Rates values('2023-' + str(@month) + '-' + str(@day), @price, (SELECT TOP 1 Id FROM Rates order BY NEWID()))
go
create procedure AddRandomIndication
as
declare @month int, @day int, @volumeInt int, @volume float;
exec @month = GetRandomNumber 1, 12;
exec @day = GetRandomNumber 1, 28;
exec @volumeInt = GetRandomNumber 0, 50;
set @volume = cast(@volumeInt as float);
insert Indications values('2023-' + str(@month) + '-' + str(@day), @volume, (SELECT TOP 1 Id FROM Apartments order BY NEWID()), (SELECT TOP 1 Id FROM Counters order BY NEWID()));

go 
create procedure AddRandomApartment
as 
declare @nameNumber int, @lastNameNumber int, @middleNameNumber int, @humanCount int, @squareInt int, @square float;
exec @nameNumber = GetRandomNumber 0, 20;
exec @lastNameNumber = GetRandomNumber 0, 20;
exec @middleNameNumber = GetRandomNumber 0, 20;
exec @humanCount = GetRandomNumber 1, 6;
exec @squareInt = GetRandomNumber 10, 100;
set @square = cast(@squareInt as float);
insert Apartments values (N'Имя' + str(@nameNumber), N'Фамилия' + str(@lastNameNumber), N'Отчество' +  str(@middleNameNumber), @humanCount, @square)