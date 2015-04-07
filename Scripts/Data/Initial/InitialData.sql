set names 'UTF8';
set @lowDate := '1850-01-01 00:00:00';
set @highDate := '2999-12-31 23:59:59';

use pfmembership;
select @adminId := Id FROM aspnetusers where Email = 'administrator@footballoracle.co.uk';

use pfmain;
-- "FIFA", "International Federation of Association Football"
set @key1 := UUID();
insert into organisation values (@key1);
insert into organisationv values (UUID(), @key1, 'FIFA', 'International Federation of Association Football', null, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "AFC", "Asian Football Confederation"
set @key2 := UUID();
insert into organisation values (@key2);
insert into organisationv values (UUID(), @key2, 'AFC', 'Asian Football Confederation', @key1, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "AFC(CSAFF)", "Central and South Asia Football Federation"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'AFC(CSAFF)', 'Central and South Asia Football Federation', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Afghanistan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Afghanistan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Bangladesh"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Bangladesh', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Bhutan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Bhutan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "India"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'India', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Kyrgyzstan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Kyrgyzstan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Maldives"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Maldives', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Nepal"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Nepal', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Pakistan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Pakistan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Sri Lanka"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Ceylon', @key3, null, null, @lowDate, '1972-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Sri Lanka', @key3, null, null, '1972-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Tajikistan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Tajikistan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Turkmenistan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Turkmenistan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Uzbekistan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Uzbekistan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "AFC(EAFF)", "East Asia Football Federation"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'AFC(EAFF)', 'East Asia Football Federation', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "China"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'China', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Guam"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Guam', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Hong Kong"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Hong Kong', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Japan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Japan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "North Korea"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'North Korea', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "South Korea"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'South Korea', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Macau"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Macau', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Mongolia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Mongolia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Northern Mariana Islands"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Northern Mariana Islands', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Palau"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Palau', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Chinese Taipei"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Chinese Taipei', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "AFC(SEAFF)", "South East Asia Football Federation"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'AFC(SEAFF)', 'South East Asia Football Federation', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Australia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Australia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Brunei"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Brunei', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Cambodia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Cambodia', @key3, null, null, @lowDate, '1970-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Khmer Republic', @key3, null, null, '1970-06-09 00:00:00', '1975-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Kampuchea', @key3, null, null, '1975-06-09 00:00:00', '1979-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Cambodia', @key3, null, null, '1979-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Indonesia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Dutch East Indies', @key3, null, null, @lowDate, '1945-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Indonesia', @key3, null, null, '1945-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Laos"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Laos', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Malaysia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Malaya', @key3, null, null, @lowDate, '1963-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Malaysia', @key3, null, null, '1963-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Myanmar"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Burma', @key3, null, null, @lowDate, '1989-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Myanmar', @key3, null, null, '1989-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Philippines"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Philippines', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Singapore"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Singapore', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Thailand"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Thailand', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "East Timor"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'East Timor', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "South Vietnam"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'South Vietnam', @key3, null, null, '1949-06-09 00:00:00', '1975-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Vietnam"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'North Vietnam', @key3, null, null, '1949-06-09 00:00:00', '1975-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Vietnam', @key3, null, null, '1975-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "AFC(WAFF)", "West Asian Football Federation"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'AFC(WAFF)', 'West Asian Football Federation', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Bahrain"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Bahrain', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Iraq"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Iraq', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Jordan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Jordan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Kuwait"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Kuwait', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Lebanon"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Lebanon', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Oman"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Oman', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Palestine"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Palestine', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Qatar"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Qatar', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Saudi Arabia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Saudi Arabia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "South Yemen"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'South Yemen', @key3, null, null, @lowDate, '1990-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Syria"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Syria', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "United Arab Emirates"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'United Arab Emirates', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Yemen"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'North Yemen', @key3, null, null, @lowDate, '1990-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Yemen', @key3, null, null, '1990-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "CAF", "Confederation of African Football"
set @key2 := UUID();
insert into organisation values (@key2);
insert into organisationv values (UUID(), @key2, 'CAF', 'Confederation of African Football', @key1, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "UNAF", "Union of North African Football Federations"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'UNAF', 'Union of North African Football Federations', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Algeria"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'FLN Team', @key3, null, null, @lowDate, '1962-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Algeria', @key3, null, null, '1962-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Egypt"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'United Arab Republic', @key3, null, null, @lowDate, '1971-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Egypt', @key3, null, null, '1971-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Libya"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Libya', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Morocco"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Morocco', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Tunisia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Tunisia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "WAFU", "West Africa Football Union"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'WAFU', 'West Africa Football Union', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Benin"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Dahomey', @key3, null, null, @lowDate, '1975-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Benin', @key3, null, null, '1975-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Burkina Faso"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Upper Volta', @key3, null, null, @lowDate, '1984-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Burkina Faso', @key3, null, null, '1984-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Cape Verde"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Cape Verde', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Gambia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'British Gambia', @key3, null, null, @lowDate, '1965-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Gambia', @key3, null, null, '1965-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Ghana"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Gold Coast', @key3, null, null, @lowDate, '1957-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Ghana', @key3, null, null, '1957-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Guinea"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Guinea', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Guinea-Bissau"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Portuguese Guinea', @key3, null, null, @lowDate, '1975-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Guinea-Bissau', @key3, null, null, '1975-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Ivory Coast"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Ivory Coast', @key3, null, null, @lowDate, '1983-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Côte d''Ivoire', @key3, null, null, '1983-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Liberia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Liberia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Mali"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Mali', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Mauritania"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Mauritania', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Niger"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Niger', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Nigeria"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Nigeria', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Senegal"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Senegal', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Sierra Leone"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Sierra Leone', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Togo"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'French Togoland', @key3, null, null, @lowDate, '1960-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Togo', @key3, null, null, '1960-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "UNIFFAC", "Central African Football Federations' Union"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'UNIFFAC', 'Central African Football Federations'' Union', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Cameroon"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Cameroon', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Central African Republic"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Central African Republic', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Chad"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Chad', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Congo"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Middle Congo', @key3, null, null, @lowDate, '1960-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Congo-Brazzaville', @key3, null, null, '1960-06-09 00:00:00', '1992-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Congo', @key3, null, null, '1992-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "DR Congo"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Belgian Congo', @key3, null, null, @lowDate, '1960-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Congo-Léopoldville', @key3, null, null, '1960-06-09 00:00:00', '1963-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Congo-Kinshasa', @key3, null, null, '1963-06-09 00:00:00', '1971-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Zaire', @key3, null, null, '1971-06-09 00:00:00', '1997-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'DR Congo', @key3, null, null, '1997-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Equatorial Guinea"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Equatorial Guinea', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Gabon"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Gabon', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "São Tomé and Príncipe"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'São Tomé and Príncipe', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "CECAFA", "Council for East and Central Africa Football Associations"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'CECAFA', 'Council for East and Central Africa Football Associations', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Burundi"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Burundi', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Djibouti"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'French Somaliland', @key3, null, null, @lowDate, '1977-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Djibouti', @key3, null, null, '1977-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Ethiopia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Ethiopia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Eritrea"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Eritrea', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Kenya"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Kenya', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Rwanda"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Rwanda', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Somalia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Somalia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "South Sudan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'South Sudan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Sudan"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Sudan', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Tanzania"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Tanganyika', @key3, null, null, @lowDate, '1963-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Tanzania', @key3, null, null, '1963-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Uganda"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Uganda', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Zanzibar"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Zanzibar', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "COSAFA", "Council of Southern Africa Football Associations"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'COSAFA', 'Council of Southern Africa Football Associations', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Angola"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Angola', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Botswana"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Botswana', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Comoros"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Comoros', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Lesotho"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Lesotho', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Mauritius"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Mauritius', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Madagascar"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Madagascar', @key3, null, null, @lowDate, '1958-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Malagasy Republic', @key3, null, null, '1958-06-09 00:00:00', '1975-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Madagascar', @key3, null, null, '1975-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Malawi"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Nyasaland', @key3, null, null, @lowDate, '1966-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Malawi', @key3, null, null, '1966-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Mozambique"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Mozambique', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Namibia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Namibia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Seychelles"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Seychelles', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "South Africa"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'South Africa', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Swaziland"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Swaziland', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Zambia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Northern Rhodesia', @key3, null, null, @lowDate, '1964-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Zambia', @key3, null, null, '1964-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Zimbabwe"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Southern Rhodesia', @key3, null, null, @lowDate, '1964-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Rhodesia', @key3, null, null, '1964-06-09 00:00:00', '1980-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Zimbabwe', @key3, null, null, '1980-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "CONCACAF", "Confederation of North, Central American and Caribbean Association Football"
set @key2 := UUID();
insert into organisation values (@key2);
insert into organisationv values (UUID(), @key2, 'CONCACAF', 'Confederation of North, Central American and Caribbean Association Football', @key1, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "NAF", "North American Zone"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'NAF', 'North American Zone', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Canada"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Canada', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Mexico"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Mexico', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "United States"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'United States', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "UNCAF", "Central American Zone"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'UNCAF', 'Central American Zone', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Belize"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Belize', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Costa Rica"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Costa Rica', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "El Salvador"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'El Salvador', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Guatemala"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Guatemala', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Honduras"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Honduras', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Nicaragua"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Nicaragua', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Panama"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Panama', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "CFU", "Caribbean Zone"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'CFU', 'Caribbean Zone', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Anguilla"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Anguilla', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Antigua and Barbuda"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Antigua and Barbuda', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Aruba"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Aruba', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Bahamas"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Bahamas', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Barbados"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Barbados', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Bermuda"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Bermuda', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Bonaire"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Bonaire', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "British Virgin Islands"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'British Virgin Islands', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Cayman Islands"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Cayman Islands', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Cuba"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Cuba', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Curaçao"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Netherlands Antilles', @key3, null, null, @lowDate, '1991-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Curaçao', @key3, null, null, '1991-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Dominica"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Dominica', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Dominican Republic"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Dominican Republic', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "French Guiana"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'French Guiana', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Grenada"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Grenada', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Guadeloupe"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Guadeloupe', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Guyana"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'British Guiana', @key3, null, null, @lowDate, '1966-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Guyana', @key3, null, null, '1966-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Haiti"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Haiti', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Jamaica"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Jamaica', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Martinique"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Martinique', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Montserrat"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Montserrat', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Puerto Rico"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Puerto Rico', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Saint Kitts and Nevis"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Saint Kitts and Nevis', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Saint Lucia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Saint Lucia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Saint Martin"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Saint Martin', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Saint Vincent and the Grenadines"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Saint Vincent and the Grenadines', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Sint Maarten"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Sint Maarten', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Suriname"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Surinam', @key3, null, null, @lowDate, '1954-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Suriname', @key3, null, null, '1954-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Trinidad and Tobago"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Trinidad and Tobago', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Turks and Caicos Islands"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Turks and Caicos Islands', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "U.S. Virgin Islands"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'U.S. Virgin Islands', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "CONMEBOL", "Confederación Sudamericana de Fútbol"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'CONMEBOL', 'Confederación Sudamericana de Fútbol', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Argentina"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Argentina', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Bolivia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Bolivia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Brazil"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Brazil', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Chile"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Chile', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Colombia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Colombia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Ecuador"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Ecuador', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Paraguay"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Paraguay', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Peru"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Peru', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Uruguay"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Uruguay', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Venezuela"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Venezuela', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "OFC", "Oceania Football Confederation"
set @key3 := UUID();
insert into organisation values (@key3);
insert into organisationv values (UUID(), @key3, 'OFC', 'Oceania Football Confederation', @key2, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "American Samoa"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'American Samoa', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Cook Islands"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Cook Islands', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Fiji"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Fiji', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Kiribati"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Kiribati', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "New Caledonia"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'New Caledonia', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "New Zealand"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'New Zealand', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Niue"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Niue', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Papua New Guinea"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Papua New Guinea', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Samoa"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Western Samoa', @key3, null, null, @lowDate, '1996-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Samoa', @key3, null, null, '1996-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Solomon Islands"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Solomon Islands', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Tahiti (French Polynesia)"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Tahiti (French Polynesia)', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Tonga"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Tonga', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Tuvalu"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'Tuvalu', @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Vanuatu"
set @key4 := UUID();
insert into country values (@key4);
insert into countryv values (UUID(), @key4, 'New Hebrides', @key3, null, null, @lowDate, '1980-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key4, 'Vanuatu', @key3, null, null, '1980-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "UEFA", "Union of European Football Associations"
set @key2 := UUID();
insert into organisation values (@key2);
insert into organisationv values (UUID(), @key2, 'UEFA', 'Union of European Football Associations', @key1, null, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Albania"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Albania', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Andorra"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Andorra', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Armenia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Armenia', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Austria"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Austria', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Azerbaijan"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Azerbaijan', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Belarus"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Belarus', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Belgium"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Belgium', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Bosnia and Herzegovina"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Bosnia and Herzegovina', @key2, null, null, '1992-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Bulgaria"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Bulgaria', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Croatia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Croatia', @key2, null, null, '1993-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Cyprus"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Cyprus', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Czech Republic"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Czechoslovakia', @key2, null, null, @lowDate, '1993-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Czech Republic', @key2, null, null, '1993-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Denmark"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Denmark', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "East Germany"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'East Germany', @key2, null, null, '1952-06-09 00:00:00', '1990-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "England"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'England', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Premier League", ""
set @key4 := UUID();
insert into organisation values (@key4);
insert into organisationv values (UUID(), @key4, 'Premier League', '', null, @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Premier League", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Premier League', @key4, 1, 3, null, null, '1992-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupcompetition values(UUID(), 1, @key5, '3');
insert into lookupcompetition values(UUID(), 2, @key5, '1');

-- "The Football League", ""
set @key4 := UUID();
insert into organisation values (@key4);
insert into organisationv values (UUID(), @key4, 'The Football League', '', null, @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Football League", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League', @key4, 1, 3, null, null, '1888-06-09 00:00:00', '1892-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Football League First Division", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League First Division', @key4, 1, 3, null, null, '1892-06-09 00:00:00', '1992-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Football League Second Division", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League Second Division', @key4, 1, 3, null, null, '1892-06-09 00:00:00', '1992-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Football League First Division", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League First Division', @key4, 1, 3, null, null, '1992-06-09 00:00:00', '2004-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Football League Championship", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League Championship', @key4, 1, 3, null, null, '2004-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupcompetition values(UUID(), 1, @key5, '9');
insert into lookupcompetition values(UUID(), 2, @key5, '2');

-- "Football League Third Division", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League Third Division', @key4, 1, 3, null, null, '1920-06-09 00:00:00', '1921-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Football League Third Division(North)", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League Third Division(North)', @key4, 1, 3, null, null, '1921-06-09 00:00:00', '1958-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Football League Third Division(South)", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League Third Division(South)', @key4, 1, 3, null, null, '1921-06-09 00:00:00', '1958-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Football League Third Division", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League Third Division', @key4, 1, 3, null, null, '1958-06-09 00:00:00', '1992-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Football League Second Division", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League Second Division', @key4, 1, 3, null, null, '1992-06-09 00:00:00', '2004-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "League One", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'League One', @key4, 1, 3, null, null, '2004-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupcompetition values(UUID(), 1, @key5, '11');
insert into lookupcompetition values(UUID(), 2, @key5, '3');

-- "Football League Fourth Division", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League Fourth Division', @key4, 1, 3, null, null, '1958-06-09 00:00:00', '1992-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Football League Third Division", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football League Third Division', @key4, 1, 3, null, null, '1992-06-09 00:00:00', '2004-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "League Two", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'League Two', @key4, 1, 3, null, null, '2004-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupcompetition values(UUID(), 1, @key5, '12');
insert into lookupcompetition values(UUID(), 2, @key5, '4');

-- "The Football Conference", ""
set @key4 := UUID();
insert into organisation values (@key4);
insert into organisationv values (UUID(), @key4, 'The Football Conference', '', null, @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Alliance Premier League", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Alliance Premier League', @key4, 1, 3, null, null, '1979-06-09 00:00:00', '1987-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Football Conference", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Football Conference', @key4, 1, 3, null, null, '1987-06-09 00:00:00', '2004-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Conference National", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Conference National', @key4, 1, 3, null, null, '2004-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupcompetition values(UUID(), 2, @key5, '9');

-- "Estonia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Estonia', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Faroe Islands"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Faroe Islands', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Finland"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Finland', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Finnish Football Association"
set @key4 := UUID();
insert into organisation values (@key4);
insert into organisationv values (UUID(), @key4, 'Finnish Football Association', '', null, @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Veikkausliiga"
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Veikkausliiga', @key4, 1, 3, null, null, '1930-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupcompetition values(UUID(), 2, @key5, '208');

-- "France"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'France', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Georgia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Georgia', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Germany"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Germany', @key2, null, null, @lowDate, '1949-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'West Germany', @key2, null, null, '1949-06-09 00:00:00', '1990-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Germany', @key2, null, null, '1990-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Gibraltar"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Gibraltar', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Greece"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Greece', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Hungary"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Hungary', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Iceland"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Iceland', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Israel"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Palestine, British Mandate', @key2, null, null, @lowDate, '1948-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Israel', @key2, null, null, '1948-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Italy"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Italy', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Kazakhstan"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Kazakhstan', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Latvia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Latvia', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Liechtenstein"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Liechtenstein', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Lithuania"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Lithuania', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Luxembourg"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Luxembourg', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Macedonia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Macedonia', @key2, null, null, '2009-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Malta"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Malta', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Moldova"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Moldova', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Montenegro"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Montenegro', @key2, null, null, '2006-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Netherlands"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Netherlands', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Northern Ireland"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Ireland', @key2, null, null, @lowDate, '1953-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Northern Ireland', @key2, null, null, '1953-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Norway"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Norway', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Poland"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Poland', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Portugal"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Portugal', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Republic of Ireland"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Irish Free State', @key2, null, null, @lowDate, '1937-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Republic of Ireland', @key2, null, null, '1937-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "FYR Macedonia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'FYR Macedonia', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Romania"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Romania', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Russia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Russia', @key2, null, null, @lowDate, '1923-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Soviet Union', @key2, null, null, '1923-06-09 00:00:00', '1991-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'CIS', @key2, null, null, '1991-06-09 00:00:00', '1992-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Russia', @key2, null, null, '1992-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Saar"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Saar', @key2, null, null, '1950-06-09 00:00:00', '1956-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "San Marino"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'San Marino', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Scotland"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Scotland', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Scottish Football Association", ""
set @key4 := UUID();
insert into organisation values (@key4);
insert into organisationv values (UUID(), @key4, 'Scottish Football Association', '', null, @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Scottish Premiership", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Scottish Premiership', @key4, 1, 3, null, null, '2013-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupcompetition values(UUID(), 1, @key5, '21');

-- "Scottish Premier League", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Scottish Premier League', @key4, 1, 3, null, null, '1998-06-09 00:00:00', '2013-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupcompetition values(UUID(), 1, @key5, '21');

-- "Scottish League Premier Division", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Scottish League Premier Division', @key4, 1, 3, null, null, '1975-06-09 00:00:00', '1998-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Scottish League First Division", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Scottish League First Division', @key4, 1, 3, null, null, '1893-06-09 00:00:00', '1975-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Scottish League", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Scottish League', @key4, 1, 3, null, null, '1890-06-09 00:00:00', '1893-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());

-- "Serbia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Kingdom of Serbs, Croats, and Slovenes ', @key2, null, null, @lowDate, '1929-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Yugoslavia', @key2, null, null, '1929-06-09 00:00:00', '2003-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Federal Republic of Yugoslavia', @key2, null, null, '1992-06-09 00:00:00', '2003-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Serbia & Montenegro', @key2, null, null, '2003-06-09 00:00:00', '2006-06-08 23:59:59', false, true, @adminId, NOW(), @adminId, NOW());
insert into countryv values (UUID(), @key3, 'Serbia', @key2, null, null, '2006-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Slovakia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Slovakia', @key2, null, null, '1993-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Slovenia"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Slovenia', @key2, null, null, '1992-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Spain"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Spain', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Royal Spanish Football Federation", ""
set @key4 := UUID();
insert into organisation values (@key4);
insert into organisationv values (UUID(), @key4, 'Royal Spanish Football Federation', '', null, @key3, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Primera División", ""
set @key5 := UUID();
insert into competition values (@key5);
insert into competitionv values (UUID(), @key5, 'Primera División', @key4, 1, 3, null, null, '2000-06-09 00:00:00', @highDate, false, true, @adminId, NOW(), @adminId, NOW());
insert into lookupcompetition values(UUID(), 1, @key5, '28');

-- "Sweden"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Sweden', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Switzerland"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Switzerland', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Turkey"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Turkey', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Ukraine"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Ukraine', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());

-- "Wales"
set @key3 := UUID();
insert into country values (@key3);
insert into countryv values (UUID(), @key3, 'Wales', @key2, null, null, @lowDate, @highDate, false, true, @adminId, NOW(), @adminId, NOW());