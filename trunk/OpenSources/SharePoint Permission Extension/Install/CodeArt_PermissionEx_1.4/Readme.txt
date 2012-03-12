
SharePoint Permission Extension 

Project Description： 

Extend the SharePoint Permission Functions:  
1)Provider Field Level Permisson Control(support 07/10)
2)Provider View Level Permisson Control(support 07/10)
3)Provider Content Type Creation Permission Control(support 07)

site: http://sppex.codeplex.com

Assembly:
CodeArt.SharePoint.PermissionEx, Version=1.0.0.0, Culture=neutral, PublicKeyToken=22b3aebaf288927f
CodeArt.SharePoint.PermissionEx.AppPages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=22b3aebaf288927f

How to install:

Step1: download the wsp file and unzip on the SharePoint server.
Step2: run wsp_install.cmd or wsp_update.cmd to install or update this solution.
for 2010, run wsp_install14.cmd or wsp_update14.cmd to install or update this solution.
Step3: active the features on site collection features management page.
Step4: go to list settings page to do configuration.
 

How to add other language support:

Step1: add the resource file in Resources folder.
Step2: run wsp_build.cmd to build a new solution setup file.
Step3: run wsp_install.cmd or wsp_update.cmd to install or update this solution.

Known Issues:
1) if you use view permission and after do the setting, you find nothing happen.
solutoin: add the ViewPermissionControlPart webpart to the view needs permission control.

ChangeLogs:
100726: add multi-language support.
100728 bug fix: not support anonymous access
11-1208 change dll deployment mode to GAC.
12-0216 remove resource assembly and deoloy resource xml to 12/resources
add ViewRightControl