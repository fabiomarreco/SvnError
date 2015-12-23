cd /d %~dp0
cd bin\debug
rem set ProgramFilesx86=%ProgramFiles%
rem if not ("%PROGRAMFILES(x86)%") == ("")  set ProgramFilesx86=%ProgramFiles(x86)%
call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\vsvars32.bat"


rem SvnError.exe -path "c:\projetos\riskphoenixSVN" -minrev 26411 -maxrev 26985 -build "C:\windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" -buildargs "src\RiskControl.sln /p:Configuration=Debug /v:q /m:5" -test "powershell" -testargs "ExecutaTesteFiltro.ps1"

SvnError.exe -path "c:\projetos\riskphoenixSVN" -minrev 29203 -maxrev 29220 -build "_build.bat" -test "_teste.bat"

pause
