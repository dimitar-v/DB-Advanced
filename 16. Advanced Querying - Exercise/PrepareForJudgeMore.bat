@echo off
Set Winrar=%ProgramFiles%\WinRAR\WinRAR.exe
//Set MyFolder= /F
Set ArchiveName=ReadyForJudge2
"%Winrar%" a -r -afzip -dh "%ArchiveName%" "%MyFolder%" -x*\bin -x*\obj -x*\.vs -x*.docx -x*.bat -x*.zip -x*.sln

pause

//powershell.exe zip -r asdf.zip -x */\* *.bat bin/\* obj/\* .vs/\*
//pause

//Set Winrar=%ProgramFiles%\WinRAR\WinRAR.exe
//Set ArchiveName=ReadyForJudge2
//FOR /D %%i IN ("*") DO "%Winrar%" a -r -afzip -dh "%ArchiveName%" "%%~i\"
