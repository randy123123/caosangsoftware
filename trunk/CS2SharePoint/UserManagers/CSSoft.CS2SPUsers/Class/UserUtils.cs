using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.Office.Server;
using System.Web.UI.WebControls;

namespace CSSoft.CS2SPUsers
{
    public class UserUtil
    {
        private string username = "";
        private UserProfileManager profileManager;

        public UserProfileManager ProfileManager
        {
            get
            {
                return profileManager;
            }
        }
        public bool IsEmployee(string userName)
        {
            string results = this.GetUserProfileValue(Property.URI_FirstName) + this.GetUserProfileValue(Property.URI_LastName);
            if (string.IsNullOrEmpty(results.Trim()))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Username domain/username
        /// </summary>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public UserUtil()
        {
            profileManager = new UserProfileManager();
        }

        public UserUtil(SPSite spSite)
        {
            SPServiceContext siteContext = SPServiceContext.GetContext(spSite);
            profileManager = new UserProfileManager(siteContext);
        }

        public UserUtil(SPSite spSite, string username)
        {
            this.username = username;
            SPServiceContext siteContext = SPServiceContext.GetContext(spSite);
            profileManager = new UserProfileManager(siteContext);
        }

        /// <summary>
        /// Create UserUtil Object
        /// </summary>
        /// <param name="username">string domain\username</param>
        public UserUtil(string username)
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
        /// Read Items ChoiceType from UserProfile Field in SSP
        /// </summary>
        /// <param name="fieldName">propertyName</param>
        /// <returns>ListItemCollection</returns>
        public System.Web.UI.WebControls.ListItemCollection GetItemsChoiceTypeProperty(string propertyName)
        {
            ListItemCollection listitems = new ListItemCollection();
            try
            {
                Microsoft.Office.Server.UserProfiles.UserProfile userProfile = this.GetUserProfile();
                UserProfileValueCollection valueCollection = null;
                if (userProfile[propertyName] is UserProfileValueCollection)
                    valueCollection = userProfile[propertyName] as UserProfileValueCollection;

                string[] s = null;
                if (valueCollection != null)
                {
                    foreach (object propValue in valueCollection)
                    {
                        try
                        {
                            s = propValue.ToString().Split(';');
                            for (int i = 0; i < s.Length; i++)
                            {
                                listitems.Add(s[i]);
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }
            return listitems;
        }

        /// <summary>
        /// Read Items ChoiceType from UserProfile Field in SSP
        /// </summary>
        /// <param name="fieldName">propertyName</param>
        /// <returns>ListItemCollection</returns>
        public System.Web.UI.WebControls.ListItemCollection GetValueItemsChoiceType(string propertyName)
        {
            Microsoft.Office.Server.UserProfiles.UserProfile Profile = this.GetUserProfile();
            ListItemCollection listResult = new ListItemCollection();
            if (Profile == null)
            {
                return listResult;
            }
            if (Profile[propertyName] == null)
            {
                return listResult;
            }
            for (int i = 0; i < Profile[propertyName].Count; i++)
            {
                if (Profile[propertyName][i] != null)
                {
                    if (!string.IsNullOrEmpty(Profile[propertyName][i].ToString()))
                    {
                        listResult.Add(Profile[propertyName][i].ToString());
                    }
                }
            }
            return listResult;
        }

        /// <summary>
        /// Get User Profile Value
        /// </summary>
        /// <param name="propertyName">propertyName</param>
        /// <returns>Value</returns>
        public string GetUserProfileValue(string propertyName)
        {
            string userProfileValue = string.Empty;
            try
            {
                UserProfile userProfile = GetUserProfile();
                if (userProfile[propertyName] != null)
                {
                    userProfileValue = userProfile[propertyName].Value != null ? userProfile[propertyName].Value as string : string.Empty;
                }
            }
            catch (Exception)
            {
                //return userProfileValue;
            }
            return userProfileValue;
        }

        /// <summary>
        /// Get User Profile Value Object
        /// </summary>
        /// <param name="propertyName">string propertyName</param>
        /// <returns>object</returns>
        public object GetUserProfileValueObject(string propertyName)
        {
            object userProfileValueObject = null;
            try
            {
                UserProfile userProfile = GetUserProfile();
                if (userProfile[propertyName] != null)
                {
                    userProfileValueObject = userProfile[propertyName].Value != null ?
                       userProfile[propertyName].Value : null;
                }
            }
            catch (Exception)
            {
                return userProfileValueObject;
            }
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
        public bool UpdateChooseItemProfile(string name, ListItemCollection addValues)
        {
            try
            {

                Microsoft.Office.Server.UserProfiles.UserProfile Profile = this.GetUserProfile();
                Profile[name].Clear();
                if (addValues == null)
                {
                    Profile.Commit();
                    return true;
                }
                foreach (var item in addValues)
                {
                    Profile[name].Add(item.ToString());
                }
                Profile.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool AddItemToChooseProfile(string name, string addValue)
        {
            try
            {

                Microsoft.Office.Server.UserProfiles.UserProfile Profile = this.GetUserProfile();
                if (Profile == null)
                {
                    return false;
                }
                if (Profile[name] == null)
                {
                    return false;
                }
                Profile[name].Add(addValue);
                Profile.Commit();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool RemoveChooseItemProfile(string name, string value)
        {
            try
            {

                Microsoft.Office.Server.UserProfiles.UserProfile Profile = this.GetUserProfile();
                if (Profile == null)
                {
                    return false;
                }
                if (Profile[name] == null)
                {
                    return false;
                }
                if (Profile[name].Count == 0)
                {
                    return false;
                }
                bool flags = false;
                for (int i = 0; i < Profile[name].Count; i++)
                {
                    if (Profile[name][i] != null)
                    {
                        if (Profile[name][i].ToString().Equals(value))
                        {
                            Profile[name].RemoveAt(i);
                            Profile.Commit();
                            flags = true;
                        }
                    }
                }
                return flags;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
