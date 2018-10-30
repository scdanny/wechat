using System;
using System.IO;
using System.Net;
using System.Text;

namespace WeChat.Service
{
    public class AppService
    {
        // Methods
        public static string HttpRequestGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Get";
            request.Timeout = 0x1d4c0;
            using (WebResponse response = request.GetResponse())
            {
                string str;
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                StringBuilder builder = new StringBuilder();
                while ((str = reader.ReadLine()) != null)
                {
                    builder.Append(str + Environment.NewLine);
                }
                reader.Close();
                responseStream.Close();
                return builder.ToString();
            }
        }        

        public static DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime time = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            long ticks = long.Parse(timeStamp + "0000000");
            TimeSpan span = new TimeSpan(ticks);
            return time.Add(span);
        }

        public static string WebRequestPost(string url, string param)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(param);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.Timeout = 0x1d4c0;
            request.ContentType = "application/x-www-form-urlencoded;";
            request.ContentLength = bytes.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }
            using (WebResponse response = request.GetResponse())
            {
                string str;
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                StringBuilder builder = new StringBuilder();
                while ((str = reader.ReadLine()) != null)
                {
                    builder.Append(str + Environment.NewLine);
                }
                reader.Close();
                responseStream.Close();
                return builder.ToString();
            }
        }
    }
}
