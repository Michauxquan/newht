using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Configuration;
namespace MD.SDK
{
    public class HttpRequest
    {
        public static string RequestServer(ApiOption apiOption, Dictionary<string, object> paras,RequestType requestType=RequestType.Get)
        {
            string url =AppAttr.MDApiUrl+ApiUrlArr[(int)apiOption];
            try
            {
                string paraStr = string.Empty;
                if (paras != null && paras.Count > 0)
                {
                    foreach (string key in paras.Keys)
                    {
                        if (string.IsNullOrEmpty(paraStr))
                            paraStr = key + "=" + paras[key];
                        else
                            paraStr += "&" + key + "=" + paras[key];
                    }

                    paraStr += "&format=json";
                }
                else
                    paraStr = "format=json";

                HttpWebRequest request;
                HttpWebResponse response;
                string strResult = string.Empty;
                if (requestType == RequestType.Get)
                {
                    url += "?" + paraStr;
                    request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = 10000;
                    request.Method = "GET";
                    request.ContentType = "application/x-www-form-urlencoded";

                    response = (HttpWebResponse)request.GetResponse();
                    System.Text.Encoding encode = Encoding.ASCII;
                    if (response.CharacterSet.Contains("utf-8"))
                        encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encode);
                    strResult = reader.ReadToEnd();

                    reader.Close();
                    response.Close();
                }
                else
                {
                    byte[] bData = Encoding.UTF8.GetBytes(paraStr.ToString());

                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.Timeout = 5000;
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = bData.Length;

                    System.IO.Stream smWrite = request.GetRequestStream();
                    smWrite.Write(bData, 0, bData.Length);
                    smWrite.Close();

                    response = (HttpWebResponse)request.GetResponse();
                    System.IO.Stream dataStream = response.GetResponseStream();
                    System.IO.StreamReader reader = new System.IO.StreamReader(dataStream, Encoding.UTF8);
                    strResult = reader.ReadToEnd();

                    reader.Close();
                    dataStream.Close();
                    response.Close(); 
                }

                return strResult;
            }
            catch { }

            return null;
        }

        public static string[] ApiUrlArr = new string[] { 
        "/oauth2/access_token",

        "/user/all",
        "/user/detail",
        "/passport/detail",

        "/post/v2/all",
        "/post/v2/detail",
        "/post/update",


        "/group/my_joined",

        "/message/create_sys",

        "/task/v4/addTask",

        "/calendar/create",

        "/app/is_admin"
        };
    }

    public enum RequestType
    {
        Get = 1,
        Post = 2
    }
}
