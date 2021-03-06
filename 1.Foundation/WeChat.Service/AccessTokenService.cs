﻿using Newtonsoft.Json;
using System;
using System.Web;
using Sitecore.Configuration;

namespace WeChat.Service
{
    public class AccessTokenService
    {
        //private static readonly string appId = "wx45d424260592cfd4";
        //private static readonly string appsecret = "3ce3dade566c8cf5d33acafd97c2fd35";

        // public wechat
        private static readonly string appId = Settings.GetSetting("wechat.appid");
        private static readonly string appsecret = Settings.GetSetting("wechat.appsecret");

        //Enterprise wechat
        private static readonly string CorpId = Settings.GetSetting("CorpId");
        private static readonly string AgentID = Settings.GetSetting("AgentID");
        private static readonly string AgentSecret = Settings.GetSetting("AgentSecret");

        public static string GetAccessToken()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("accessToken");

            // Check if cookie exists in the current request.
            if (cookie == null)
            {
                AccessToken token = (AccessToken)JsonConvert.DeserializeObject(AppService.HttpRequestGet("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appId + "&secret=" + appsecret), typeof(AccessToken));

                if (token != null && !string.IsNullOrWhiteSpace(token.Access_Token))
                {
                    cookie = new HttpCookie("accessToken")
                    {
                        Value = token.Access_Token,
                        // Set cookie to expire in 2 hours.
                        Expires = DateTime.Now.AddHours(2)
                    };
                    // Insert the cookie in the current HttpResponse.
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    return token.Access_Token;
                }
                else
                {
                    return "Try again in sometime.";
                }
            }
            else
            {
                return cookie.Value;
            }
        }

        public static string GetQYAccessToken()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("QYaccessToken");

            // Check if cookie exists in the current request.
            if (cookie == null)
            {
                AccessToken token = (AccessToken)JsonConvert.DeserializeObject(AppService.HttpRequestGet("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=" + CorpId + "&corpsecret=" + AgentSecret), typeof(AccessToken));

                if (token != null && !string.IsNullOrWhiteSpace(token.Access_Token))
                {
                    cookie = new HttpCookie("QYaccessToken")
                    {
                        Value = token.Access_Token,
                        // Set cookie to expire in 2 hours.
                        Expires = DateTime.Now.AddHours(2)
                    };
                    // Insert the cookie in the current HttpResponse.
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    return token.Access_Token;
                }
                else
                {
                    return "Try again in sometime.";
                }
            }
            else
            {
                return cookie.Value;
            }
        }
    }

    public class AccessToken
    {
        // Properties
        public string Access_Token { get; set; }

        public int Expires_In { get; set; }
    }



}
