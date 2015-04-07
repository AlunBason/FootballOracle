set names 'UTF8';
set @lowDate := '1850-01-01 00:00:00';
set @highDate := '2999-12-31 23:59:59';

use pfmembership;
select @adminId := Id FROM aspnetusers where Email = 'administrator@footballoracle.co.uk';

use pfmain;
select @countryId := HeaderKey FROM countryv where CountryName = 'Wales';

-- Cardiff City
set @key2 := UUID();
insert into venue values (@key2);
insert into venuev values (UUID(), @key2, 'Cardiff City Stadium', 28016, null, null, null, null, null, @countryId, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());
set @key1 := UUID();
insert into team values (@key1);
insert into teamv values (UUID(), @key1, 'Cardiff City', @key2, @countryId, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupteam values (UUID(), 1, @key1, 264);
insert into lookupteam values (UUID(), 2, @key1, 485);

set @key1 := UUID();
insert into team values (@key1);
insert into teamv values (UUID(), @key1, 'Colwyn Bay', null, @countryId, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

set @key1 := UUID();
insert into team values (@key1);
insert into teamv values (UUID(), @key1, 'Merthyr Town', null, @countryId, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- Newport County
set @key2 := UUID();
insert into venue values (@key2);
insert into venuev values (UUID(), @key2, 'Rodney Parade', 7012, null, null, null, null, null, @countryId, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());
set @key1 := UUID();
insert into team values (@key1);
insert into teamv values (UUID(), @key1, 'Newport County', @key2, @countryId, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupteam values (UUID(), 1, @key1, 323);
insert into lookupteam values (UUID(), 2, @key1, 1824);

-- Swansea City
set @key2 := UUID();
insert into venue values (@key2);
insert into venuev values (UUID(), @key2, 'Liberty Stadium', 20750, null, null, null, null, null, @countryId, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());
set @key1 := UUID();
insert into team values (@key1);
insert into teamv values (UUID(), @key1, 'Swansea City', @key2, @countryId, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupteam values (UUID(), 1, @key1, 357);
insert into lookupteam values (UUID(), 2, @key1, 2513);

-- Wrexham
set @key2 := UUID();
insert into venue values (@key2);
insert into venuev values (UUID(), @key2, 'Racecourse Ground', 15500, null, null, null, null, null, @countryId, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());
set @key1 := UUID();
insert into team values (@key1);
insert into teamv values (UUID(), @key1, 'Wrexham', @key2, @countryId, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupteam values (UUID(), 2, @key1, 2859);