using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OWZXEnum
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum EnumLogType
    {
        Create = 1,
        Update = 2,
        Delete = 3
    }

    //日志对象类型
    public enum EnumLogObjectType
    {
        Customer = 1,
        Orders = 2,
        Activity = 3,
        Product = 4,
        User = 5,
        Agent = 6,
        Opportunity = 7,
        StockIn = 8,
        StockOut = 9
    }

    /// <summary>
    /// 日志模块
    /// </summary>
    public enum EnumLogModules
    {
        Customer = 1,
        Sales = 2,
        Stock = 3,
        Finance = 4,
        System = 5
    }

    /// <summary>
    /// 日志对象
    /// </summary>
    public enum EnumLogEntity
    {
        ClientSetting = 501
    }

    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum EnumLoginStatus
    {
        [DescriptionAttribute("全部")]
        All = -1,
        [DescriptionAttribute("正常")]
        Normal = 1,
        [DescriptionAttribute("异地登陆")]
        OtherLogin = 2,
        [DescriptionAttribute("删除")]
        Delete = 9
    }
}
