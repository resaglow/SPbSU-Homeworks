@echo off

call gen_il.bat 
powershell -Command "(gc sum.il) -replace '2B', '2D' | Out-File sub.il" 
powershell -Command "(gc sub.il) -replace 'add', 'sub' | Out-File sub.il" 
ilasm sub.il 
sub.exe
del sub.exe