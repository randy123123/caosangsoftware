@SET basepath=D:\Robert Kahn\CSSoft\OpenSources\EF SharePoint 2010 workflow activities\

cd "D:\Robert Kahn\CSSoft\OpenSources\EF SharePoint 2010 workflow activities\Package"
REM e:

@SET solutionName=EFSPWFActivities

@SET activityName=alterTaskByTaskID
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=completeTaskByTaskID
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=deleteItemsByCAML
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=deleteItemsByListview
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=getRelevantTaskID
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=getUserLoginsByGroupName
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=updateItemsByCAML
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=updateItemsByListview
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=waitForSeconds
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=getCurrentDateTime
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=getEmailAttachmentLinks
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=enableNewLineForRichText
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=CalculateBusinessHours
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=getLocalTimeFromUTC
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=getCountByCAML
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=getSumByCAML
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=getCountByListview
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

@SET activityName=getListIDByListTitle
mkdir %activityName%
copy "%basepath%%activityName%\bin\Debug\Deploy.cmd" "%activityName%\Deploy.cmd" /Y
copy "%basepath%%activityName%\bin\Debug\%solutionName%.%activityName%.wsp" "%activityName%\%solutionName%.%activityName%.wsp" /Y

pause
