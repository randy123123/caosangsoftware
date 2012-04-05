using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CSSoft
{
    public class _Test
    {
        public void TestCS2Secret()
        {
            string pass = CS2Secret.CS2Password;
            string message = "tringuyen@live.com";
            string encryptString = CS2Secret.EncryptString(message);
            string decryptString = CS2Secret.DecryptString(encryptString);
            string data = "VhLIYvSa++ZrOJKtlUK+2+OibY/gVmwzJCf7j2UG/zog7Yvy6CAq/sGfwXlBkYCecJr+wrLHsIE=";
            string getData = CS2Secret.DecryptString(data);
            data = "VhLIYvSa++ZrOJKtlUK+2+OibY/gVmwzJCf7j2UG/zog7Yvy6CAq/gEVJWkxQYsKbnUKzLGo038=";
            getData = CS2Secret.DecryptString(data);
        }
    }
}
