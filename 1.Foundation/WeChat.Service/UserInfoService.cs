using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Sitecore.Configuration;

namespace WeChat.Service
{
    public class UserInfoService
    {
        private static readonly string appId = Settings.GetSetting("wechat.appid");
        private static readonly string appsecret = Settings.GetSetting("wechat.appsecret");
        public static UserInfo GetUserInfo(string code)
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                string tokenUrl = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appId, appsecret, code);

                AuthTokenResponse token = (AuthTokenResponse)JsonConvert.DeserializeObject(AppService.HttpRequestGet(tokenUrl), typeof(AuthTokenResponse));

                if (token != null)
                {
                    var expiresIn = DateTime.UtcNow.AddSeconds(Convert.ToDouble(token.Expires_in));

                    if (DateTime.UtcNow > expiresIn)
                    {
                        string refreshTokenUrl = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", appId, token.Refresh_token);

                        token = (AuthTokenResponse)JsonConvert.DeserializeObject(AppService.HttpRequestGet(refreshTokenUrl), typeof(AuthTokenResponse));
                    }

                    if (token != null && !string.IsNullOrWhiteSpace(token.Access_token))
                    {
                        string userInfoUrl = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=en", token.Access_token, token.Openid);

                        UserInfo userInfo = (UserInfo)JsonConvert.DeserializeObject(AppService.HttpRequestGet(userInfoUrl), typeof(UserInfo));

                        return userInfo;
                    }
                }
            }

            return null;
        }
    }

    public class UserInfo
    {
        public string Openid { get; set; }
        public string Nickname { get; set; }
        public string Sex { get; set; }
        public string Language { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
        public List<string> Privilege { get; set; }
        public string Unionid { get; set; }
    }

    public class AuthTokenResponse
    {
        public string Access_token { get; set; }
        public string Expires_in { get; set; }
        public string Refresh_token { get; set; }
        public string Openid { get; set; }
        public string Scope { get; set; }
    }
}
