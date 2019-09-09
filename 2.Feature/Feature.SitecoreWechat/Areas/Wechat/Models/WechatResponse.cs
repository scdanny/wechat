using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Feature.SitecoreWechat.Areas.Wechat.Models
{
    public class WechatResponse
    {
    }
    public class MediaUrl
    {
        public string Url { get; set; }
    }

    public class MaterialResponse
    {
        public string Media_Id { get; set; }
        public string Url { get; set; }
    }

    public class NewsResponse
    {
        public string Media_ID { get; set; }
    }
}