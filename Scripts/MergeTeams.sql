set @BadTeamVKey := '03f1e0a4-bb78-44f7-87a0-c00a175a3b0b';
set @GoodTeamVKey := 'bbc2b4e7-59f6-4af1-ba13-078e87ba3c64';

select @BadTeamKey := headerkey from pfmain.teamv where PrimaryKey = @BadTeamVKey;
select @GoodTeamKey := headerkey from pfmain.teamv where PrimaryKey = @GoodTeamVKey;

update pfmain.matchv set Team1Guid = @GoodTeamKey where Team1Guid = @BadTeamKey;
update pfmain.matchv set Team2Guid = @GoodTeamKey where Team2Guid = @BadTeamKey;

delete from pfmain.teamname where teamvkey = @BadTeamVKey;
delete from pfmain.teamv where HeaderKey = @BadTeamKey;
delete from pfmain.team where PrimaryKey = @BadTeamKey;
