﻿using CloudSalesTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YXManage.Controllers
{
    public class BaseController : Controller
    {
        protected string ClientID = AppSettings.Settings["ClientID"];

        protected int PageSize = 20;

        protected Dictionary<string, object> JsonDictionary = new Dictionary<string, object>();
        /// <summary>
        /// 登录IP
        /// </summary>
        protected string OperateIP
        {
            get
            {
                return string.IsNullOrEmpty(Request.Headers.Get("X-Real-IP")) ? Request.UserHostAddress : Request.Headers["X-Real-IP"];
            }
        }
        protected CloudSalesEntity.Manage.M_Users CurrentUser
        {
            get
            {
                if (Session["Manager"] == null)
                {
                    return null;
                }
                else
                {
                    return (CloudSalesEntity.Manage.M_Users)Session["Manager"];
                }
            }
            set { Session["Manager"] = value; }
        }
        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool IsLimits(string menucode)
        {
            if (Session["Manager"] != null)
            {
                CloudSalesEntity.Manage.M_Users model = (CloudSalesEntity.Manage.M_Users)Session["Manager"];
                if (model.Menus.Where(m => m.MenuCode == menucode).Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
