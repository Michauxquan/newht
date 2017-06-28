using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MD.SDK
{
    public class AppAttr
    {
        public static string MDApiUrl = ConfigurationManager.AppSettings["MDApiUrl"] ?? "https://api.mingdao.com";
        public static string AppKey = ConfigurationManager.AppSettings["AppKey"] ?? "B0FEFE749FFC";
        public static string AppSecret = ConfigurationManager.AppSettings["AppSecret"] ?? "73A16FE437EF3C73D283E57C5D78ACBA";
        public static string CallBackUrl = ConfigurationManager.AppSettings["CallBackUrl"] ?? "http://www.sh-yunxiao.com/Home/CallBack";
    }
}
