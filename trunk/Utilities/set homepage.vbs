Set WSHShell = WScript.CreateObject("WScript.Shell")
' home page URL
StartPage = "http://natuan"
WSHShell.RegWrite "HKLM\Software\Microsoft\Internet Explorer\Main\Start Page", StartPage
WSHShell.RegWrite "HKCU\Software\Microsoft\Internet Explorer\Main\Start Page", StartPage