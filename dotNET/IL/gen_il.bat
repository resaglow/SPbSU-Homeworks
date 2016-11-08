@echo off

csc sum.cs
ildasm.exe sum.exe /out=sum.il
del sum.exe