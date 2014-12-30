@echo off
if "%startFlag%"=="" goto :eof

set pathMSBuild=C:\Windows\Microsoft.NET\Framework\v4.0.30319
set pathBlat="C:\Program Files (x86)\blat323\full"
set NUnitPath="C:\Program Files (x86)\NUnit 2.6.4\bin"
set PATH=%PATH%;%pathMSBuild%;%pathBlat%;%NUnitPath%

set repoName=geometry
set slnName=EllipseDotIntersect

set gitURL=http://github.com/resaglow/%repoName%

set buildPath=%repoName%\%slnName%\bin\Debug
set testsBuildPath=%repoName%\EllipseDotIntersectTests\bin\Debug
set slnPath=%repoName%\%slnName%.sln

set binFiles=%resDir%\buildCheckBinaries.txt
set missingFile=

set logClone=logClone.log
set logBuild=logBuild.log
set logSend=logSend.log
set logTests=logTests.log

set errorClone=false
set errorBuild=false
set errorCheck=false
set errorSend=false
set errorTests=false

set emailList=%resDir%\emailList.txt
set emailSubject=%slnName% build info
set emailBody=OK.
set emailFiles=emailFiles.txt
