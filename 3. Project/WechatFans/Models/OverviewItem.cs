using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WechatFans.Models
{
    public class OverviewItem
    {
        public OverviewItem()
        {

        }
        public HtmlString title { get; set; }
        public string URL { get; set; }
        public string imageUrl { get; set; }
        public HtmlString image { get; set; }
    }
}