using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.XConnect;

namespace WeChat.Service
{
    [Serializable]
    [FacetKey(DefaultFacetKey)]
    public class WechatUserFacet : Sitecore.XConnect.Facet
    {
        public const string DefaultFacetKey = "WechatUserFacet";
        public WechatUserFacet(string openid)
        {
            Openid = openid;
        }

        private WechatUserFacet() { }

        public string Openid { get; private set; } // Example: ABC12345
       // public string Openid { get; set; }
        public string Nickname { get; set; }
        public string Sex { get; set; }
        public string Language { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
        public List<string> Privilege { get; set; }
        public string Unionid { get; set; }



    }
}
