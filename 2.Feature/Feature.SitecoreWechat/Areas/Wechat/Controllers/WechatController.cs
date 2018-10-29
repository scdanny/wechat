using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}