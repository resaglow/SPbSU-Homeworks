@echo off
if "%startFlag%"=="" goto :eof

echo Cloning repo...

git clone %gitURL% >nul 2>%logClone%

if errorlevel 1 (
	set errorClone=true
	echo Cloning failed.
) else echo Cloning succeeded.

echo.
