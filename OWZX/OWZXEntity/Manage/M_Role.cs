using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OWZXEntity.Manage
{
    [Serializable]
    public partial class M_Role
    {
        public M_Role()
        { }
        #region Model
        private int _autoid;
        private string _roleid;
        private string _name;
        private string _parentid;
        private int? _status = 0;
        private string _description = "";
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
        [Property("Lower")]
        public string RoleID
        {
            set { _roleid = value; }
            get { return _roleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ParentID
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
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
        [Property("Lower")]
        public string CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Property("Lower")]
        public string ClientID
        {
            set { _clientid = value; }
            get { return _clientid; }
        }
        #endregion Model

        public int IsDefault { get; set; }

        [Property("Lower")]
        public string AgentID { get; set; } 

        public List<Menu> Menus { get; set; }

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
