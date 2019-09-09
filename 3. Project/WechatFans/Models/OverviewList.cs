using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WechatFans.Models
{
    public class OverviewList : List<OverviewItem>
    {
        public OverviewList() { }

        public string ReadMore { get; set; }
    }
}