set SPAdminTool=%CommonProgramFiles%\Microsoft Shared\web server extensions\12\BIN\stsadm.exe
set MakeCabTool=Assemblies\makecab.exe
@ECHO OFF
echo.
echo. Adil Baig's Tech Blog
echo. http://baigadil.blogspot.com
echo. baig.adil@gmail.com
echo.
echo --- Making the Required CABINET file ---
"%MakeCabTool%" -f wsp_structure.DDF

echo --- INSTALLING the Cool Stuffs : Custom Field Controls Solution... ---
"%SPAdminTool%" -o addsolution -filename Package\CoolStuffs.Sharepoint.CustomFields.wsp
"%SPAdminTool%" -o deploysolution -immediate -allowGacDeployment -name CoolStuffs.Sharepoint.CustomFields.wsp -url %1
"%SPAdminTool%" -o execadmsvcjobs

pause

echo Doing an iisreset...

popd
iisreset
