
@SET STSADM="c:\program files\common files\microsoft shared\web server extensions\14\bin\STSADM"
@SET path=D:\Research\sppex\CodeArt.SharePoint.PermissionEx\Deploy\
@SET path=

%STSADM% -o upgradesolution -filename %path%CodeArt_PermissionEx.wsp -name CodeArt_PermissionEx.wsp -immediate  -allowGacDeployment  -allowCasPolicies

%STSADM% -o execadmsvcjobs

pause
 