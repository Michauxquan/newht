using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Mvc;

using OWZXBusiness;

using OWZXEntity.Manage;
using OWZXBusiness.Manage;

namespace OWZXManage.Controllers
{
    public class DefaultController : BaseController
    {
        //
        // GET: /Default/

        public ActionResult Index(string href = "", string name = "")
        { 
            ViewBag.Herf = string.IsNullOrEmpty(href) ? "" : href + (string.IsNullOrEmpty(name) ? "" : "&name=" + name);

            if (OWZXManage.Common.Common.IsMobileDevice())
            {
                return Redirect("/M/Home/Index");
            }
            return View();
        }

        public ActionResult LeftMenu(string id)
        {
            ViewBag.MenuCode = id;
            return PartialView();
        }
    
        public ActionResult Home()
        { 

            return View();
        }
         

    }
}
