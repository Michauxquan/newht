using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using OWZXEntity.Manage;
using OWZXDAL.Manage;
using OWZXBusiness.Manage;
using OWZXEntity;


namespace OWZXBusiness
{
   

    public class M_UsersBusiness
    {
         #region 查询
        /// <summary>
        /// 根据账号密码获取信息（登录）
        /// </summary>
        /// <param name="loginname">账号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static M_Users GetM_UserByUserName(string loginname, string pwd, string operateip)
        { 
            DataTable dt = new M_UsersDAL().GetM_UserByUserName(loginname, pwd);
            M_Users model = null;
            if (dt.Rows.Count > 0)
            {
                model = new M_Users();
                model.FillData(dt.Rows[0]);
            }
             return model;
        }
        /// <summary>
        /// 根据账号密码获取信息（登录）
        /// </summary>
        /// <param name="loginname">账号</param> 
        /// <returns></returns>
        public static M_Users GetM_UserByUserName(string loginname)
        {
            DataTable dt = new M_UsersDAL().GetM_UserByLoginName(loginname);
            M_Users model = null;
            if (dt.Rows.Count > 0)
            {
                model = new M_Users();
                model.FillData(dt.Rows[0]);
            } 
            return model;
        }
        /// <summary>
        /// 根据账号密码获取信息（登录）
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="pwd"></param>
        /// <param name="operateip"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static M_Users GetM_UserByProUserName(string loginname, string pwd, string operateip,out int result, int uid=0)
        { 
            DataSet ds = new M_UsersDAL().GetM_UserByProUserName(loginname, pwd, out result);
            M_Users model = null;
            if (ds.Tables.Contains("M_User") && ds.Tables["M_User"].Rows.Count > 0)
            {
                model = new M_Users();
                model.FillData(ds.Tables["M_User"].Rows[0]);
                if (!string.IsNullOrEmpty(model.RoleID))
                    model.Role = ManageSystemBusiness.GetRoleByIDCache(model.RoleID);
                //权限
                if (model.Role != null && model.Role.IsDefault == 1)
                {
                    model.Menus = CommonBusiness.ManageMenus;
                }
                else if (model.IsAdmin == 1)
                {
                    model.Menus = CommonBusiness.ManageMenus;
                }
                else
                {
                    model.Menus = new List<Menu>();
                    foreach (DataRow dr in ds.Tables["Permission"].Rows)
                    {
                        Menu menu = new Menu();
                        menu.FillData(dr);
                        model.Menus.Add(menu);
                    }
                }
            }
            //记录登录日志
            LogBusiness.AddLoginLog(model.Uid, 0, operateip, "", "管理员登陆");

            return model;
        }
        public static int GetM_UserCountByLoginName(string loginname)
        {
            DataTable dt = new M_UsersDAL().GetM_UserByLoginName(loginname);
            return dt.Rows.Count;
        }
        public static M_Users GetUserDetail(int userID)
        {
            DataTable dt = M_UsersDAL.BaseProvider.GetUserDetail(userID);

            M_Users model = null;
            if (dt.Rows.Count == 1)
            {
                model = new M_Users();
                model.FillData(dt.Rows[0]);
            }

            return model;
        }
         
        public static List<M_Users> GetUsers(int pageSize, int pageIndex, ref int totalCount, ref int pageCount, int type = 2, int status = -1, string keyWords = "", string colmonasc = "", bool isasc = false)
        {
            string whereSql = " a.isfreeze<>9";  
            if (type > -1)
            {
                whereSql += " and a.admingid=" + type + " ";
            } 
            if (status > -1)
            {
                whereSql += " and a.isfreeze=" + status;
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                whereSql += " and (a.UserName like '%" + keyWords + "%' or a.mobile like'%" + keyWords + "%' or a.email like'%" + keyWords + "%') ";
            }
            string cstr = @" a.*,convert(varchar(25),b.registertime,120) registertime,convert(varchar(25),b.lastvisittime,120) lastvisittime ";
            DataTable dt = CommonBusiness.GetPagerData("Owzx_Users a join owzx_userdetails b on a.uid=b.uid  ", cstr, whereSql, "a.Uid", colmonasc, pageSize, pageIndex, out totalCount, out pageCount, isasc);
            List<M_Users> list = new List<M_Users>();
            M_Users model;
            foreach (DataRow item in dt.Rows)
            {
                model = new M_Users();
                model.FillData(item);
                if (!string.IsNullOrEmpty(model.RoleID))
                    model.Role = ManageSystemBusiness.GetRoleByIDCache(model.RoleID);
                list.Add(model);
            }

            return list;
        }


        /// <summary>
        /// 新增或修改用户信息
        /// </summary>
        public static int CreateM_User(M_Users musers)
        { 
            int bl = M_UsersDAL.BaseProvider.CreateM_User( musers.UserName, musers.Password, musers.Email, musers.IsAdmin, musers.RoleID, musers.Email, musers.Mobile,musers.Salt, musers.Avatar,musers.AdminGid);


            return bl;
        }

        public static bool UpdateM_UserRole(int userid, string RoleID, string Description = "")
        {
            bool bl = M_UsersDAL.BaseProvider.UpdateM_UserRole(userid, RoleID, Description);
            return bl;
        }
        public static bool UpdateM_UserStatus(int userid, int status)
        {
            return M_UsersDAL.BaseProvider.UpdateM_UserStatus(userid, status);
        }
        #endregion

        #region 改
        /// <summary>
        /// 修改管理员账户
        /// </summary>
        public static bool SetAdminAccount(string userid, string loginname, string pwd)
        { 
            return M_UsersDAL.BaseProvider.SetAdminAccount(userid, loginname, pwd);
        }
        /// <summary>
        /// 修改用户户信息
        /// </summary>
        public static bool UpdateM_User(int userid, string name, string roleid, string email, string mobilephone, string officephone, string jobs, string avatar, string description)
        {
            bool bl = M_UsersDAL.BaseProvider.UpdateM_User(userid, name, roleid, email, mobilephone, officephone, jobs, avatar, description);
            return bl;
        }
        public static bool DeleteM_User(int userid, int status)
        {
            return M_UsersDAL.BaseProvider.DeleteM_User(userid, status);
        }

        public static bool UpdateM_User(M_Users user)
        {
            return M_UsersDAL.BaseProvider.UpdateUser(user);
        }
        public static bool UpdatePartUser(M_Users user)
        {
            return M_UsersDAL.BaseProvider.UpdatePartUser(user);
        }
        #endregion
    }

    

    
}
