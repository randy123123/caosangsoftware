
xcopy /s /Y /C /R "..\CodeArt.SharePoint.PermissionEx\bin\debug\CodeArt.SharePoint.PermissionEx.dll" "GAC\"
xcopy /s /Y /C /R "..\CodeArt.SharePoint.PermissionEx\12\*" "12\"

xcopy /s /Y /C /R "..\CodeArt.SharePoint.PermissionEx.AppPages\bin\CodeArt.SharePoint.PermissionEx.AppPages.dll" "GAC"

xcopy /s /Y /C /R "..\CodeArt.SharePoint.PermissionEx.AppPages\*.aspx" "12\TEMPLATE\layouts\codeart\"

xcopy /s /Y /C /R "..\CodeArt.SharePoint.PermissionEx\Resources\*.resx" "12\Resources\"

xcopy /s /Y /C /R "..\CodeArt.SharePoint.PermissionEx\Readme.txt" "Readme.txt"

WSPBuilder  -WSPName CodeArt_PermissionEx.wsp
pause