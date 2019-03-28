Set Winrar=%ProgramFiles%\WinRAR\WinRAR.exe
//Set MyFolder= /F
Set ArchiveName=ReadyForJudge
"%Winrar%" a -r -afzip -dh -u "%ArchiveName%" "%MyFolder%" -x*\bin -x*\obj -x*\.vs -x*.docx -x*.bat -x*.zip -x*.sln