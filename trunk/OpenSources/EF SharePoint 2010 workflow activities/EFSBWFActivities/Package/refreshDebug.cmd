@SET basepath="C:\EricFang\VisualStudio\EFSPWFActivities\EFSBWFActivities\"

cd "C:\EricFang\VisualStudio\EFSPWFActivities\EFSBWFActivities\Package"
c:

@SET nameSpace=EFSBWFActivities

@SET activityName=waitForUnlockWorkflowSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=completeTaskByTaskIdSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=deleteItemsByCamlSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=deleteItemsByListviewSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=getRelevantTaskIdSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=getUserLoginsByGroupNameSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=updateItemsByCamlSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=updateItemsByListviewSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=waitForSecondsSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=getCurrentDateTimeSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=getEmailAttachmentLinksSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=enableNewLineForRichTextSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=CalculateBusinessHoursSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=getLocalTimeFromUtcSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=getCountByCamlSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

@SET activityName=getCountByListviewSB
copy %basepath%%activityName%\bin\Debug\%nameSpace%.%activityName%.wsp  .\%nameSpace%.%activityName%.wsp /Y

pause
