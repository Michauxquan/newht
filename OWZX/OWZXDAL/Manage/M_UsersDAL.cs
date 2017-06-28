using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using OWZXEntity.Manage;

namespace OWZXDAL.Manage
{
    public class M_UsersDAL : BaseDAL
    {
        public static M_UsersDAL BaseProvider = new M_UsersDAL();

        public DataTable GetM_UserByUserName(string loginname, string pwd)
        {

            SqlParameter[] paras = { 
                                    new SqlParameter("@UserName",loginname),
                                    new SqlParameter("@LoginPwd",pwd)
                                   };
            return GetDataTable("select * from Owzx_Users_Users where UserName=@UserName and LoginPwd=@LoginPwd", paras, CommandType.Text);
        }
        public DataTable GetM_UserByLoginName(string loginname)
        {

            SqlParameter[] paras = { 
                                    new SqlParameter("@LoginName",loginname)
                                   };
            return GetDataTable("select * from Owzx_Users where UserName=@LoginName", paras, CommandType.Text);
        }
        public DataTable GetUserDetail(int userID)
        {

            SqlParameter[] paras = { 
                                    new SqlParameter("@uid",userID)
                                   };

            return GetDataTable("owzx_getuserbyid", paras, CommandType.StoredProcedure);
        }

        public bool SetAdminAccount(string userid,string loginname, string pwd)
        {

            SqlParameter[] paras = { 
                                    new SqlParameter("@userID",userid),
                                    new SqlParameter("@UserName",loginname),
                                    new SqlParameter("@LoginPwd",pwd)
                                   };

            return ExecuteNonQuery("update M_Users set LoginName=@UserName , LoginPwd=@LoginPwd where userID=@userID", paras, CommandType.Text) > 0;
        }
        public int CreateM_User(string loginname, string loginpwd, string name, int? isadmin, string roleid, string email, string mobilephone, string Salt, string avatar, int AdminGid)
        {
            string sql = "INSERT INTO Owzx_Users(LoginName,LoginPWD,Name,Email,AdminGid,Mobile,Avatar ,IsAdmin ,Salt,isfreeze ,RoleID) " +
                        " values(@LoginName,@LoginPWD,@Name,@Email,@AdminGid,@MobilePhone,@Avatar,@IsAdmin,@Salt,0,@RoleID)" +
                         " select SCOPE_IDENTITY() ";

            SqlParameter[] paras = {  
                                       new SqlParameter("@LoginName",loginname),
                                       new SqlParameter("@LoginPWD",loginpwd),
                                       new SqlParameter("@Name",name),
                                       new SqlParameter("@Email",email),
                                       new SqlParameter("@AdminGid",AdminGid), 
                                       new SqlParameter("@MobilePhone",mobilephone),  
                                       new SqlParameter("@Avatar",avatar),
                                       new SqlParameter("@IsAdmin",isadmin), 
                                       new SqlParameter("@Salt",Salt), 
                                       new SqlParameter("@RoleID",roleid)
                                   };

            return int.Parse(ExecuteScalar(sql, paras, CommandType.Text).ToString());
        }
        public bool UpdateM_User(int userid, string name, string roleid, string email, string mobilephone, string officephone, string jobs, string avatar, string description)
        {
            string sql = "update Owzx_Users set UserName=@Name,Mobile=@MobilePhone,Email=@Email,Avatar=@Avatar ,RoleID=@RoleID where UID=@UserID ";

            SqlParameter[] paras = {  
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@Name",name),
                                       new SqlParameter("@Email",email),
                                       new SqlParameter("@MobilePhone",mobilephone),  
                                       new SqlParameter("@Avatar",avatar),  
                                       new SqlParameter("@RoleID",roleid)
                                   };

            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }
        public bool DeleteM_User(int userid, int status)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@userID",userid),
                                    new SqlParameter("@Status",status),
                                   };

            return ExecuteNonQuery("update Owzx_Users_Users set isfreeze=@Status where uid=@userID", paras, CommandType.Text) > 0;
        }
        public DataSet GetM_UserByProUserName(string loginname, string pwd, out int result)
        {
            result = 0;
            SqlParameter[] paras = {
                                    new SqlParameter("@Result",result),
                                    new SqlParameter("@LoginName",loginname),
                                    new SqlParameter("@LoginPwd",pwd)
                                   };
            paras[0].Direction = ParameterDirection.Output;
            DataSet ds = GetDataSet("M_GetM_UserToLogin", paras, CommandType.StoredProcedure, "M_User|Permission");
            result = Convert.ToInt32(paras[0].Value);

            return ds;
        }

        public bool UpdateM_UserRole(int userid, string roleid, string Description)
        {
            string sql = "update Owzx_Users set RoleID=@RoleID where Uid=@UserID ";

            SqlParameter[] paras = {  
                                       new SqlParameter("@UserID",userid),
                                        new SqlParameter("@RoleID",roleid),
                                       new SqlParameter("@Description",Description)
                                   };

            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }
        public bool UpdateM_UserStatus(int userid, int status)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@userID",userid),
                                    new SqlParameter("@Status",status),
                                   };

            return ExecuteNonQuery("update M_Users set isfreeze=@Status where uid=@userID and isfreeze not in (9)", paras, CommandType.Text) > 0;
        }

        public bool UpdateUser(M_Users userInfo)
        {
            SqlParameter[] parms = {
									   new SqlParameter("@username",userInfo.UserName),
                                       new SqlParameter("@mobile",userInfo.Mobile),
									   new SqlParameter("@password",userInfo.Password),
									   new SqlParameter("@userrid",userInfo.UserRid),
                                       new SqlParameter("@admingid",userInfo.AdminGid),
									   new SqlParameter("@nickname",userInfo.NickName),
									   new SqlParameter("@uid",userInfo.Uid), 
                                       new SqlParameter("@qq",userInfo.QQ), 
                                       new SqlParameter("@imei",userInfo.IMEI),
                                       new SqlParameter("@usertype",userInfo.UserType)
                                   };

            return ExecuteNonQuery("owzx_updatepartuser", parms, CommandType.StoredProcedure) > 0; 
        }

        public bool UpdatePartUser(M_Users partUserInfo)
        {
            SqlParameter[] parms = {
									new SqlParameter("@username",partUserInfo.UserName),
                                    new SqlParameter("@email",partUserInfo.Email),
                                    new SqlParameter("@mobile",partUserInfo.Mobile),
                                    new SqlParameter("@password",partUserInfo.Password),
                                    new SqlParameter("@userrid",partUserInfo.UserRid),
                                    new SqlParameter("@admingid",partUserInfo.AdminGid),
                                    new SqlParameter("@nickname",partUserInfo.NickName),
                                    new SqlParameter("@avatar",partUserInfo.Avatar),
                                    new SqlParameter("@paycredits",partUserInfo.PayCredits),
                                    new SqlParameter("@rankcredits",partUserInfo.RankCredits),
                                    new SqlParameter("@verifyemail",partUserInfo.VerifyEmail),
                                    new SqlParameter("@verifymobile",partUserInfo.VerifyMobile),
                                    new SqlParameter("@liftbantime",partUserInfo.LiftBanTime),
                                    new SqlParameter("@salt",partUserInfo.Salt),
                                    new SqlParameter("@uid",partUserInfo.Uid),
                                    new SqlParameter("@imei",partUserInfo.IMEI)
                                   };

            return ExecuteNonQuery("owzx_updatepartuser", parms, CommandType.StoredProcedure) > 0; 
        }
    }
}
