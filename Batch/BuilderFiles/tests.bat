@echo off
if "%startFlag%"=="" goto :eof

echo Testing library...

%NUnitPath%\nunit-console.exe %testsBuildPath%\EllipseDotIntersectTests.dll>%logTests%

if errorlevel 1 (
	set errorTests=true
	echo Error testing.
) else (
	echo Testing succeeded.
)

echo.
