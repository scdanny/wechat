using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeChat.Service;

using Sitecore.Configuration;

using Newtonsoft.Json;
using Feature.SitecoreWechat.Areas.Wechat.Models;


namespace Feature.SitecoreWechat.Areas.Wechat.Controllers
{
    public class WechatController : Controller
    {
        // GET: Wechat/Wechat
        public ActionResult Index()
        {
            return View();
        }
        public string VerifySignature()
        {
            if(Request["echostr"].ToString() !=null)
            {
                string signature = Request["signature"];
                string timestamp = Request["timestamp"];
                string nonce = Request["nonce"];
                string echostr = Request["echostr"];
                
                Sitecore.Diagnostics.Log.Info("Signature:" + signature + " " + "Timestamp:" + timestamp + " " + "Nonce:" + nonce + " " + "Echostr:" + echostr, this);

                return echostr;

            }else
            {
                return null;
            }
        }

        public ActionResult GetQrcode()

        {
            string r1 = QRCodeService.CreateQRCode();

            Qrcode qrcode = JsonConvert.DeserializeObject<Qrcode>(r1);

            return View(qrcode);

            //return Content(QRCodeService.CreateQRCode());
        }
        public ActionResult CreateWechatMenu()
        {
            string redirect_page = "";
            var createMenuParams = new CreateMenuParams();
            createMenuParams.button.Add(new CreateMenuParams.MenuButton()
            {
                type = "view",
                name = "Sitecore XP",
                url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + "appid=" + Settings.GetSetting("wechat.appid") + "&redirect_uri=" + Settings.GetSetting("wechat.redirect_uri") + redirect_page + "&response_type=code&scope=snsapi_userinfo&state=1803#wechat_redirect"
            });

            createMenuParams.button.Add(new CreateMenuParams.MenuButton()
            {
                type = "view",
                name = "Sitecore commerce",
                url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + "appid=" + Settings.GetSetting("wechat.appid") + "&redirect_uri=" + Settings.GetSetting("wechat.redirect_uri") + redirect_page + "&response_type=code&scope=snsapi_userinfo&state=1803#wechat_redirect"
            });

            string r1 = CreateMenuService.CreateMenu(createMenuParams);
            return Content(r1);
        }
        public ActionResult GetUserInfo()
        {
            if (!(Sitecore.Context.PageMode.IsExperienceEditor))
            {
                string code = Request.QueryString["code"];

                var userInfo = UserInfoService.GetUserInfo(code);

                if (userInfo != null)
                {
                    // ContactService.AddContact(userInfo);
                    return View("GetUserInfo", userInfo);
                }
                return View("EmptyUserInfo");
            }
            return View("EmptyUserInfo");
        }
        


    }
}