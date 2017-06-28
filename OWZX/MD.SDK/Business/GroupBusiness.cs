using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MD.SDK
{
    public class GroupBusiness
    {
        public static GrouptList GetMyJoined(string token)
        {
            var paras = new Dictionary<string, object>();
            paras.Add("access_token", token);
            var result = HttpRequest.RequestServer(ApiOption.group_my_joined, paras);

            return JsonConvert.DeserializeObject<GrouptList>(result);
        }
    }
}
