cd bin\debug
set ProgramFilesx86=%ProgramFiles%
if not ("%PROGRAMFILES(x86)%") == ("")  set ProgramFilesx86=%ProgramFiles(x86)%
call "!ProgramFilesx86!\Microsoft Visual Studio 12.0\Common7\Tools\vsvars32.bat"


SvnError.exe -path "c:\projetos\riskphoenix_mcs_3C" -minrev 25481 -maxrev 25486 -build "C:\windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" -buildargs "src\RiskControl.sln /p:Configuration=Release /v:q /m:5" -test "powershell" -testargs "ExecutaTesteFiltro.ps1"

pause
