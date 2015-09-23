@echo off
if "%startFlag%"=="" goto :eof

echo Building...

MSBuild %slnPath%>%logBuild%

if errorlevel 1 (
	set errorBuild=true
	echo Building failed.
) else echo Build succeeded.

echo.