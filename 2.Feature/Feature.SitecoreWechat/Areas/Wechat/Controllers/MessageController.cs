using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeChat.Service;
using Newtonsoft.Json;
using Feature.SitecoreWechat.Areas.Wechat.Models;
using Sitecore;
using Sitecore.Data;
using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using System.IO;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Feature.SitecoreWechat.Areas.Wechat.Controllers
{
    public class MessageController : Controller
    { 
        // GET: Wechat/Message
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SendForm()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Send(string ToUserName,string ContentBody,string MsgType)
        {
            MsgParams msgParams = new MsgParams();
            

            msgParams.touser = ToUserName;
            
            msgParams.msgtype = MsgType;

            MsgContent msgContent = new MsgContent();
            msgContent.content = ContentBody;

            msgParams.text = msgContent;

            string req0 = JsonConvert.SerializeObject(msgParams);

            string r1 = MsgService.Send(msgParams);
            return Content(req0+r1);

            //SendMsgResponse response =(SendMsgResponse)JsonConvert.DeserializeObject(MsgService.Send(msgParams),typeof(SendMsgResponse));
            //if (response.errcode == "0")
            //{
            //    return View("SendForm");
            //}else
            //{
            //    return View("SendError");
            //}
            
        }

        [HttpGet]
        public ActionResult UploadImageForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadImage()
        {
            HttpPostedFileBase file = Request.Files["file"];
            MediaParams imgParams = new MediaParams();
            imgParams.filename = file.FileName;
            imgParams.filelength = file.ContentLength;
            imgParams.contenttype = file.ContentType;
            imgParams.media = new byte[file.InputStream.Length];
            file.InputStream.Read(imgParams.media, 0, (int)file.InputStream.Length);
            file.InputStream.Close();
            string req0 = JsonConvert.SerializeObject(imgParams) + "\r\n";
            //string req0 = file.FileName.ToString();
            string r1 = MsgService.UploadImg(imgParams);
            return Content(HttpUtility.UrlDecode(r1));
        }

        public ActionResult UploadMedia()
        {
            List<WechatMedia> mList = new List<WechatMedia>();
            var mdb = Sitecore.Configuration.Factory.GetDatabase("master");
            var mItem = mdb.GetItem(Request.Params["myItem"]);
            var localItem = mdb.GetItem(Request.Params["localItem"]);
            if ((mItem == null)||(localItem==null)) return Content("Item is not existing!");


            if (!UploadThumbnail(mItem))
            {
                //return UploadThumbnail(mItem);
                return Content("Upload News Thumbnail failed!");
            }
            //ChildList mChldren =(ChildList)mItem.GetChildren().Where(i => i.TemplateName == "Msg Paragraph");
            ChildList mChildren = localItem.GetChildren();
            if(mChildren ==null) return Content("Children are empty!");

            MediaParams mediaParams = new MediaParams();

            

            foreach (Item i in mChildren)
            {
                var mField= (ImageField)i.Fields["PImage"];
                MediaItem mediaItem = mField.MediaItem;
               
                mediaParams.filename = mediaItem.Name + "." + mediaItem.Extension;
                mediaParams.filelength = (int) mediaItem.GetMediaStream().Length;
                mediaParams.contenttype = mediaItem.MimeType;
                mediaParams.media = new byte[(int)mediaItem.GetMediaStream().Length];

                using (Stream stream = mediaItem.GetMediaStream())
                {
                   

                    stream.Read(mediaParams.media, 0, (int)mediaItem.GetMediaStream().Length);
                }

                MediaUrl mediaUrl =(MediaUrl)JsonConvert.DeserializeObject(MsgService.UploadImg(mediaParams),typeof(MediaUrl));

                mList.Add(new WechatMedia
                {

                    itemId = i.ID.ToString(),
                    mediaId = null,
                    mediaUrl = HttpUtility.UrlDecode(mediaUrl.Url)

                });
                
            }

            if (UpdateUrl(mList)) return Content("Update Media Urls Successfully");
            else return Content("Update Media Urls failed");
 
        }

        private Boolean UpdateUrl(List<WechatMedia> mList)
        {
            var mdb = Sitecore.Configuration.Factory.GetDatabase("master");
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                foreach (WechatMedia j in mList)
                {
                    Item uitem = mdb.GetItem(j.itemId);
                    uitem.Editing.BeginEdit();
                    uitem["mediaurl"] = j.mediaUrl;
                    uitem.Editing.EndEdit();
                }
            }
            return true;
        }

        private Boolean UploadThumbnail(Item mItem)
        {
            MediaParams mediaParams = new MediaParams();
            mediaParams.mediatype = "thumb";
            
            var mField = (ImageField)mItem.Fields["Image"];
            if(mField == null)
            {
                return false;
            }

            MediaItem mediaItem = mField.MediaItem;

            if(mediaItem == null)
            {
                return false;
            }

            mediaParams.filename = mediaItem.Name + "." + mediaItem.Extension;
            mediaParams.filelength = (int)mediaItem.GetMediaStream().Length;
            mediaParams.contenttype = mediaItem.MimeType;
            mediaParams.media = new byte[(int)mediaItem.GetMediaStream().Length];

            using (Stream stream = mediaItem.GetMediaStream())
            {


                stream.Read(mediaParams.media, 0, (int)mediaItem.GetMediaStream().Length);
            }

            MaterialResponse materialResponse = (MaterialResponse)JsonConvert.DeserializeObject(MsgService.AddMaterial(mediaParams), typeof(MaterialResponse));

            
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                    
                   
                mItem.Editing.BeginEdit();
                mItem["ThumbnailID"] = materialResponse.Media_Id;
                mItem["ThumbnailUrl"] = HttpUtility.UrlDecode(materialResponse.Url);
                mItem.Editing.EndEdit();
                
            }

            return true;
            //return Content(JsonConvert.SerializeObject(materialResponse));
        }

        [HttpPost]
        public ActionResult PublishHtml(string itemID,string toUserName,string genHtml)
        {
            var mdb = Sitecore.Configuration.Factory.GetDatabase("master");
            var mItem = mdb.GetItem(new Sitecore.Data.ID(itemID));
            if (mItem == null) return Content("Item is not existing!");
            WCArticles articles = new WCArticles();
            NewsParams newsParams = new NewsParams();

            newsParams.show_cover_pic = 1;
            newsParams.need_open_comment = 1;
            newsParams.only_fans_can_comment = 1;
            newsParams.title = mItem.Fields["Title"].ToString();
            newsParams.author = mItem.Fields["Author"].ToString();
            newsParams.digest = mItem.Fields["Summary"].ToString();
            newsParams.thumb_media_id = mItem.Fields["thumbnailID"].ToString();
            newsParams.content_source_url = Sitecore.Links.LinkManager.GetItemUrl(mItem);
            newsParams.content = MsgService.Base64Decode(Request.Params["genHtml"]);

            articles.Add(newsParams);

            NewsResponse newsResponse = (NewsResponse)JsonConvert.DeserializeObject(MsgService.AddNews(articles), typeof(NewsResponse));

            using (new Sitecore.SecurityModel.SecurityDisabler())
            {


                mItem.Editing.BeginEdit();
                mItem["NewsID"] = newsResponse.Media_ID;
                
                mItem.Editing.EndEdit();

            }

            RTMsgID rtMsgID = new RTMsgID();
            rtMsgID.media_id = newsResponse.Media_ID;

            RTMessageInt rtMessageInt = new RTMessageInt();
            rtMessageInt.touser = toUserName;
            rtMessageInt.msgtype = "mpnews";
            rtMessageInt.mpnews = rtMsgID;
            string r = MsgService.SendRTMsg(rtMessageInt);

            return Content(r);
        }

        
        public ActionResult ResetQuota(string appID)
        {
        
            string r = MsgService.ResetQuota(appID);
            return Content(r);
        }
    }
}