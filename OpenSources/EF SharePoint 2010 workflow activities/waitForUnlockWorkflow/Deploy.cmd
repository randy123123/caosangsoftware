@SET STSADM="c:\program files\common files\microsoft shared\web server extensions\14\bin\stsadm"

net stop "SharePoint 2010 Administration"

%STSADM% -o retractsolution -name EFSPWFActivities.waitForUnlockWorkflow.wsp -immediate
%STSADM% -o execadmsvcjobs

%STSADM% -o deletesolution -name EFSPWFActivities.waitForUnlockWorkflow.wsp -override
%STSADM% -o execadmsvcjobs

%STSADM% -o addsolution -filename ".\EFSPWFActivities.waitForUnlockWorkflow.wsp"
%STSADM% -o execadmsvcjobs

%STSADM% -o deploysolution -name EFSPWFActivities.waitForUnlockWorkflow.wsp -immediate -force -allowgacdeployment
%STSADM% -o execadmsvcjobs

net start "SharePoint 2010 Administration"

net stop "SharePoint 2010 Timer"
net start "SharePoint 2010 Timer"

@rem iisreset

pause
