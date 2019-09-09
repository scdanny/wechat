using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeChat.Service;

using Sitecore.Configuration;

using Newtonsoft.Json;
using Feature.SitecoreWechat.Areas.Wechat.Models;
using WeChat.Service.Tencent;


namespace Feature.SitecoreWechat.Areas.Wechat.Controllers
{
    public class WechatController : Controller
    {
        // GET: Wechat/Wechat
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VerifySignature()
        {
            if (Request["echostr"].ToString() != null)
            {
                string signature = Request["signature"];
                string timestamp = Request["timestamp"];
                string nonce = Request["nonce"];
                string echostr = Request["echostr"];

                Sitecore.Diagnostics.Log.Info("Signature:" + signature + " " + "Timestamp:" + timestamp + " " + "Nonce:" + nonce + " " + "Echostr:" + echostr, this);

                return Content(echostr);

            }
            else
            {
                return null;
            }
        }

        public ActionResult QyCallBack()
        {

            string token = Settings.GetSetting("CorpToken");//从配置文件获取Token

            string encodingAESKey = Settings.GetSetting("EncodingAESKey");//从配置文件获取EncodingAESKey

            string corpId = Settings.GetSetting("CorpId");//从配置文件获取corpId

            string echoString = Request["echoStr"];
            string signature = Request["msg_signature"];//企业号的 msg_signature
            string timestamp = Request["timestamp"];
            string nonce = Request["nonce"];

            Sitecore.Diagnostics.Log.Info("Signature:" + signature + " " + "Timestamp:" + timestamp + " " + "Nonce:" + nonce + " " + "Echostr:" + echoString, this);

            string decryptEchoString = "";
            if (CheckSignature(token, signature, timestamp, nonce, corpId, encodingAESKey, echoString, ref decryptEchoString))
            {
                if (!string.IsNullOrEmpty(decryptEchoString))
                {
                    Sitecore.Diagnostics.Log.Info("decryptEchoString:" + decryptEchoString, this);
                    return Content(decryptEchoString);
                }
                else { return null; }
            }
            else { return null; }
        }

        public bool CheckSignature(string token, string signature, string timestamp, string nonce, string corpId, string encodingAESKey, string echostr, ref string retEchostr)
        {
            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(token, encodingAESKey, corpId);
            int result = wxcpt.VerifyURL(signature, timestamp, nonce, echostr, ref retEchostr);
            if (result != 0)
            {
                //LogTextHelper.Error("ERR: VerifyURL fail, ret: " + result);
                return false;
            }

            return true;

            //ret==0表示验证成功，retEchostr参数表示明文，用户需要将retEchostr作为get请求的返回参数，返回给企业号。
            // HttpUtils.SetResponse(retEchostr);
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
                name = "Sitecore",
                url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + "appid=" + Settings.GetSetting("wechat.appid") + "&redirect_uri=" + Settings.GetSetting("wechat.redirect_uri") + redirect_page + "&response_type=code&scope=snsapi_userinfo&state=1803#wechat_redirect"
            });

            createMenuParams.button.Add(new CreateMenuParams.MenuButton()
            {
                type = "view",
                name = "Habitat",
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

        public ActionResult WechatLogin()
        {
            return View("WechatLogin");
        }


    }
}