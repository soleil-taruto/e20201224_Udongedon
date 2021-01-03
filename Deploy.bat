CALL qq
cx **

rem	CALL DebugRelease.bat /B
rem	CALL Release.bat /B
	CALL Release.bat /V 003

Tools\UpdatingCopy.exe out C:\be\Web\DocRoot\Elsa\d20201224_Udongedon
Tools\RunOnBatch.exe C:\be\Web Deploy.bat
