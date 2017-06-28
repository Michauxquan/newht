using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudSalesBusiness.Manage;

namespace YXManage.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (CurrentUser == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.Menus = CurrentUser.Menus.Where(x => x.PCode == "100000000").OrderBy(x => x.Sort).ToList();
            string beginTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            ViewBag.LogRPT = ClientBusiness.GetClientsLoginReport(1, beginTime, beginTime);
            ViewBag.GrowRPT = ClientBusiness.GetClientsGrow(1, beginTime, beginTime).FirstOrDefault();
            ViewBag.OrderNum = ClientOrderBusiness.GetClientOrdersCount(-1, beginTime, beginTime);
            ViewBag.FeedNum = FeedBackBusiness.GetFeedBacksCount("", "", 1);
            ViewBag.FeedAllNum = FeedBackBusiness.GetFeedBacksCount(beginTime, beginTime,-1);

            return View();
        }

        public ActionResult Login()
        {
            if (CurrentUser != null)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public ActionResult Logout()
        {
            CurrentUser = null;
            return Redirect("/Home/Login");
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public JsonResult UserLogin(string userName, string pwd)
        {
            bool bl = false;

            string operateip = string.IsNullOrEmpty(Request.Headers.Get("X-Real-IP")) ? Request.UserHostAddress : Request.Headers["X-Real-IP"];
            int result = 0; 
            CloudSalesEntity.Manage.M_Users model = CloudSalesBusiness.M_UsersBusiness.GetM_UserByProUserName(userName, pwd, operateip,out result);
            
            if (model != null)
            {
                CurrentUser = model;
                Session["Manager"] = model;     
                bl = true;
            }
            JsonDictionary.Add("result", bl);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}
