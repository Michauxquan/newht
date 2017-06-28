using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 
using OWZXBusiness;
using OWZXEntity;
using OWZXBusiness.Manage;
using OWZXEntity.Manage;
using OWZXEnum;

namespace OWZXManage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["ClientManager"] == null)
            {
                return Redirect("/Home/Login");
            }
            return Redirect("/Default/Index");
        }

        public ActionResult Register(string ReturnUrl = "")
        {
            string loginUrl = "/Default/Index";
            ViewBag.LoginUrl = !string.IsNullOrEmpty(ReturnUrl) ? ReturnUrl.Replace("&", "%26") : loginUrl; 
            return View();
        }

        public ActionResult FindPassword()
        {
            return View();
        }

        public ActionResult Login(string ReturnUrl = "", int Status = 0, string name = "", int BindAccountType = 0)
        {

            if (Session["ClientManager"] != null)
            {
                return Redirect("/Default/Index");
            }

            ViewBag.Status = Status;
            ViewBag.BindAccountType = BindAccountType;
            ViewBag.ReturnUrl = ReturnUrl.Replace("&", "%26") + (string.IsNullOrEmpty(name) ? "" : "%26name=" + name).Replace("&", "%26") ?? string.Empty;
            
            HttpCookie cook = Request.Cookies["owzx_user"];
            if (cook != null)
            {
                if (cook["status"] == "1")
                {
                    string operateip = Common.Common.GetRequestIP();
                    int result;
                    OWZXEntity.Manage.M_Users model = M_UsersBusiness.GetM_UserByProUserName(cook["username"], cook["pwd"], operateip, out result);;
                    if (model != null)
                    {
                        Session["ClientManager"] = model;
                        return Redirect("/Default/Index");
                    }
                }
                else
                {
                    ViewBag.UserName = cook["username"];
                }
            }
            ViewBag.IsMobileDevice = OWZXManage.Common.Common.IsMobileDevice();

            return View();
        }

        public ActionResult Logout(int Status = 0)
        {
            HttpCookie cook = Request.Cookies["owzx_user"];
            if (cook != null)
            {
                cook["status"] = "0";
                Response.Cookies.Add(cook);
            }

            Session["ClientManager"] = null;
            if (Status > 0)
            {
                return Redirect("/Home/Login?Status=" + Status);
            }
            return Redirect("/Home/Login");
        }
        public JsonResult GetSign(string ReturnUrl)
        {
            Dictionary<string, object> resultObj = new Dictionary<string, object>();
            resultObj.Add("sign", OWZXManage.Common.Signature.GetSignature(Common.Common.YXAgentID, Common.Common.YXClientID, ReturnUrl));

            return new JsonResult
            {
                Data = resultObj,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult Authorize(string sign, string ReturnUrl)
        {
            if (!string.IsNullOrEmpty(sign) && !string.IsNullOrEmpty(ReturnUrl))
            {
                if (sign.Equals(OWZXManage.Common.Signature.GetSignature(Common.Common.YXAgentID, Common.Common.YXClientID, ReturnUrl), StringComparison.OrdinalIgnoreCase))
                {
                    return View("Login");
                }
            }

            Response.Write("<script>alert('参数有误');location.href='http://www.pcfc28.com';</script>");
            Response.End();
            return View("Login");
        }

        public ActionResult Terms() 
        {
            return View();
        }
          
        //微信账户选择进入方式
        public ActionResult WeiXinSelectLogin()
        {
            if (Session["WeiXinTokenInfo"] == null)
            {
                return Redirect("/Home/Login");
            }

            return View();
        }

        //微信授权地址
        public ActionResult WeiXinLogin(string ReturnUrl)
        {
            ReturnUrl = ReturnUrl.Replace("http://www.pcfc28.com/", "").Replace("https://www.pcfc28.com/", "");
            return Redirect(WeiXin.Sdk.Token.GetAuthorizeUrl(Server.UrlEncode(WeiXin.Sdk.AppConfig.CallBackUrl), Server.UrlEncode(ReturnUrl), OWZXManage.Common.Common.IsMobileDevice()));
        }

        //微信回调地址
        public ActionResult WeiXinCallBack(string code, string state = "")
        {
            string operateip = Common.Common.GetRequestIP();
            var userToken = WeiXin.Sdk.Token.GetAccessToken(code);

            if (string.IsNullOrEmpty(userToken.errcode))
            {
                //var model = OrganizationBusiness.GetUserByOtherAccount(userToken.unionid, "", operateip, (int)EnumAccountType.WeiXin); 
                ////已注册
                //if (model != null)
                //{
                //    //未注销
                //    if (model.Status.Value == 1)
                //    {
                //        Session["ClientManager"] = model;
                //        if (string.IsNullOrEmpty(state))
                //        {
                //            return Redirect("/Default/Index");
                //        }
                //        else
                //        {
                //            state = state.Replace("|", "&").Replace("%23", "&");
                //            return Redirect("http://www.pcfc28.com/" + state);
                //        }
                //    }
                //    else
                //    {
                //        if (model.Status.Value == 9)
                //        {
                //            Response.Write("<script>alert('您的账户已注销,请切换其他账户登录');location.href='/Home/login';</script>");
                //            Response.End();
                //        }
                //        else
                //        {
                //            return Redirect("/Home/Login");
                //        }

                //    }
                //}
                //else
                //{
                //    Response.Write("<script>alert('您的微信账号暂未绑定用户,请使用其他方式登陆.');location.href='/Home/login';</script>");
                //    Response.End(); 
                //}
            }

            return Redirect("/Home/Login"); 

        }

        #region Ajax

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public JsonResult UserLogin(string userName, string pwd, string remember = "")
        {
            Dictionary<string, object> JsonDictionary = new Dictionary<string, object>();  
            string operateip = Common.Common.GetRequestIP(); ;
            int result = 0;
            string msg = "";
            Common.PwdErrorUserEntity pwdErrorUser = null;

            if (Common.Common.CachePwdErrorUsers.ContainsKey(userName)) pwdErrorUser = Common.Common.CachePwdErrorUsers[userName];

            if (pwdErrorUser == null || (pwdErrorUser.ErrorCount < 10 && pwdErrorUser.ForbidTime < DateTime.Now))
            {
                M_Users tempmodel = M_UsersBusiness.GetM_UserByUserName(userName);
                if (tempmodel != null)
                {

                    if (tempmodel.IsFreeZe == 0)
                    {
                        var pswd = OWZXTool.Encrypt.MD5(pwd + tempmodel.Salt);
                        if (pswd == tempmodel.Password)
                        {
                            M_Users model = M_UsersBusiness.GetM_UserByProUserName(userName, pswd, operateip, out result);

                            if (model != null)
                            {
                                HttpCookie cook = new HttpCookie("owzx_user");
                                cook["username"] = userName;
                                cook["pwd"] = pwd;
                                if (remember == "1")
                                {
                                    cook["status"] = remember;
                                }
                                cook.Expires = DateTime.Now.AddDays(7);
                                Response.Cookies.Add(cook);
                                Session["ClientManager"] = model;
                                result = 1;
                            }
                            else
                            {
                                msg = result == 3 ? "用户已被禁闭，请联系管理员" : "用户名或密码错误！";
                            }
                        }
                        else
                        { 
                            result = 3;
                            msg = "用户密码错误！";
                        }
                    }
                    else
                    {
                        result = 4;
                        msg = "用户已被禁闭，请联系管理员" ;
                    }
                }
                else
                {
                    result = 4;
                    msg = "用户名不存在";
                }
                if (!string.IsNullOrEmpty(msg) && result!=4)
                {
                    if (pwdErrorUser == null)
                    {
                        pwdErrorUser = new Common.PwdErrorUserEntity();
                    }
                    else
                    {
                        if (pwdErrorUser.ErrorCount > 9)
                        {
                            pwdErrorUser.ErrorCount = 0;
                        }
                    }

                    pwdErrorUser.ErrorCount++;
                    if (pwdErrorUser.ErrorCount > 6)
                    {
                        pwdErrorUser.ForbidTime = DateTime.Now.AddHours(2);
                        result = 2;
                    }
                    else
                    {
                        JsonDictionary.Add("errorCount", pwdErrorUser.ErrorCount);
                        result = 3;
                    }

                    Common.Common.CachePwdErrorUsers[userName] = pwdErrorUser;
                }
            }
            else
            {
                int forbidTime = (int)(pwdErrorUser.ForbidTime - DateTime.Now).TotalMinutes;
                JsonDictionary.Add("forbidTime", forbidTime);
                result = -1;
            }
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("errorinfo",msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //账号是否存在
        public JsonResult IsExistLoginName(string loginName)
        {
            bool bl = true;//OrganizationBusiness.IsExistLoginName(loginName);
            Dictionary<string, object> JsonDictionary = new Dictionary<string, object>();
            JsonDictionary.Add("Result", bl?1:0);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

      
        //发送手机验证码
        public JsonResult SendMobileMessage(string mobilePhone)
        {
            Dictionary<string, object> JsonDictionary = new Dictionary<string, object>();
            Random rd = new Random();
            int code=rd.Next(100000, 1000000);

            bool flag = Common.MessageSend.SendMessage(mobilePhone, code);
            JsonDictionary.Add("Result",flag?1:0);

            if (flag) 
            {
                Common.Common.SetCodeSession(mobilePhone, code.ToString());

                Common.Common.WriteAlipayLog(mobilePhone + " : " + code.ToString());
                
            }

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //验证手机验证码
        public JsonResult ValidateMobilePhoneCode(string mobilePhone, string code)
        {
            bool bl = Common.Common.ValidateMobilePhoneCode(mobilePhone, code);
            Dictionary<string, object> JsonDictionary = new Dictionary<string, object>();
            JsonDictionary.Add("Result", bl ? 1 : 0);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        } 
        #endregion

    }
}
