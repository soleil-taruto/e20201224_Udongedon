C:\Factory\Tools\RDMD.exe /RC out
C:\Factory\Tools\RDMD.exe /RM tmp

MD out\Data
MD out\Data\Elsa
MD out\Data\res

ROBOCOPY dat out\Data\Elsa /MIR
ROBOCOPY res out\Data\res /MIR

CALL Confuse.bat
COPY /B Elsa20200001\Elsa20200001\bin\Release\Elsa20200001.exe out\Game.exe
COPY /B Elsa20200001\Elsa20200001\bin\Release\DxLib.dll out
COPY /B Elsa20200001\Elsa20200001\bin\Release\DxLib_x64.dll out
COPY /B Elsa20200001\Elsa20200001\bin\Release\DxLibDotNet.dll out

C:\Factory\Tools\xcp.exe doc out
COPY /B AUTHORS out
COPY /B LICENSE out

C:\Factory\SubTools\zip.exe /PE- /RVE- %* /G out Udongedon
