@echo off
if "%startFlag%"=="" goto :eof

echo Cleaning up...

echo Removing repo...
if exist %repoName% (
	rd /s /q %repoName%
	echo Remove completed.
) else echo Old repo not found.

echo Cleaning temp files...
if "%errorClone%"=="false" if exist %logClone% del %logClone%
if "%errorBuild%"=="false" if exist %logBuild% del %logBuild%
if "%errorTests%"=="false" if exist %logTests% del %logTests%
if "%errorSend%"=="false" if exist %logSend% (
	del %logSend%
	if exist %emailFiles% del %emailFiles%
)

echo Temp files cleaned. Cleanup done.
echo.
