using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using OWZXBusiness;
using OWZXBusiness.Manage;
using OWZXEntity.Manage;

namespace OWZXManage.Controllers
{
    public class SysSetController : BaseController
    { 
        public ActionResult Users()
        {
            ViewBag.Roles = ManageSystemBusiness.GetRoles();
            return View();
        }
         

        public ActionResult Role()
        {
            return View();
        }
        public ActionResult RolePermission(string id)
        {
            ViewBag.Model = ManageSystemBusiness.GetRoleByID(id);
            ViewBag.Menus = CommonBusiness.ManageMenus.Where(m => m.PCode == ExpandClass.CLIENT_TOP_CODE).ToList();
            return View();
        }  

        #region Ajax

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoles()
        {
            var list = ManageSystemBusiness.GetRoles();
            JsonDictionary.Add("items", list);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetRoleByID(string id)
        {
            var model = ManageSystemBusiness.GetRoleByID(id);
            JsonDictionary.Add("model", model);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult SaveRole(string entity)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            M_Role model = serializer.Deserialize<M_Role>(entity);

            if (string.IsNullOrEmpty(model.RoleID))
            {
                model.RoleID = new ManageSystemBusiness().CreateRole(model.Name, model.Description, string.Empty);
            }
            else
            {
                bool bl = new ManageSystemBusiness().UpdateRole(model.RoleID, model.Name, model.Description, string.Empty);
                if (!bl)
                {
                    model.RoleID = "";
                }
            }
            JsonDictionary.Add("model", model);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public JsonResult DeleteRole(string roleid)
        {
            int result = 0;
            bool bl = new ManageSystemBusiness().DeleteRole(roleid, CurrentUser.Uid, OperateIP, out result);
            JsonDictionary.Add("status", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public JsonResult SaveRolePermission(string roleid, string permissions)
        {
            if (permissions.Length > 0)
            {
                permissions = permissions.Substring(0, permissions.Length - 1);

            }
            bool bl = new ManageSystemBusiness().UpdateRolePermission(roleid, permissions, CurrentUser.Uid, OperateIP);
            JsonDictionary.Add("status", bl);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUsers(string keyWords, int pageIndex,int status=-1,int sourcetype=2)
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
        /// <summary>
        /// 新增或修改用户
        /// </summary>
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
            if (model.Uid<1)
            {
                if (M_UsersBusiness.GetM_UserCountByLoginName(model.UserName) == 0)
                {
                    model.Salt = OWZXTool.Encrypt.CreateRandomValue(6,true);
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
            if (model.Uid<1) { JsonDictionary["errmeg"] = "操作失败"; }
            JsonDictionary.Add("model", model);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DeleteMUser(int id)
        {

            bool bl =  M_UsersBusiness.DeleteM_User(id, 9);
            JsonDictionary.Add("status", (bl ? 1 : 0));
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateUserStatus(int id,int status)
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
    }
}
