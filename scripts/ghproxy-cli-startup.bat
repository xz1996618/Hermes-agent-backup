@echo off
REM 等待 E 盘就绪
:wait
if exist E:\NUL goto run
timeout /t 5 /nobreak >nul
goto wait

:run
start /b "" "E:\13_WorkSpaceForHermes\02 for some project\ghproxy-cli\bin\Release\net8.0\win-x64\ghproxy-cli.exe" start
