FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"

@echo off
Set Winrar=%ProgramFiles%\WinRAR\WinRAR.exe
//Set MyFolder= /F
Set ArchiveName=ReadyForJudge
//"%Winrar%" a -r -afzip -ep1 -dh "%ArchiveName%" "%MyFolder%"

FOR /D %%i IN ("*") DO "%Winrar%" a -r -afzip -dh "%ArchiveName%" "%%~i\"

//pause