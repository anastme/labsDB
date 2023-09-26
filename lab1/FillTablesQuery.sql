use PaymentOfUtilities;

declare @manyToManyCount int, @oneToManyCount int, @iterator int;
set @manyToManyCount = 1000;
set @oneToManyCount = 50;
set @iterator = 1;

delete PaymentTypes;
delete Counters;
delete Indications;
delete Apartments;
delete Rates;
--���������� ����� ��������
insert PaymentTypes values (N'����'), (N'���'), (N'�������������')
--���������� ���������
--���������� ������� � �������
declare @typeId int;
while @iterator < @oneToManyCount
	begin
		set @typeId = (select top 1 Id from PaymentTypes order by NEWID())
		insert Counters values(@typeId);
		exec AddRandomApartment;
		exec AddRandomRate;
		set @iterator = @iterator + 1;
	end;
-- ���������� ���������
set @iterator = 0;
while @iterator < @manyToManyCount
	begin
		exec AddRandomIndication;
		set @iterator = @iterator + 1;
	end;
