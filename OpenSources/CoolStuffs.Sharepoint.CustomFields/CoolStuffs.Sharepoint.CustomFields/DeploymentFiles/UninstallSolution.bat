set SPAdminTool=%CommonProgramFiles%\Microsoft Shared\web server extensions\12\BIN\stsadm.exe

@ECHO OFF
echo.
echo. Adil Baig's Tech Blog
echo. http://baigadil.blogspot.com
echo. baig.adil@gmail.com
echo.
echo. --- RETRACT and DELETE solution ---
"%SPAdminTool%" -o retractsolution -name CoolStuffs.Sharepoint.CustomFields.wsp -immediate -url %1
"%SPAdminTool%" -o execadmsvcjobs
"%SPAdminTool%" -o deletesolution -name CoolStuffs.Sharepoint.CustomFields.wsp -override

echo Doing an iisreset...
pause
popd
iisreset
