using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MD.SDK
{
    public  class PostBusiness
    {
        public static PostList GetPostAll(string token, string keywords, PostType postType, string maxID, int pagesize=20)
        {
            var paras = new Dictionary<string, object>();
            paras.Add("access_token", token);
            paras.Add("keywords", keywords);
            paras.Add("post_type",(int) postType);
            paras.Add("max_id", maxID);
            paras.Add("pagesize", pagesize);
            var result = HttpRequest.RequestServer(ApiOption.post_v2_all, paras);

            return JsonConvert.DeserializeObject<PostList>(result);
        }

        public static PostEntity GetPostAll(string token, string pID)
        {
            var paras = new Dictionary<string, object>();
            paras.Add("access_token", token);
            paras.Add("p_id", pID);
            var result = HttpRequest.RequestServer(ApiOption.post_v2_detail, paras);

            return JsonConvert.DeserializeObject<PostEntity>(result);
        }

        public static string Update(string token, string msg, string gID, PostShareType shareType, PostType postType, out int errorCode)
        {
            errorCode = 0;
            var paras = new Dictionary<string, object>();
            paras.Add("access_token", token);
            paras.Add("p_msg", msg);
            paras.Add("g_id", gID);
            paras.Add("s_type", (int)shareType);
            paras.Add("p_type", (int)postType);
            var result = HttpRequest.RequestServer(ApiOption.post_update, paras);

            JObject resultObj =(JObject) JsonConvert.DeserializeObject(result);
            if (resultObj != null)
            {
                if (resultObj.Property("error_code") == null)
                    return resultObj["post"].ToString();
                else
                    errorCode =int.Parse( resultObj["error_code"].ToString());
            }

            return string.Empty;
            
        }
    }

    public enum PostType
    {
        All=-1,
        Normal =0,
        Link = 1,
        Pic = 2,
        Doc = 3,
        QA = 4,
        Vote = 7,
        Video = 8
    }
    
    public enum PostShareType{
        All=0,
        Group=1,
        GroupAndFllowed=2,
        MySelf=3
    }
}
