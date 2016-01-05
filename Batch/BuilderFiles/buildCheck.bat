@echo off
if "%startFlag%"=="" goto :eof

echo Checking build...

for /F "tokens=*" %%i in (%binFiles%) do (
	if not exist "%buildPath%\%%i" (
		set missingFile=%%i
		goto :error
	)
)

echo Checking succeeded.
echo.
goto :eof

:error
echo Checking failed. Missing file: %missingFile%.
set errorCheck=true
echo.
