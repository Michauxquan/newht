using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OWZXBusiness;
using OWZXEntity.Manage;

namespace OWZXManage.Controllers
{
    public class WebUserController : BaseController
    {
        //
        // GET: /WebUser/

        public ActionResult WebUser()
        {
            return View();
        }
        public ActionResult UsersLog(string id="")
        {
            ViewBag.Uid = id;
            return View();
        }

        #region Ajax
        #region WebUser
        public JsonResult GetUsers(string keyWords, int pageIndex, int status = -1, int sourcetype = 1)
        {
            int totalCount = 0, pageCount = 0;
            var list = M_UsersBusiness.GetUsers(PageSize, pageIndex, ref totalCount, ref pageCount, sourcetype, status, keyWords);

            JsonDictionary.Add("Items", list);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUserDetail(int id)
        {
            var item = M_UsersBusiness.GetUserDetail(id);

            JsonDictionary.Add("Item", item);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult ValidateLoginName(string loginName)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonDictionary.Add("Info", M_UsersBusiness.GetM_UserCountByLoginName(loginName) > 0 ? "登录名已存在" : "");
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveUser(string entity)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            M_Users model = serializer.Deserialize<M_Users>(entity);
            string mes = "执行成功";
            JsonDictionary.Add("errmeg", "执行成功");
            if (model.Uid < 1)
            {
                if (M_UsersBusiness.GetM_UserCountByLoginName(model.UserName) == 0)
                {
                    model.Salt = OWZXTool.Encrypt.CreateRandomValue(6, true);
                    model.Password = OWZXTool.Encrypt.MD5(model.Salt);
                    model.IsAdmin = 0;
                    model.AdminGid = 2;
                    model.IsFreeZe = 0;
                    model.Uid = M_UsersBusiness.CreateM_User(model);
                }
                else { JsonDictionary["errmeg"] = "登录名已存在,操作失败"; }
            }
            else
            {
                bool bl = M_UsersBusiness.UpdateM_User(model);
                M_UsersBusiness.UpdatePartUser(model);
                if (!bl)
                {
                    model.Uid = 0;
                }
            }
            if (model.Uid < 1) { JsonDictionary["errmeg"] = "操作失败"; }
            JsonDictionary.Add("model", model);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DeleteMUser(int id)
        {

            bool bl = M_UsersBusiness.DeleteM_User(id, 9);
            JsonDictionary.Add("status", (bl ? 1 : 0));
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateUserStatus(int id, int status)
        {

            bool bl = M_UsersBusiness.UpdateM_UserStatus(id, status);
            JsonDictionary.Add("status", (bl ? 1 : 0));
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        } 
        #endregion

        #region UsersLog

        public JsonResult UserLogsList(string keyWords, int pageIndex,int uid=-1)
        {
            int totalCount = 0, pageCount = 0;
            var list = LogBusiness.UsersLogList(PageSize, pageIndex, ref totalCount, ref pageCount, keyWords, uid);

            JsonDictionary.Add("Items", list);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion
        #endregion
    }
}
