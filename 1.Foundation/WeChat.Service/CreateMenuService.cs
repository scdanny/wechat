using Newtonsoft.Json;
using System.Collections.Generic;

namespace WeChat.Service
{
    public class CreateMenuService
    {
        public static string CreateMenu()
        {
            string accessToken = AccessTokenService.GetAccessToken();

            string url = "https://api.wechat.com/cgi-bin/menu/create?access_token=" + accessToken;

            var createMenuParams = new CreateMenuParams();
            createMenuParams.button.Add(new CreateMenuParams.MenuButton()
            {
                type = "view",
                name = "Sitecore Experience Platform",
                url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxe50bdb27143920a2&redirect_uri=http://wechatpoc.southeastasia.cloudapp.azure.com/&response_type=code&scope=snsapi_userinfo&state=1803#wechat_redirect"
            });

            string parameters = JsonConvert.SerializeObject(createMenuParams);
            return AppService.WebRequestPost(url, parameters);
            //return (CreateMenuResponse)JsonConvert.DeserializeObject(response, typeof(CreateMenuResponse));
        }

    }

    public class CreateMenuResponse
    {
        public string Errcode { get; set; }
        public string Errmsg { get; set; }
    }

    public class CreateMenuParams
    {
        public CreateMenuParams() { button = new List<MenuButton>(); }
        public class MenuButton
        {
            public string type { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }

        public List<MenuButton> button { get; set; }
    }
}
