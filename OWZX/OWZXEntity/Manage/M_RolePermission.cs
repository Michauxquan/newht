using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OWZXEntity.Manage
{
    [Serializable]
    public partial class M_RolePermission
    {
        public M_RolePermission()
        { }
        #region Model
        private int _autoid;
        private string _roleid;
        private string _permissionid;
        private DateTime? _createtime = DateTime.Now;
        private string _createuserid;
        private string _clientid;
        /// <summary>
        /// 
        /// </summary>
        public int AutoID
        {
            set { _autoid = value; }
            get { return _autoid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RoleID
        {
            set { _roleid = value; }
            get { return _roleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MenuCode
        {
            set { _permissionid = value; }
            get { return _permissionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClientID
        {
            set { _clientid = value; }
            get { return _clientid; }
        }
        #endregion Model

    }
}
