using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WeChat.Service;

using Sitecore.Configuration;

using Newtonsoft.Json;
using Feature.SitecoreWechat.Areas.Wechat.Models;

namespace Feature.SitecoreWechat.Controllers
{
    public class WechatIdentifyController : Controller
    {
        // GET: WechatIdentify
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserInfo()
        {
            if (!(Sitecore.Context.PageMode.IsExperienceEditor))
            {
                string code = Request.QueryString["code"];

                var userInfo = UserInfoService.GetUserInfo(code);

                if (userInfo != null)
                {
                    ContactService.AddContact(userInfo);
                    return View("GetUserInfo", userInfo);
                }
                return View("EmptyUserInfo");
            }
            return View("EmptyUserInfo");
        }

        public ActionResult LoginForm()
        {
            return View("LoginForm");
        }
    }
}