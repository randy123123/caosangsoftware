@SET STSADM="c:\program files\common files\microsoft shared\web server extensions\14\bin\stsadm"

net stop "SharePoint 2010 Administration"

%STSADM% -o retractsolution -name EFSPWFActivities.deleteItemsByCAML.wsp -immediate
%STSADM% -o execadmsvcjobs

%STSADM% -o deletesolution -name EFSPWFActivities.deleteItemsByCAML.wsp -override
%STSADM% -o execadmsvcjobs

%STSADM% -o addsolution -filename ".\EFSPWFActivities.deleteItemsByCAML.wsp"
%STSADM% -o execadmsvcjobs

%STSADM% -o deploysolution -name EFSPWFActivities.deleteItemsByCAML.wsp -immediate -force -allowgacdeployment
%STSADM% -o execadmsvcjobs

net start "SharePoint 2010 Administration"

net stop "SharePoint 2010 Timer"
net start "SharePoint 2010 Timer"

@rem iisreset

pause
