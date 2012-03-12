using System;
using System.Collections.Generic;
using System.Text;

namespace CoolStuffs.Sharepoint.Constants
{
    public  class ConstantLibrary
    {
        private static string currentUser = "F65CDAC1-0529-4a94-9BB2-3968C0F14ABE";
        private static string currentDateTime = "C2ECEA4B-E828-43fa-87BE-3A67BC518D86";
        private static string openingCurlyBraces = "8630DDE6-9BF9-47bf-A2FD-4177FA7F6890_";
        private static string closingCurlyBraces = "_32549903-3F2F-4b3c-BDD8-9DA529069EBE";

        public string CurrentUser
        {
            get
            {
                return currentUser;
            }
        }

        public string CurrentDateTime
        {
            get
            {
                return currentDateTime;
            }
        }

        public string OpeningCurlyBraces
        {
            get
            {
                return openingCurlyBraces;
            }
        }

        public string ClosingCurlyBraces
        {
            get
            {
                return closingCurlyBraces;
            }
        }
    }
}
