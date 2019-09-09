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

        public static string WebRequestPostJson(string url, string param)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(param);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "Post";
            request.Timeout = 0x1d4c0;
            request.ContentType = "application/json";
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


        public static string WebRequestPost(string url,string fileName,string contentType,byte[] media)
        {
           
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.MaximumResponseHeadersLength = 1024;
            request.Method = "Post";
            request.Timeout = 0x1d4c0;
            //request.ContentType = "multipart/form-data;";
            //request.ContentLength = media.Length;

            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"media\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            //StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"media\";filename=\"{0}\"\r\nContent-Type:\"{1}\"\r\n\r\n", fileName,contentType));

            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            //string output = "boundary:"+ boundary + "\r\n";
            //output += "itemBoundaryBytes:" + itemBoundaryBytes.ToString() + "\r\n";
            //output += "postHeaderBytes:" + postHeaderBytes.ToString() + "\r\n";
            //output += "endBoundaryBytes:" + endBoundaryBytes.ToString();
            //return output;

            using (Stream stream = request.GetRequestStream())
            {

                stream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                stream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                stream.Write(media, 0, media.Length);
                stream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                //stream.Flush();
                //stream.Close();
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
