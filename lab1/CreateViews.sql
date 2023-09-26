use PaymentOfUtilities;
go
create view IndicationByPaymentType(Date, Volume, Name)
as
select Indications.Date, Indications.Volume, PaymentTypes.Name from Indications 
inner join Counters on Indications.CounterId = Counters.Id 
inner join PaymentTypes on Counters.PaymentTypeId = PaymentTypes.Id group by Indications.Date, Indications.Volume, PaymentTypes.Name; 

go
create view VolumeIndicationsByApartment(ApartmentId, Volume)
as
select Apartments.Id, Sum(Indications.Volume) from Apartments
inner join  Indications on Apartments.Id = Indications.ApartmentId group by Apartments.Id;
		