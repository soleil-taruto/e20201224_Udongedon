C:\Factory\Tools\RDMD.exe /RC out
C:\Factory\Tools\RDMD.exe /RM tmp

ROBOCOPY dat out\Data /MIR

CALL Confuse.bat
COPY /B Elsa20200001\Elsa20200001\bin\Release\Elsa20200001.exe-confused out\Game.exe
COPY /B Elsa20200001\Elsa20200001\bin\Release\DxLib.dll out
COPY /B Elsa20200001\Elsa20200001\bin\Release\DxLib_x64.dll out
COPY /B Elsa20200001\Elsa20200001\bin\Release\DxLibDotNet.dll out

C:\Factory\Tools\xcp.exe doc out
C:\Factory\Elsa\EditConfig.exe out\Config.conf ;[LOG_ENABLED_MARK] 1
COPY /B AUTHORS out

C:\Factory\SubTools\zip.exe /PE- /RVE- %* /G out Udongedon
