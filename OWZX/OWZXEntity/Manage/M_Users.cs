/**  版本信息模板在安装目录下，可自行修改。
* M_Users.cs
*
* 功 能： N/A
* 类 名： M_Users
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/3/6 19:52:53   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
namespace OWZXEntity.Manage
{
    ///// <summary>
    ///// M_Users:实体类(属性说明自动提取数据库字段的描述信息)
    ///// </summary>
    //[Serializable]
    //public partial class M_Users
    //{
    //    public M_Users()
    //    {}
    //    #region Model
    //    private int _autoid;
    //    private string _userid;
    //    private string _loginname;
    //    private string _loginpwd;
    //    private string _name="";
    //    private string _email="";
    //    private string _mobilephone="";
    //    private string _officephone="";
    //    private string _jobs="";
    //    private string _avatar="";
    //    private int? _isadmin=0;
    //    private int? _status=1;
    //    private string _description="";
    //    private DateTime? _createtime= DateTime.Now;
    //    private string _createuserid;
    //    public List<Menu> Menus { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int AutoID
    //    {
    //        set{ _autoid=value;}
    //        get{return _autoid;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Property("Lower")] 
    //    public string UserID
    //    {
    //        set{ _userid=value;}
    //        get{return _userid;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string LoginName
    //    {
    //        set{ _loginname=value;}
    //        get{return _loginname;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string LoginPWD
    //    {
    //        set{ _loginpwd=value;}
    //        get{return _loginpwd;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string Name
    //    {
    //        set{ _name=value;}
    //        get{return _name;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string Email
    //    {
    //        set{ _email=value;}
    //        get{return _email;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string MobilePhone
    //    {
    //        set{ _mobilephone=value;}
    //        get{return _mobilephone;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string OfficePhone
    //    {
    //        set{ _officephone=value;}
    //        get{return _officephone;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string Jobs
    //    {
    //        set{ _jobs=value;}
    //        get{return _jobs;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string Avatar
    //    {
    //        set{ _avatar=value;}
    //        get{return _avatar;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int? IsAdmin
    //    {
    //        set{ _isadmin=value;}
    //        get{return _isadmin;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int? Status
    //    {
    //        set{ _status=value;}
    //        get{return _status;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string Description
    //    {
    //        set{ _description=value;}
    //        get{return _description;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public DateTime? CreateTime
    //    {
    //        set{ _createtime=value;}
    //        get{return _createtime;}
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Property("Lower")] 
    //    public string CreateUserID
    //    {
    //        set{ _createuserid=value;}
    //        get{return _createuserid;}
    //    }
    //    public string RoleID
    //    {
    //        set;
    //        get;
    //    }
    //    public M_Role Role
    //    {
    //        set;
    //        get;
    //    }
    //    #endregion Model

    //    /// <summary>
    //    /// 填充数据
    //    /// </summary>
    //    /// <param name="dr"></param>
    //    public void FillData(System.Data.DataRow dr)
    //    {
    //        dr.FillData(this);
    //    }
        

    //}

    /// <summary>
    /// 部分用户信息类
    /// </summary>
   [Serializable]
    public class PartUserInfo
    {
        private int _uid;//用户id
        private string _username = "";//用户名称
        private string _userid = "";//用户标识编号
        private string _email = "";//用户邮箱
        private string _mobile = "";//用户手机
        private string _password = "";//用户密码
        private string _safepassword = "";//用户密码
        private int _admingid;//用户管理员组id
        private int _userrid;//用户等级id
        private string _nickname = "";//用户昵称
        private string _avatar;//用户头像
        private int _paycredits;//支付积分
        private int _rankcredits;//等级积分
        private int _verifyemail;//是否验证邮箱 
        private int _verifysafepassword;//是否验证安全吗
        private int _verifymobile;//是否验证手机
        private DateTime _liftbantime = new DateTime(1900, 1, 1);//解禁时间
        private string _salt;//盐值
        private string _openid;//微信Openid
        private string _qq;//QQ号
        private string _imei;//手机IMEI码
        private int _parentid;//父级id
        private int _usertype;//是否代理用户 1是 0否


        /// <summary>
        ///用户id
        /// </summary>
        public int Uid
        {
            set { _uid = value; }
            get { return _uid; }
        }
        /// <summary>
        ///用户名称
        /// </summary>
        public string UserName
        {
            set { _username = value.TrimEnd(); }
            get { return _username; }
        }

        public int Parentid
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        public int UserType
        {
            set { _usertype = value; }
            get { return _usertype; }
        }

        public string SafePassWord
        {
            set { _safepassword = value.TrimEnd(); }
            get { return _safepassword; }
        }

        private decimal bankmoney;
        public decimal BankMoney
        {
            set { bankmoney = value; }
            get { return bankmoney; }
        }
        /// <summary>
        ///用户标识编号
        /// </summary>
        public string UserId
        {
            set { _userid = value.TrimEnd(); }
            get { return _userid; }
        }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email
        {
            set { _email = value.TrimEnd(); }
            get { return _email; }
        }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string Mobile
        {
            set { _mobile = value.TrimEnd(); }
            get { return _mobile; }
        }
        /// <summary>
        /// 是否验地区
        /// </summary>
        public int VerifyLoginRg { set; get; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password
        {
            set { _password = value.TrimEnd(); }
            get { return _password; }
        }
        ///<summary>
        ///用户管理员组id
        ///</summary>
        public int AdminGid
        {
            get { return _admingid; }
            set { _admingid = value; }
        }
        ///<summary>
        ///用户等级id
        ///</summary>
        public int UserRid
        {
            get { return _userrid; }
            set { _userrid = value; }
        }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName
        {
            set { _nickname = value.TrimEnd(); }
            get { return _nickname; }
        }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value.TrimEnd(); }
        }
        ///<summary>
        ///支付积分
        ///</summary>
        public int PayCredits
        {
            get { return _paycredits; }
            set { _paycredits = value; }
        }
        /// <summary>
        /// 等级积分
        /// </summary>
        public int RankCredits
        {
            get { return _rankcredits; }
            set { _rankcredits = value; }
        }
        /// <summary>
        /// //已改动
        /// </summary>
        public int VerifySafePassWord
        {
            get { return _verifysafepassword; }
            set { _verifysafepassword = value; }
        }

        /// <summary>
        /// 是否验证邮箱
        /// </summary>
        public int VerifyEmail
        {
            get { return _verifyemail; }
            set { _verifyemail = value; }
        }
        /// <summary>
        /// 是否验证手机
        /// </summary>
        public int VerifyMobile
        {
            get { return _verifymobile; }
            set { _verifymobile = value; }
        }
        /// <summary>
        /// 解禁时间
        /// </summary>
        public DateTime LiftBanTime
        {
            get { return _liftbantime; }
            set { _liftbantime = value; }
        }
        ///<summary>
        ///盐值
        ///</summary>
        public string Salt
        {
            get { return _salt; }
            set { _salt = value; }
        }
        ///<summary>
        ///微信标识
        ///</summary>
        public string OpendId
        {
            get { return _openid; }
            set { _openid = value.TrimEnd(); }
        }
        ///<summary>
        ///QQ号
        ///</summary>
        public string QQ
        {
            get { return _qq; }
            set { _qq = value.TrimEnd(); }
        }
        /// <summary>
        /// 部门id
        /// </summary>
        public int DepId
        { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepName
        { get; set; }
        /// <summary>
        /// 手机IMEI码
        /// </summary>
        public string IMEI
        {
            get { return _imei; }
            set { _imei = value.TrimEnd(); }
        }


        private decimal totalmoney;
        /// <summary>
        /// 账户佣金
        /// </summary>
        public decimal TotalMoney
        {
            get { return totalmoney; }
            set { totalmoney = value; }
        }
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }

    /// <summary>
    /// 完整用户信息
    /// </summary>
    [Serializable]
    public class M_Users : PartUserInfo
    {
        private DateTime _lastvisittime = DateTime.Now;//最后访问时间
        private string _lastvisitip = "";//最后访问ip
        private int _lastvisitrgid = -1;//最后访问区域id
        private DateTime _registertime = DateTime.Now;//用户注册时间
        private string _registerip = "";//用户注册ip
        private int _registerrgid = -1;//用户注册区域id
        private int _gender = 0;//用户性别(0代表未知，1代表男，2代表女)
        private string _realname = "";//用户真实名称
        private DateTime _bday = DateTime.Now;//用户出生日期
        private string _idcard = "";//身份证号
        private int _regionid = 0;//区域id
        private int _regionidtwo = 0;//区域id
        private string _address = "";//所在地
        private string _bio = "";//简介
        private int _invitecode = 0;//邀请码
        private int? _isadmin = 0;
        private int _isfreeze = 0;//邀请码
        public List<Menu> Menus { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastVisitTime
        {
            set { _lastvisittime = value; }
            get { return _lastvisittime; }
        }

        public int IsFreeZe
        {
            set { _isfreeze = value; }
            get { return _isfreeze; }
        }

        /// <summary>
        /// 最后访问ip
        /// </summary>
        public string LastVisitIP
        {
            set { _lastvisitip = value; }
            get { return _lastvisitip; }
        }
        /// <summary>
        /// 最后访问区域id
        /// </summary>
        public int LastVisitRgId
        {
            set { _lastvisitrgid = value; }
            get { return _lastvisitrgid; }
        }
        /// <summary>
        /// 用户注册时间
        /// </summary>
        public DateTime RegisterTime
        {
            set { _registertime = value; }
            get { return _registertime; }
        }
        /// <summary>
        /// 用户注册ip
        /// </summary>
        public string RegisterIP
        {
            set { _registerip = value; }
            get { return _registerip; }
        }
        /// <summary>
        /// 用户验证区域id1
        /// </summary>
        public int RegionIdTwo
        {
            set { _regionidtwo = value; }
            get { return _regionidtwo; }
        }
        /// <summary>
        /// 用户验证区域id
        /// </summary>
        public int RegisterRgId
        {
            set { _registerrgid = value; }
            get { return _registerrgid; }
        }
        ///<summary>
        ///用户性别(0代表未知，1代表男，2代表女)
        ///</summary>
        public int Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
        /// <summary>
        /// 用户真实名称
        /// </summary>
        public string RealName
        {
            set { _realname = value; }
            get { return _realname; }
        }
        ///<summary>
        ///用户出生日期
        ///</summary>
        public DateTime Bday
        {
            get { return _bday; }
            set { _bday = value; }
        }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard
        {
            set { _idcard = value; }
            get { return _idcard; }
        }
        ///<summary>
        ///区域id
        ///</summary>
        public int RegionId
        {
            get { return _regionid; }
            set { _regionid = value; }
        }
        ///<summary>
        ///所在地
        ///</summary>
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        ///<summary>
        ///简介
        ///</summary>
        public string Bio
        {
            get { return _bio.TrimEnd(); }
            set { _bio = value; }
        }
        public int? IsAdmin
        {
            set { _isadmin = value; }
            get { return _isadmin; }
        }
        /// <summary>
        /// 邀请码
        /// </summary>
        public int InviteCode
        {
            get { return _invitecode; }
            set { _invitecode = value; }
        }
        public string RoleID
        {
            set;
            get;
        }
        public M_Role Role
        {
            set;
            get;
        }

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }


    }

    /// <summary>
    /// 用户细节信息类
    /// </summary>
   [Serializable]
    public class UserDetailInfo
    {
        private int _uid;//用户id
        private DateTime _lastvisittime = DateTime.Now;//最后访问时间
        private string _lastvisitip = "";//最后访问ip
        private int _lastvisitrgid = -1;//最后访问区域id
        private DateTime _registertime = DateTime.Now;//用户注册时间
        private string _registerip = "";//用户注册ip
        private int _registerrgid = -1;//用户注册区域id
        private int _gender = 0;//用户性别(0代表未知，1代表男，2代表女)
        private string _realname = "";//用户真实名称
        private DateTime _bday = DateTime.Now;//用户出生日期
        private string _idcard = "";//身份证号
        private int _regionid = 0;//区域id
        private int _regionidtwo = 0;//区域id
        private string _address = "";//所在地
        private string _bio = "";//简介
        private int _invitecode = 0;//邀请码
        private string _signname = "";//个性签名
        /// <summary>
        ///用户id
        /// </summary>
        public int Uid
        {
            set { _uid = value; }
            get { return _uid; }
        }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastVisitTime
        {
            set { _lastvisittime = value; }
            get { return _lastvisittime; }
        }
        /// <summary>
        /// 最后访问ip
        /// </summary>
        public string LastVisitIP
        {
            set { _lastvisitip = value; }
            get { return _lastvisitip.TrimEnd(); }
        }
        /// <summary>
        /// 最后访问区域id
        /// </summary>
        public int LastVisitRgId
        {
            set { _lastvisitrgid = value; }
            get { return _lastvisitrgid; }
        }
        /// <summary>
        /// 用户注册时间
        /// </summary>
        public DateTime RegisterTime
        {
            set { _registertime = value; }
            get { return _registertime; }
        }
        /// <summary>
        /// 用户注册ip
        /// </summary>
        public string RegisterIP
        {
            set { _registerip = value; }
            get { return _registerip.TrimEnd(); }
        }
        /// <summary>
        /// 用户注册区域id
        /// </summary>
        public int RegisterRgId
        {
            set { _registerrgid = value; }
            get { return _registerrgid; }
        }
        ///<summary>
        ///用户性别(0代表未知，1代表男，2代表女)
        ///</summary>
        public int Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
        /// <summary>
        /// 用户真实名称
        /// </summary>
        public string RealName
        {
            set { _realname = value; }
            get { return _realname.TrimEnd(); }
        }
        ///<summary>
        ///用户出生日期
        ///</summary>
        public DateTime Bday
        {
            get { return _bday; }
            set { _bday = value; }
        }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard
        {
            set { _idcard = value; }
            get { return _idcard.TrimEnd(); }
        }
        ///<summary>
        ///区域id
        ///</summary>
        public int RegionId
        {
            get { return _regionid; }
            set { _regionid = value; }
        }
        ///<summary>
        ///区域id
        ///</summary>
        public int RegionIdTwo
        {
            get { return _regionidtwo; }
            set { _regionidtwo = value; }
        }
        ///<summary>
        ///所在地
        ///</summary>
        public string Address
        {
            get { return _address.TrimEnd(); }
            set { _address = value; }
        }
        ///<summary>
        ///简介
        ///</summary>
        public string Bio
        {
            get { return _bio.TrimEnd(); }
            set { _bio = value; }
        }
        /// <summary>
        /// 邀请码
        /// </summary>
        public int InviteCode
        {
            get { return _invitecode; }
            set { _invitecode = value; }
        }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string SignName
        {
            get { return _signname.TrimEnd(); }
            set { _signname = value; }
        }
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}

