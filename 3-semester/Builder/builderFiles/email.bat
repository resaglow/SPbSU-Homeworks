@echo off
if "%startFlag%"=="" goto :eof

echo Sending email...

break>%emailFiles%

:: Reverse order to find out the very first error

if %errorTests%==true (
	set emailSubject=%emailSubject% - Error testing
	set emailBody=An error while testing library.
	echo %logTests%,>>%emailFiles%
)  

if %errorCheck%==true (
	set emailSubject=%emailSubject% - Error checking
	set emailBody=File %missingFile% not found while checking the build.
) 

if %errorBuild%==true (
	set emailSubject=%emailSubject% - Error building
	set emailBody=An error while building.
	echo %logBuild%,>>%emailFiles%
)  

if %errorClone%==true (
	set emailSubject=%emailSubject% - Error cloning
	set emailBody=An error while cloning repository.
	echo %logClone%,>>%emailFiles%
)

blat -tf %emailList% -subject "%emailSubject%" -body "%emailBody%" -atf %emailFiles% 1>%logSend% 2>&1

if errorlevel 1 (
	set errorSend=true
	echo Error sending.
) else (
	echo Sending succeeded.
)

echo.