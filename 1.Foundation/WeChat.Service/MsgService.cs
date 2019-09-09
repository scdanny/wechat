using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeChat.Service
{
    public class MsgService
    {
        public static string Send(MsgParams msgParams)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + AccessTokenService.GetAccessToken();

            string msg = JsonConvert.SerializeObject(msgParams);

            return AppService.WebRequestPost(url, msg);

        }

        public static string QYSend(QYMsgParams qymsgParams)
        {
            string url = " https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token=" + AccessTokenService.GetQYAccessToken();

            string msg = JsonConvert.SerializeObject(qymsgParams);

            return AppService.WebRequestPost(url, msg);

        }


        public static string SendRTMsg(RTMessageInt rtMessageInt)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + AccessTokenService.GetAccessToken();

            string msg = JsonConvert.SerializeObject(rtMessageInt);

            return AppService.WebRequestPost(url, msg);

        }
        public static string UploadImg(MediaParams imgParams)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token=" + AccessTokenService.GetAccessToken();
            //string img = JsonConvert.SerializeObject(imgParams);
            //return AppService.WebRequestPost(url,img);
            return AppService.WebRequestPost(url,imgParams.filename,imgParams.contenttype,imgParams.media);
        }

        public static string AddMaterial(MediaParams mediaParams)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/add_material?access_token=" + AccessTokenService.GetAccessToken() + "&type=" + mediaParams.mediatype ;
            //string img = JsonConvert.SerializeObject(imgParams);
            //return AppService.WebRequestPost(url,img);
            return AppService.WebRequestPost(url, mediaParams.filename, mediaParams.contenttype, mediaParams.media);
        }

        public static string AddNews(WCArticles wcArticles)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/add_news?access_token=" + AccessTokenService.GetAccessToken();

            //var jsonsSettings = new JsonSerializerSettings();
            
            string articles = JsonConvert.SerializeObject(wcArticles);
            articles = "{ \"articles\":" + articles + "}";
            return AppService.WebRequestPostJson(url, articles);
        }
        
        public static string ResetQuota(string appID)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/clear_quota?access_token=" + AccessTokenService.GetAccessToken();
            AppID myID = new AppID();
            myID.appid = appID;
            string wcID = JsonConvert.SerializeObject(myID);
            return AppService.WebRequestPost(url, wcID);
        }

        /// <summary>
        　　/// Base64加密
        　　/// </summary>
        　　/// <param name="Message"></param>
        　　/// <returns></returns>
        public static string Base64Code(string Message)
        {
            byte[] bytes = Encoding.Default.GetBytes(Message);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        　　/// Base64解密
        　　/// </summary>
        　　/// <param name="Message"></param>
        　　/// <returns></returns>
        public static string Base64Decode(string Message)
        {
            byte[] bytes = Convert.FromBase64String(Message);
            return Encoding.Default.GetString(bytes);
        }

    }
}

public class MsgParams
{
 public string touser { get; set; }
 public string msgtype { get; set; }
 public MsgContent text { get; set; }
}

public class QYMsgParams
{
    public string touser { get; set; }
    public string msgtype { get; set; }
    public MsgContent text { get; set; }

    public string agentid { get; set; }
    public string safe { get; set; }
}

public class MsgContent
{
    public string content { get; set; }
}

public class RTMessageInt
{
    public string touser { get; set; }
    public string msgtype { get; set; }
    public RTMsgID mpnews { get; set; }
}
public class RTMsgID
{
    public string media_id { get; set; }
}
public class SendMsgResponse
{
    public string errcode { get; set;}
    public string errmsg { get; set; }
}

public class MediaParams
{
    public string filename { get; set; }
    public int filelength { get; set; }
    public string contenttype { get; set; }

    public string mediatype { get; set; }
    public byte[] media { get; set; }

}

public class NewsParams
{
    public NewsParams() { }
    public string title { get; set; }
    public string thumb_media_id { get; set; }
    public string author { get; set; }
    public string digest { get; set; }
    public UInt32 show_cover_pic { get; set; }
    public string content { get; set; }
    public string content_source_url { get; set; }
    public UInt32 need_open_comment { get; set; }
    public UInt32 only_fans_can_comment { get; set; }

}
public class WCArticles : List<NewsParams>
{
    public WCArticles()  {   }

}

public class AppID
{
    public string appid { get; set; }
}

