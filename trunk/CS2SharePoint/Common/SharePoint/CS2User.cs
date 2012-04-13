using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Net.Mail;
using Microsoft.SharePoint.Administration;
using System.IO;
using Microsoft.Office.Server.UserProfiles;

namespace CSSoft
{
    public partial class CS2User : IDisposable
    {
        #region IDisposable
        ~CS2User() 
        {
            Dispose();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable
        
        private string username = "";
        private UserProfileManager profileManager;

        public UserProfileManager ProfileManager
        {
            get
            {
                return profileManager;
            }
        }

        /// <summary>
        /// Username domain/username
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public CS2User()
        {
            profileManager = new UserProfileManager();
        }

        public CS2User(SPSite spSite)
        {
            SPServiceContext siteContext = SPServiceContext.GetContext(spSite);
            profileManager = new UserProfileManager(siteContext);
        }

        public CS2User(SPSite spSite, string username)
        {
            this.username = username;
            SPServiceContext siteContext = SPServiceContext.GetContext(spSite);
            profileManager = new UserProfileManager(siteContext);
        }

        /// <summary>
        /// Create UserUtil Object
        /// </summary>
        /// <param name="username">string domain\username</param>
        public CS2User(string username)
        {
            this.username = username;
            profileManager = new UserProfileManager();
        }

        /// <summary>
        /// Get User Profile in SSP
        /// </summary>
        /// <returns>UserProfile</returns>
        public UserProfile GetUserProfile()
        {
            UserProfile usrProfile = username.EndsWith(@"\system") ? profileManager.GetUserProfile(true) : profileManager.GetUserProfile(username);
            return usrProfile;
        }
        
        /// <summary>
        /// Get User Profile Value Object
        /// </summary>
        /// <param name="propertyName">string propertyName</param>
        /// <returns>object</returns>
        public object GetUserProfileValue(string propertyName)
        {
            object userProfileValueObject = null;
            try
            {
                UserProfile userProfile = GetUserProfile();
                userProfileValueObject = userProfile[propertyName].Value;
            }
            catch { }
            return userProfileValueObject;
        }

        /// <summary>
        /// Get Colleagues in User Profile
        /// </summary>
        /// <returns>Colleague[]</returns>
        public Colleague[] GetColleagues()
        {
            ColleagueManager colleagueManager = GetUserProfile().Colleagues;
            return colleagueManager.GetItems();
        }

        /// <summary>
        /// Get Properties in Section
        /// </summary>
        /// <param name="sectionName">sectionName</param>
        /// <returns>List<Property></returns>
        public System.Collections.Generic.List<Property> GetPropertiesWithSection(string sectionName)
        {
            System.Collections.Generic.List<Property> properties = new System.Collections.Generic.List<Property>();
            bool isSectionName = false;
            foreach (Property prop in profileManager.PropertiesWithSection)
            {
                if (prop.IsSection)
                {
                    isSectionName = (prop.DisplayName == sectionName);
                }
                else if (isSectionName)
                {
                    properties.Add(prop);
                }
            }
            return properties;
        }
        /// <summary>
        /// Update a property of user
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <param name="value">Value of property</param>
        /// <returns>true if updated/ false can not update profile</returns>
        public bool UpdateProfile(string name, object value)
        {
            try
            {
                Microsoft.Office.Server.UserProfiles.UserProfile Profile = this.GetUserProfile();
                Profile[name].Value = value;
                Profile.Commit();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
