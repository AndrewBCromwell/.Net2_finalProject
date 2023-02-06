ECHO off

sqlcmd -S localhost -E -i Circus_Days_db.sql

rem server is localhost

ECHO .
ECHO if no errors appear DB was created
PAUSE