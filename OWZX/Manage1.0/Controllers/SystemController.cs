using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CloudSalesBusiness.Manage;
using System.Web.Script.Serialization;
using CloudSalesEntity;
using CloudSalesEntity.Manage;
using CloudSalesBusiness;
namespace YXManage.Controllers
{
     [YXManage.Common.UserAuthorize]
    public class SystemController : BaseController
    {
        #region view
         public ActionResult Admin()
        {
            return View();
        }
         public ActionResult Users()
         {
             ViewBag.Roles = ManageSystemBusiness.GetRoles();
             return View();
         }
         public ActionResult Roles()
         {
             return View();
         }
         public ActionResult Index()
         {
             return View();
         }

         public ActionResult Detail(string id)
         {
             ViewBag.Modules = CloudSalesBusiness.ModulesBusiness.GetModules();
             if (string.IsNullOrEmpty(id))
                 ViewBag.ID = 0;
             else
                 ViewBag.ID = int.Parse(id);

             return View();
         }

         public ActionResult RolePermission(string id)
         {
             ViewBag.Model = ManageSystemBusiness.GetRoleByID(id);
             ViewBag.Menus = CommonBusiness.ManageMenus.Where(m => m.PCode == ExpandClass.CLIENT_TOP_CODE).ToList();
             return View();
         }
         #region ExpressView
         public ActionResult ExpressIndex()
         {
             return View();
         }

         public ActionResult ExpressDetail(string id)
         {
             ViewBag.ExpressID = id;

             return View();
         }
         #endregion
         #region IndustryView
         public ActionResult IndustryIndex()
         {
             return View();
         }

         public ActionResult IndustryDetail(string id)
         {
             ViewBag.IndustryID = id;

             return View();
         }
         #endregion
        #endregion

        #region ajax

         #region ajax 产品
         public JsonResult GetModulesProducts(int pageIndex, string keyWords)
         {
             int totalCount = 0, pageCount = 0;
             var list = ModulesProductBusiness.GetModulesProducts(keyWords, PageSize, pageIndex, ref totalCount, ref pageCount);
             JsonDictionary.Add("Items", list);
             JsonDictionary.Add("TotalCount", totalCount);
             JsonDictionary.Add("PageCount", pageCount);

             return new JsonResult()
             {
                 Data = JsonDictionary,
                 JsonRequestBehavior = JsonRequestBehavior.AllowGet
             };
         }

         public JsonResult GetModulesProductDetail(int id)
         {
             var item = ModulesProductBusiness.GetModulesProductDetail(id);
             JsonDictionary.Add("Item", item);
             JsonDictionary.Add("Result", 1);
             return new JsonResult()
             {
                 Data = JsonDictionary,
                 JsonRequestBehavior = JsonRequestBehavior.AllowGet
             };
         }

         public JsonResult SaveModulesProduct(string modulesProduct)
         {
             JavaScriptSerializer serializer = new JavaScriptSerializer();
             ModulesProduct model = serializer.Deserialize<ModulesProduct>(modulesProduct);

             bool flag = false;
             if (model.AutoID == 0)
             {
                 model.CreateUserID = string.Empty;
                 flag = ModulesProductBusiness.InsertModulesProduct(model);
             }
             else
             {
                 model.CreateUserID = string.Empty;
                 flag = ModulesProductBusiness.UpdateModulesProduct(model);
             }
             JsonDictionary.Add("Result", flag ? 1 : 0);

             return new JsonResult()
             {
                 Data = JsonDictionary,
                 JsonRequestBehavior = JsonRequestBehavior.AllowGet
             };
         }

         public JsonResult DeleteModulesProduct(int id)
         {
             bool flag = ModulesProductBusiness.DeleteModulesProduct(id);
             JsonDictionary.Add("Result", flag ? 1 : 0);
             return new JsonResult()
             {
                 Data = JsonDictionary,
                 JsonRequestBehavior = JsonRequestBehavior.AllowGet
             };
         }
         #endregion

        public JsonResult GetAdminDetail()
        {
            JsonDictionary.Add("LoginName", CurrentUser.LoginName);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ConfirmAdminPwd(string pwd)
        {
            CloudSalesEntity.Manage.M_Users model = CloudSalesBusiness.M_UsersBusiness.GetM_UserByUserName(CurrentUser.LoginName, pwd, string.Empty);
            JsonDictionary.Add("Result", model!=null?1:0);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SetAdminAccount(string loginName, string newPwd)
        {
            bool flag = CloudSalesBusiness.M_UsersBusiness.SetAdminAccount(CurrentUser.UserID, loginName, newPwd);
            
            JsonDictionary.Add("Result", flag ? 1 : 0);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }



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
                model.RoleID = new ManageSystemBusiness().CreateRole(model.Name, model.ParentID, model.Description, string.Empty);
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
            bool bl = new ManageSystemBusiness().DeleteRole(roleid, CurrentUser.UserID, OperateIP, out result);
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
            bool bl = new ManageSystemBusiness().UpdateRolePermission(roleid, permissions, CurrentUser.UserID, OperateIP);
            JsonDictionary.Add("status", bl);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUsers(string keyWords, int pageIndex)
        {
            int totalCount = 0, pageCount = 0;
            var list = M_UsersBusiness.GetUsers(keyWords, string.Empty, PageSize, pageIndex, ref totalCount, ref pageCount);

            JsonDictionary.Add("Items", list);
            JsonDictionary.Add("TotalCount", totalCount);
            JsonDictionary.Add("PageCount", pageCount);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUserDetail(string id)
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
            JsonDictionary.Add("errmeg", "执行成功");
            if (string.IsNullOrEmpty(model.UserID))
            {
                if (M_UsersBusiness.GetM_UserCountByLoginName(model.LoginName) == 0)
                {
                    model.CreateUserID = CurrentUser.UserID;
                    model.UserID = new M_UsersBusiness().CreateM_User(model);
                }
                else { JsonDictionary["errmeg"] = "登录名已存在,操作失败"; }
            }
            else
            {
                bool bl = new M_UsersBusiness().UpdateM_User(model.UserID, model.Name, model.RoleID, model.Email, model.MobilePhone, model.OfficePhone, model.Jobs, model.Avatar, model.Description);
                if (!bl)
                {
                    model.UserID = "";
                }
            }
            if (string.IsNullOrEmpty(model.UserID)) { JsonDictionary["errmeg"] = "操作失败"; }
            JsonDictionary.Add("model", model);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult DeleteMUser(string id)
        {

            bool bl = (new M_UsersBusiness()).DeleteM_User(id, 9);
            JsonDictionary.Add("status", (bl ? 1 : 0));
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        #region ExpressAjax
        /// <summary>
        /// 快递公司列表
        /// </summary>
        public JsonResult GetExpressCompanys(int pageIndex, string keyWords)
        {
            int totalCount = 0, pageCount = 0;
            var list = ExpressCompanyBusiness.GetExpressCompanys(keyWords, PageSize, pageIndex, ref totalCount, ref pageCount);
            JsonDictionary.Add("Items", list);
            JsonDictionary.Add("TotalCount", totalCount);
            JsonDictionary.Add("PageCount", pageCount);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 快递公司详情
        /// </summary>
        public JsonResult GetExpressCompanyDetail(string id)
        {
            var item = ExpressCompanyBusiness.GetExpressCompanyDetail(id);
            JsonDictionary.Add("Item", item);
            JsonDictionary.Add("Result", 1);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 保存快递公司
        /// </summary>
        public JsonResult SaveExpressCompany(string expressCompany)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ExpressCompany model = serializer.Deserialize<ExpressCompany>(expressCompany);

            bool flag = false;
            if (model.AutoID == 0)
            {
                model.CreateUserID = string.Empty;
                flag = ExpressCompanyBusiness.InsertExpressCompany(model);
            }
            else
            {
                model.CreateUserID = string.Empty;
                flag = ExpressCompanyBusiness.UpdateExpressCompany(model);
            }
            JsonDictionary.Add("Result", flag ? 1 : 0);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 删除快递公司
        /// </summary>
        public JsonResult DeleteExpressCompany(string id)
        {
            bool flag = ExpressCompanyBusiness.DeleteExpressCompany(id);
            JsonDictionary.Add("Result", flag ? 1 : 0);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
        #region IndustryAjax
        /// <summary>
        /// 获取行业列表
        /// </summary>
        public JsonResult GetIndustrys(string keyWords)
        {
            var list = IndustryBusiness.GetIndustrys();
            if (!string.IsNullOrEmpty(keyWords))
            {
                list = list.FindAll(m => m.Name.Contains(keyWords));
            }
            JsonDictionary.Add("Items", list);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 获取行业详情
        /// </summary>
        public JsonResult GetIndustryDetail(string id)
        {
            var item = IndustryBusiness.GetIndustryDetail(id);
            JsonDictionary.Add("Item", item);
            JsonDictionary.Add("Result", 1);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 保存行业
        /// </summary>
        public JsonResult SaveIndustry(string industry)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Industry model = serializer.Deserialize<Industry>(industry);

            bool flag = false;
            if (string.IsNullOrEmpty(model.IndustryID))
            {
                model.CreateUserID = string.Empty;
                flag = !string.IsNullOrEmpty(IndustryBusiness.InsertIndustry(model.Name, model.Description, CurrentUser.UserID, string.Empty)) ? true : false;
            }
            else
            {
                model.CreateUserID = string.Empty;
                flag = IndustryBusiness.UpdateIndustry(model.IndustryID, model.Name, model.Description);
            }
            JsonDictionary.Add("Result", flag ? 1 : 0);

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
