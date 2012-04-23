@SET STSADM="c:\program files\common files\microsoft shared\web server extensions\14\bin\stsadm"

cd "D:\Robert Kahn\CSSoft\OpenSources\EF SharePoint 2010 workflow activities\Package"
REM e:

net stop "SharePoint 2010 Administration"

@SET activityName=alterTaskByTaskID
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=completeTaskByTaskID
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=deleteItemsByCAML
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=deleteItemsByListview
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=getRelevantTaskID
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=getUserLoginsByGroupName
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=updateItemsByCAML
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=updateItemsByListview
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=waitForSeconds
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=getCurrentDateTime
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=getEmailAttachmentLinks
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=enableNewLineForRichText
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=CalculateBusinessHours
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=getLocalTimeFromUTC
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=getCountByCAML
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=getSumByCAML
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=getCountByListview
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

@SET activityName=getListIDByListTitle
%STSADM% -o retractsolution -name %activityName%.wsp -immediate
%STSADM% -o execadmsvcjobs
%STSADM% -o deletesolution -name %activityName%.wsp -override
%STSADM% -o execadmsvcjobs

net start "SharePoint 2010 Administration"

net stop "SharePoint 2010 Timer"
net start "SharePoint 2010 Timer"

@rem iisreset

pause
