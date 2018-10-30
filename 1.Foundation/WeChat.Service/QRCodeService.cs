using Newtonsoft.Json;

namespace WeChat.Service
{
    public class QRCodeService
    {
        public static string CreateQRCode()
        {
            string accessToken = AccessTokenService.GetAccessToken();

            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + accessToken;

            var qrCodeParams = new CreateQRCodeParams()
            {
                action_name = "QR_LIMIT_SCENE",
                action_info = new Scene()
                {
                    scene_id = "1803"
                }
            };

            string parameters = JsonConvert.SerializeObject(qrCodeParams);            
            return AppService.WebRequestPost(url, parameters);            
            
            //return (CreateQRCodeResponse)JsonConvert.DeserializeObject(response, typeof(CreateQRCodeResponse));
        }
    }
}

public class CreateQRCodeParams
{
    public string action_name { get; set; }
    public Scene action_info { get; set; }
}

public class Scene
{
    public string scene_id { get; set; }
}

public class CreateQRCodeResponse
{
    public string Ticket { get; set; }
    public string Expire_Seconds { get; set; }
    public string Url { get; set; }
}
