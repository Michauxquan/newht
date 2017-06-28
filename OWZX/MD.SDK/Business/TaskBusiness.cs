using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MD.SDK
{
    public class TaskBusiness
    {
        public static string AddTask(string token, string title, string ownerID, List<string> memberIDs, string endDate, string folderID, string des, out int errorCode)
        {
            errorCode = 0;
            var paras = new Dictionary<string, object>();
            paras.Add("access_token", token);
            paras.Add("t_title", title);
            paras.Add("u_id", ownerID);
            paras.Add("t_mids", string.Join(",", memberIDs.ToArray()));
            paras.Add("t_ed", endDate);
            paras.Add("t_folderID", folderID);
            paras.Add("t_des", des);
            var result = HttpRequest.RequestServer(ApiOption.task_v4_addTask, paras,RequestType.Post);

            JObject resultObj = (JObject)JsonConvert.DeserializeObject(result);
            if (resultObj != null)
            {
                if (resultObj.Property("error_code") == null)
                    return resultObj["task"].ToString();
                else
                    errorCode = int.Parse(resultObj["error_code"].ToString());
            }

            return string.Empty;

        }
    }
}
