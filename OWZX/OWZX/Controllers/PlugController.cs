using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using OWZXBusiness;
using System.IO;
using OWZXEnum;
using System.Web.Script.Serialization;
using OWZXEntity;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.RPC;
using Qiniu.RS;

namespace OWZXManage.Controllers
{
    public class PlugController : BaseController
    {
        public JsonResult GetToken()
        {
            return new JsonResult()
            {
                Data = Common.Common.GetQNToken(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public int DeleteAttachment(string key)
        {
            Config.Init(); 
            //实例化一个RSClient对象，用于操作BucketManager里面的方法
            RSClient client = new RSClient();
            CallRet ret = client.Delete(new EntryPath(Common.Common.bucket, key));

            return ret.OK ? 1 : 0;
        }
        /// <summary>
        /// 支持批量上传  
        /// </summary>
        /// <param name="filepath">格式 英文逗号分割 A,B,C </param>
        /// <param name="file">文件夹名车 例如 产品product 订单 orders</param>
        /// <returns>图片地址A,图片地址B,图片地址C</returns>
        public string UploadAttachment(string filepath,string file)
        {
           return  Common.Common.UploadAttachment(filepath, file);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadFile()
        {
            string oldPath = "",
                   folder = OWZXTool.AppSettings.Settings["UploadTempPath"], 
                   action = "";
            if (Request.Form.AllKeys.Contains("oldPath"))
            {
                oldPath = Request.Form["oldPath"];
            }
            if (Request.Form.AllKeys.Contains("folder") && !string.IsNullOrEmpty(Request.Form["folder"]))
            {
                folder = Request.Form["folder"];
            }
            string uploadPath = HttpContext.Server.MapPath(folder);

            if (Request.Form.AllKeys.Contains("action"))
            {
                action = Request.Form["action"];
            }
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            List<string> list = new List<string>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                //判断图片类型
                string ContentType = file.ContentType;
                Dictionary<string, string> types = new Dictionary<string, string>();
                types.Add("image/x-png", "1");
                types.Add("image/png", "1");
                types.Add("image/gif", "1");
                types.Add("image/jpeg", "1");
                //types.Add("image/tiff", "1");
                types.Add("application/x-MS-bmp", "1");
                types.Add("image/pjpeg", "1");
                if (!types.ContainsKey(ContentType))
                {
                    continue;
                }
                if (file.ContentLength > 1024 * 1024 * 10)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(oldPath) && oldPath != "/modules/images/default.png" && new FileInfo(HttpContext.Server.MapPath(oldPath)).Exists)
                {
                    file.SaveAs(HttpContext.Server.MapPath(oldPath));
                    list.Add(oldPath);
                }
                else 
                {
                    string[] arr = file.FileName.Split('.');
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmssms") + new Random().Next(1000, 9999).ToString() + "." + arr[arr.Length - 1];
                    string filePath = uploadPath + fileName;
                    file.SaveAs(filePath);
                    list.Add(folder + fileName);
                }
            }

            JsonDictionary.Add("Items", list);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
 
    }
}
