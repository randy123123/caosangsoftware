@SET STSADM="c:\program files\common files\microsoft shared\web server extensions\14\bin\stsadm"

cd "D:\Robert Kahn\CSSoft\OpenSources\EF SharePoint 2010 workflow activities\Package"
REM e:

net stop "SharePoint 2010 Administration"

@SET solutionName=EFSPWFActivities

@SET activityName=getSumByCAML
%STSADM% -o retractsolution -name %solutionName%.%activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %solutionName%.%activityName%.wsp -override
%STSADM% -o execadmsvcjobs
%STSADM% -o addsolution -filename "%activityName%\%solutionName%.%activityName%.wsp"
%STSADM% -o execadmsvcjobs
%STSADM% -o deploysolution -name %solutionName%.%activityName%.wsp -immediate -force -allowgacdeployment
%STSADM% -o execadmsvcjobs

net start "SharePoint 2010 Administration"

net stop "SharePoint 2010 Timer"
net start "SharePoint 2010 Timer"

@rem iisreset

pause
