COPY /B Elsa20200001\Elsa20200001\bin\Release\Elsa20200001.exe-confused-rename-table.txt.gz tmp\rt.tmp
C:\Factory\Tools\Encryptor.exe /KB C:\Dev\Common.keybundle tmp\rt.tmp
COPY /B tmp\rt.tmp.enc out\rt.dat
