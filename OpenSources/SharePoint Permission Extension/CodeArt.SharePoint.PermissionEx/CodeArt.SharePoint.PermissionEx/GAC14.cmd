echo Adding assemblies to the GAC...
set path=D:\Research\sppex\CodeArt.SharePoint.PermissionEx\CodeArt.SharePoint.PermissionEx
"%path%\gacutil.exe" -if %path%\bin\Debug\CodeArt.SharePoint.PermissionEx.dll 
set pathApp=D:\Research\sppex\CodeArt.SharePoint.PermissionEx\CodeArt.SharePoint.PermissionEx.AppPages
"%path%\gacutil.exe" -if %pathApp%\bin\CodeArt.SharePoint.PermissionEx.AppPages.dll 
 
  
 
REM -- iisapp /a "SharePoint - 81" /r

c:\windows\system32\inetsrv\AppCmd Recycle AppPool "SharePoint - 80"

REM -- pause     