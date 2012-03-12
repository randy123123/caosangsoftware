
@SET STSADM="c:\program files\common files\microsoft shared\web server extensions\14\bin\STSADM"
@SET path=D:\Research\sppex\CodeArt.SharePoint.PermissionEx\Deploy\
@SET path=

%STSADM% -o addsolution -filename %path%CodeArt_PermissionEx.wsp

%STSADM% -o execadmsvcjobs

pause
 