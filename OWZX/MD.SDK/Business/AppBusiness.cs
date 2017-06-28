using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MD.SDK.Entity.App
{
    public class AppBusiness
    {
        public static bool IsAppAdmin(string token,string uID, out int errorCode)
        {
            errorCode = 0;
            var paras = new Dictionary<string, object>();
            paras.Add("access_token", token);
            paras.Add("u_id", uID);
            var result = HttpRequest.RequestServer(ApiOption.app_is_admin, paras);

            JObject resultObj = (JObject)JsonConvert.DeserializeObject(result);
            if (resultObj != null)
            {
                if (resultObj.Property("error_code") == null)
                {
                    var count =int.Parse( resultObj["count"].ToString());
                    return count > 0;
                }
                else
                    errorCode = int.Parse(resultObj["error_code"].ToString());
            }
            return false; 

        }
    }
}
