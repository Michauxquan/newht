using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace WeiXin.Sdk
{
    public class AppConfig
    {
        public static string WeiXinApiUrl = ConfigurationManager.AppSettings["WeiXinApiUrl"] ?? "https://api.weixin.qq.com";
        public static string AppKey = ConfigurationManager.AppSettings["WeiXinAppKey"] ?? "wx58f43bd3f3df6bef";
        public static string AppSecret = ConfigurationManager.AppSettings["WeiXinAppSecret"] ?? "a9ec35a6090289ff5e4f173ba5a00866";
        public static string CallBackUrl = ConfigurationManager.AppSettings["WeiXinCallBackUrl"] ?? "localhost:8888/Home/WeiXinCallBack";
        public static string BindCallBackUrl = ConfigurationManager.AppSettings["WeiXinBindCallBackUrl"] ?? "localhost:8888/MyAccount/WeiXinCallBack";
    }
}
