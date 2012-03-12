echo Adding assemblies to the GAC...
set path=D:\Research\sppex\CodeArt.SharePoint.PermissionEx\CodeArt.SharePoint.PermissionEx
"%path%\gacutil.exe" -if %path%\bin\Debug\CodeArt.SharePoint.PermissionEx.dll 

 
iisreset

 
REM -- iisapp /a "SharePoint - 81" /r

pause     