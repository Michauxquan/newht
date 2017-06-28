using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZXDAL.Manage
{
    public class SystemDAL : BaseDAL
    {
        public static SystemDAL BaseProvider = new SystemDAL();

        public DataTable GetSystemMenus()
        {
            return GetDataTable("select * from owzx_Menu  where type=2 and IsHide=0");
        }

        public DataTable GetSystemMenu(string menuCode)
        {
            string sqlStr = "select * from owzx_Menu  where type=2 and MenuCode=@MenuCode";
            SqlParameter[] paras = { 
                                    new SqlParameter("@MenuCode",menuCode),
                                   };

            return GetDataTable(sqlStr, paras, CommandType.Text);
        }


        public bool AddSystemMenu(string menuCode, string name, string controller, string view, string pCode, int sort, int layer, int type)
        {

            string sqlStr = "insert into owzx_menu(MenuCode,Name,Controller,[View],PCode,Sort,Layer,Type) values(@MenuCode,@Name,@Controller,@View,@PCode,@Sort,@Layer,@Type)";
            SqlParameter[] paras = { 
                                    new SqlParameter("@MenuCode",menuCode),
                                    new SqlParameter("@Name",name),
                                    new SqlParameter("@Controller",controller),
                                    new SqlParameter("@View",view),
                                    new SqlParameter("@PCode",pCode),
                                    new SqlParameter("@Sort",sort),
                                    new SqlParameter("@Layer",layer),
                                    new SqlParameter("@Type",type)
                                   };

            return ExecuteNonQuery(sqlStr, paras, CommandType.Text) > 0;
        }

        public bool UpdateSystemMenu(string menuCode, string name, string controller, string view, int sort)
        {

            string sqlStr = "update owzx_menu set MenuCode=@MenuCode,Name=@Name,Controller=@Controller,[View]=@View,Sort=@Sort where MenuCode=@MenuCode";
            SqlParameter[] paras = { 
                                    new SqlParameter("@MenuCode",menuCode),
                                    new SqlParameter("@Name",name),
                                    new SqlParameter("@Controller",controller),
                                    new SqlParameter("@View",view),
                                    new SqlParameter("@Sort",sort)
                                   };

            return ExecuteNonQuery(sqlStr, paras, CommandType.Text) > 0;
        }

        public bool DeleteSystemMenu(string menuCode)
        {

            string sqlStr = "update  owzx_menu set IsHide=1 where MenuCode=@MenuCode";
            SqlParameter[] paras = { 
                                    new SqlParameter("@MenuCode",menuCode)
                                   };

            return ExecuteNonQuery(sqlStr, paras, CommandType.Text) > 0;
        }

        public DataTable GetRoles()
        {
            string sql = "select * from owzx_Role where Status<>9";

            return GetDataTable(sql);
        }

        public DataSet GetRoleByID(string roleid)
        {
            string sql = "select * from owzx_Role where RoleID=@RoleID and Status<>9; select * from owzx_RolePermission where RoleID=@RoleID";

            SqlParameter[] paras = { 
                                       new SqlParameter("@RoleID",roleid)
                                   };

            return GetDataSet(sql, paras, CommandType.Text, "Role|Menus");
        }

        public bool CreateRole(string roleid, string name, string description, string operateid)
        {
            string sql = "insert into owzx_Role(RoleID,Name,Status,IsDefault,Description,CreateUserID) " +
                        " values(@RoleID,@Name1,0,@Description,@CreateUserID)";

            SqlParameter[] paras = { 
                                       new SqlParameter("@RoleID",roleid),
                                       new SqlParameter("@Name",name),
                                       new SqlParameter("@Description",description),
                                       new SqlParameter("@CreateUserID",operateid)
                                   };

            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }

        public bool UpdateRole(string roleid, string name, string description)
        {
            string sql = "update owzx_Role set Name=@Name,Description=@Description where RoleID=@RoleID";

            SqlParameter[] paras = { 
                                       new SqlParameter("@RoleID",roleid),
                                       new SqlParameter("@Name",name),
                                       new SqlParameter("@Description",description)
                                   };
            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }

        public bool DeleteRole(string roleid, out int result)
        {
            result = 0;
            SqlParameter[] paras = { 
                                       new SqlParameter("@Result",result),
                                       new SqlParameter("@RoleID",roleid)
                                   };
            paras[0].Direction = ParameterDirection.Output;
            bool bl = ExecuteNonQuery("owzx_DeleteRole", paras, CommandType.StoredProcedure) > 0;
            result = Convert.ToInt32(paras[0].Value);
            return bl;
        }

        public bool UpdateRolePermission(string roleid, string permissions, int userid)
        {
            SqlParameter[] paras = { 
                                       new SqlParameter("@RoleID",roleid),
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@Permissions",permissions)
                                   };
            return ExecuteNonQuery("owzx_UpdateRolePermission", paras, CommandType.StoredProcedure) > 0;
        }

        public bool UpdateUserRole(string userid, string roleid, string operateid)
        {

            SqlParameter[] paras = { 
                                       new SqlParameter("@RoleID",roleid),
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@OpreateID",operateid)
                                   };
            bool bl = ExecuteNonQuery("owzx_UpdateUserRole", paras, CommandType.StoredProcedure) > 0;
            return bl;
        }

    }
}
