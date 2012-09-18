@echo off
:Introduce
	echo ** Introduce:
	echo 	That is batch file to backup N days the web application
	echo 	in SharePoint or restore from a backup file.
	echo 	Please edit the configurations before run batch file
	echo ** Excute:	
	echo - Show help:
	echo 		BackupSite.bat -help
	echo - Backup site which in config:
	echo 		BackupSite.bat
	echo - Restore site which in config use your backup file: 
	echo 		BackupSite.bat -restore "path_of_file_backup"
	if "%~1" == "-help" goto EndBackup

:UserConfig
	@set SITE_URL_BACKUP=http://tringuyen
	@set FILE_NAME_REFIX=tringuyen_80_
	@set DEL_AFTER_N_DAYS=10
	@set FOLDER_BACKUP=D:\SharePointBackup
	echo Batch back up SharePoint sites
	echo ************
	@set STSADM="c:\program files\common files\microsoft shared\web server extensions\12\bin\stsadm"
	@cd c:\Program Files\Common Files\Microsoft Shared\web server extensions\12\BIN

:CheckConfigAndArgs
	echo Check your config
	if not exist "%FOLDER_BACKUP%" (
		echo ****ERROR***: FOLDER_BACKUP not exist
		goto EndBackup
	)
	@set CALL_RESTORE=%~1
	if "%CALL_RESTORE%" == "-restore" (
		goto CallRestore 
	) else (
		goto CallBackup
	)
:CallBackup
	echo Call backup.
	echo Please wait in some minutes...
	@set FILE_BACKUP=%FOLDER_BACKUP%\%FILE_NAME_REFIX%%DATE:/=_%.bak
	if exist "%FILE_BACKUP%" (
		echo Delete exist file "%FILE_BACKUP%"
		del "%FILE_BACKUP%"
	)
	%STSADM% -o backup -url "%SITE_URL_BACKUP%" -filename "%FILE_BACKUP%"
	echo Delete old file after %DEL_AFTER_N_DAYS% days
	FORFILES /P "%FOLDER_BACKUP%" /M *.bak /C "cmd /c Del @path" /D -%DEL_AFTER_N_DAYS%
	goto EndBackup

:CallRestore
	echo Call restore.
	echo Please wait in some minutes...
	@set FILE_RESTORE=%~2
	%STSADM% -o restore -url "%SITE_URL_BACKUP%" -filename "%FILE_RESTORE%" -overwrite
	goto EndBackup
	
:EndBackup
	echo Done!