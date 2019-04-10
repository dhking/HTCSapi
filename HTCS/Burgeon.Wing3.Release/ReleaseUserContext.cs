using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release
{
    public class ReleaseUserContext
    {

        private static string cookieDomain = "";

        /// <summary>
        /// 验证当前用户是否合法
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            int userid = 0;
            int.TryParse(UserId, out userid);
            ReleaseUser user = ReleaseUserManager.Instance.GetUserById(userid);
            if (user != null)
            {
                return string.Equals(Utils.EncryptUtil.Md5(UserId + user.DynamicCode), UserIdentifier) && !string.IsNullOrWhiteSpace(UserIdentifier);
            }
            else
            {
                return false;
            }
        }

        public void Setup(long userid, string name, string code)
        {
            this.UserId = userid.ToString();
            this.UserName = name;
            this.UserIdentifier = Utils.EncryptUtil.Md5(userid.ToString() + code);
            this.Timer = "00001";
            ReleaseUserManager.Instance.UpdateLogin(userid, code);
        }

        public ReleaseUserContext() { }

        public ReleaseUserContext(string _cookieDomain)
        {
            cookieDomain = _cookieDomain;
        }

        #region 用户Context

        public string UserId
        {
            get
            {
                try
                {
                    return GetCookieValue("UserId");
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                SetCookies("UserId", value);
            }
        }

        public string UserName
        {
            get
            {
                try
                {
                    return GetCookieValue("UserName");
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                SetCookies("UserName", value);
            }
        }

        //发布用户 唯一标识
        public string UserIdentifier
        {
            get
            {
                try
                {
                    return GetCookieValue("UserIdentifier");
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                SetCookies("UserIdentifier", value);
            }
        }

        public string Timer
        {
            get
            {
                try
                {
                    return GetCookieValue("Timer");
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                SetCookies("Timer", value);
            }
        }

        private static string GetCookieValue(string name)
        {
            return System.Web.HttpUtility.UrlDecode(CookieHelper.GetCookieValue(name).ToString());
        }

        private static void SetCookies(string cookieName, string cookieValue)
        {
            //2014-07-10 boris 修改过期时间只到凌晨6:00
            CookieHelper.RemoveCookie(cookieName);
            DateTime endtime = DateTime.Now.AddHours(12);
            CookieHelper.AddCookie(cookieName, System.Web.HttpUtility.UrlEncode(cookieValue), endtime, cookieDomain);
        }

        #endregion
    }
}
