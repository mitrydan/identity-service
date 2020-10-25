:: TODO. Pass list of folders to clear as parameters. Default (Bin, Obj, _Bin).
:: TODO. Add list of folders to skip as parameters. Default (Libs, Packages).
:: TODO. List not processed folders on separate line.

@echo OFF
Setlocal EnableDelayedExpansion
for /f %%a in ('copy /Z "%~dpf0" nul') do set "CR=%%a"

echo Start bin and obj folders removing.

for /d /r . %%d in (bin,obj) do (
	if exist "%%d" (
		set currentDir=%%d
		set leftTest=x!currentDir:\Libs\=!
		set leftTest2=x!currentDir:\packages\=!
		if "!leftTest!"=="x!currentDir!" (
			if "!leftTest2!"=="x!currentDir!" (
			call :spinner
			rd /s /q "%%d"
			)
		)
	)
)

:: Tribute to RRS.
echo Zzzzz  ^|^\      ^_^,^,^,^-^-^,^,^_        +-----------------------------+
echo        ^/^,^`^.^-^'^`^'   ^.^_  ^\^-^;^;^,^_    ^| Process completed.          ^|
echo       ^|^,4^-  ^) ^)^_   ^.^;^.^(  ^`^'^-^'   ^| Process any key to exit.    ^|
echo      ^'^-^-^-^'^'^(^_^/^.^_^)^-^'^(^_^\^_^)        +-----------------------------+

pause > nul
exit /b


:spinner
set /a "spinner=(spinner + 1) %% 4"
set "spinChars=\|/-"
<nul set /p ".=Processing !spinChars:~%spinner%,1!!CR!"
exit /b