@echo off
:PROMPT
SET /P AREYOUSURE=Are you sure (Y/N)?
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END

set mysqlpath="c:\Program Files (x86)\MySQL\MySQL Workbench CE 6.1.7\mysql"
set scriptpath="Data"

set svr=localhost
set usr=root
set pwd=S3pt1971

rem ***** LIVE VALUES *****
rem set svr=mysql1327.netcetera.co.uk
rem set usr=knmfyixi
rem set pwd=79yjuYr33S
rem ***********************

@echo DeleteMatchesVenuesTeams.sql
%mysqlpath% -h%svr% -u%usr% -p%pwd% --silent < %scriptpath%\Delete\DeleteMatchesVenuesTeams.sql
@echo EnglandTeamData.sql
%mysqlpath% -h%svr% -u%usr% -p%pwd% --silent < %scriptpath%\Team\EnglandTeamData.sql
@echo WalesTeamData.sql
%mysqlpath% -h%svr% -u%usr% -p%pwd% --silent < %scriptpath%\Team\WalesTeamData.sql
rem @echo PersonData_England.sql
rem %mysqlpath% -h%svr% -u%usr% -p%pwd% --silent < %scriptpath%\Person\PersonData_England.sql
rem @echo PersonData_France.sql
rem %mysqlpath% -h%svr% -u%usr% -p%pwd% --silent < %scriptpath%\Person\PersonData_France.sql
rem @echo PersonData_Germany.sql
rem %mysqlpath% -h%svr% -u%usr% -p%pwd% --silent < %scriptpath%\Person\PersonData_Germany.sql
rem @echo PersonData_Spain.sql
rem %mysqlpath% -h%svr% -u%usr% -p%pwd% --silent < %scriptpath%\Person\PersonData_Spain.sql

pause

:END

