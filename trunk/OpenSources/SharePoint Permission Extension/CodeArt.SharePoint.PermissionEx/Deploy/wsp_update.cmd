
@SET STSADM="c:\program files\common files\microsoft shared\web server extensions\12\bin\STSADM" 

%STSADM% -o upgradesolution -filename CodeArt_PermissionEx.wsp -name CodeArt_PermissionEx.wsp -immediate  -allowGacDeployment  -allowCasPolicies

%STSADM% -o execadmsvcjobs

pause
 