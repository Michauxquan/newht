using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OWZXEnum
{
    /// <summary>
    /// 系统类型
    /// </summary>
    public enum EnumSystemType
    {
        [DescriptionAttribute("管理系统")]
        Manage = 1,
        [DescriptionAttribute("竞猜系统")]
        Client = 2,
        [DescriptionAttribute("代理商系统")]
        Agent = 3
    }
    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum EnumStatus
    {
        [DescriptionAttribute("全部")]
        All = -1,
        [DescriptionAttribute("禁用")]
        Invalid = 0,
        [DescriptionAttribute("正常")]
        Valid = 1,
        [DescriptionAttribute("删除")]
        Delete = 9
    }
    /// <summary>
    /// 执行状态码
    /// </summary>
    public enum EnumResultStatus
    {
        [DescriptionAttribute("失败")]
        Failed = 0,
        [DescriptionAttribute("成功")]
        Success = 1,
        [DescriptionAttribute("无权限")]
        IsLimit = 10000,
        [DescriptionAttribute("系统错误")]
        Error = 10001,
        [DescriptionAttribute("存在数据")]
        Exists = 10002
    }
    /// <summary>
    /// 搜索类型
    /// </summary>
    public enum EnumSearchType
    {
        Myself = 1,
        Branch = 2,
        All = 3
    }

    /// <summary>
    /// 账号类型
    /// </summary>
    public enum EnumAccountType
    {
        /// <summary>
        /// 账号
        /// </summary>
        UserName = 1,
        /// <summary>
        /// 手机
        /// </summary>
        Mobile = 2, 
        /// <summary>
        /// 微信
        /// </summary>
        WeiXin = 3
        
    }

    /// <summary>
    /// 注册来源
    /// </summary>
    public enum EnumRegisterType
    {
        /// <summary>
        /// 后台添加
        /// </summary>
        Manage = 1,
        /// <summary>
        /// 自助注册
        /// </summary>
        Self = 2, 
        /// <summary>
        /// 微信
        /// </summary>
        WeiXin = 3,
        /// <summary>
        /// 用户分享
        /// </summary>
        ShopShare = 4
    }

    /// <summary>
    /// 系统设置
    /// </summary>
    public enum EnumSettingKey
    {
        /// <summary>
        /// 积分来源 1：产品 2：订单金额
        /// </summary>
        [DescriptionAttribute("IValue")]
        IntegralSource = 1,
        /// <summary>
        /// 金额积分比例
        /// </summary>
        [DescriptionAttribute("DValue")]
        IntegralScale = 2

    }
}
