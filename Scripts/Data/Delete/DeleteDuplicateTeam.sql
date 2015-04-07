-- Enter team name in this query to get HeaderKey. 

select * from pfmain.teamv tv where tv.TeamName = 'St Mirren';

-- Paste HeaderKey here to remove all matches and teams

set @TeamKey := 'd8b6f514-b5d7-4f0c-ae57-40916e572bb3';

delete from pfmain.matchv where Team1Guid = @TeamKey or Team2Guid = @TeamKey;

delete from `match` where PrimaryKey not in (select HeaderKey from pfmain.matchv);

delete from teamv where HeaderKey = @TeamKey;

delete from team where PrimaryKey = @TeamKey;

