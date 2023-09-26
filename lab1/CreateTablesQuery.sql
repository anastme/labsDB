use PaymentOfUtilities;
--типы
create table PaymentTypes
(
	Id int primary key identity(0, 1),
	Name nvarchar(20) unique
);
--тарифы
create table Rates
(
	Id int primary key identity(0, 1),
	Date date not null,
	Price money not null,
	PaymentTypeId int not null,
	constraint FK_Rates_To_PaymentTypes foreign key(PaymentTypeId) references PaymentTypes(Id) on delete cascade,
);
--квартиры
create table Apartments
(
	Id int primary key identity(0, 1),
	FirstName nvarchar(20) not null,
	LastName nvarchar(20) not null,
	MiddleName nvarchar(20),
	HumanCount int default 1 check(HumanCount >= 1),
	Square float not null
);
--счетчики
create table Counters
(
	Id int primary key identity(0, 1),
	PaymentTypeId int not null,
	constraint FK_Counters_To_Types foreign key(PaymentTypeId) references PaymentTypes(Id) on delete cascade,

);
--показания
create table Indications
(
	Id int primary key identity(0, 1),
	Date date not null,
	Volume float not null,
	ApartmentId int not null,
	constraint FK_Indications_To_Apartments foreign key(ApartmentId) references Apartments(Id) on delete cascade,
	CounterId int not null,
	constraint FK_Indications_To_Counters foreign key(CounterId) references Counters(Id) on delete cascade,
);