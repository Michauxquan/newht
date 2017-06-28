﻿

define(function (require, exports, module) {

    require("jquery");
    require("daterangepicker");
    var Global = require("global"),
        Easydialog = require("easydialog"),
        moment = require("moment"),
        doT = require("dot");

    var Order = {};

    Order.Params = {
        pageIndex: 1,
        pageSize: 20,
        status: -1,
        type: -1,
        beginDate: '',
        endDate: '',
        keyWords: ''
    };


    //列表初始化
    Order.init = function () {
        Order.bindEvent();
        Order.bindData();
    };

    //绑定事件
    Order.bindEvent = function () {
        //关键字查询
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                Order.Params.pageIndex = 1;
                Order.Params.keyWords = keyWords;
                Order.bindData();
            });
        });
        //日期插件
        $("#orderBeginTime").daterangepicker({
            showDropdowns: true,
            empty: true,
            opens: "right",
            ranges: {
                '今天': [moment(), moment()],
                '昨天': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                '上周': [moment().subtract(6, 'days'), moment()],
                '本月': [moment().startOf('month'), moment().endOf('month')]
            }
        }, function (start, end, label) {
            Order.Params.pageIndex = 1;
            Order.Params.beginDate = start ? start.format("YYYY-MM-DD") : '';
            Order.Params.endDate = end ? end.format("YYYY-MM-DD") : '';
            Order.bindData();
        });

        $(document).click(function (e) {
            //隐藏下拉
            if (!$(e.target).parents().hasClass("dropdown-ul") && !$(e.target).parents().hasClass("dropdown") && !$(e.target).hasClass("dropdown")) {
                $(".dropdown-ul").hide();
            }
        });
        $('#editOrder').click(function () {
            var id = $(this).data("id");
            var amount = $(this).data("amount");
            var html = "<input type='text' value='" + amount + "' id='txt-orderAmount' />";
            Easydialog.open({
                container: {
                    id: "show-model-updateorderaccount",
                    header: "修改订单支付金额",
                    content: html,
                    yesFn: function () {
                        if (confirm("确定修改价格吗?")) {
                            Global.post("/Client/UpdateOrderAmount", { id: id, amount: $("#txt-orderAmount").val() }, function (data) {
                                if (data.Result == 1) {
                                    Order.bindData();
                                } else if (data.Result == 1001) {
                                    alert("订单已被审核,操作失败,请刷新页面查看.");
                                } else if (data.Result == 1002) {
                                    alert("订单已被删除,操作失败,请刷新页面查看.");
                                } else {
                                    alert("修改失败");
                                }
                            });
                        }
                    },
                    callback: function () {
                    }
                }
            });
        });
        $('#examineOrder').click(function () {
            if (confirm("确定审核通过吗?")) {
                Global.post("/Client/PayOrderAndAuthorizeClient", { id: $(this).data("id"), agentID: $(this).data("agentid") }, function (data) {
                    if (data.Result == 1) {
                        Order.bindData();
                    } else if (data.Result == 1001) {
                        alert("订单已被审核,操作失败,请刷新页面查看.");
                    } else if (data.Result == 1002) {
                        alert("订单已被删除,操作失败,请刷新页面查看.");
                    } else {
                        alert("审核失败");
                    }
                });
            }
        });
        $('#deleteOrder').click(function () {
            if (confirm("确定删除?")) {
                Global.post("/Client/CloseClientOrder", { id: $(this).data("id") }, function (data) {
                    if (data.Result == 1) {
                        Order.bindData();
                    } else if (data.Result == 1001) {
                        alert("订单已被审核,操作失败,请刷新页面查看.");
                    } else if (data.Result == 1002) {
                        alert("订单已被删除,操作失败,请刷新页面查看.");
                    } else {
                        alert("关闭失败");
                    }
                });
            }
        });

    };

    //搜索
    require.async("dropdown", function () {
        var OrderStatus = [
            { ID: "0", Name: "未支付" },
            { ID: "1", Name: "已支付" },
            { ID: "9",  Name: "已关闭" }
        ];
        $("#OrderStatus").dropdown({
            prevText: "订单状态-",
            defaultText: "所有",
            defaultValue: "-1",
            data: OrderStatus,
            dataValue: "ID",
            dataText: "Name",
            width: "120",
            onChange: function (data) {
                $("#clientOrders").nextAll().remove();
                Order.Params.pageIndex = 1;
                Order.Params.status = parseInt(data.value);
                Order.bindData();
            }
        });

        var OrderTypes = [
            { ID: "4", Name: "试用" },
            { ID: "1", Name: "购买系统" },
            { ID: "2", Name: "购买人数" },
            { ID: "3", Name: "续费" }
        ];
        $("#OrderTypes").dropdown({
            prevText: "订单类型-",
            defaultText: "所有",
            defaultValue: "-1",
            data: OrderTypes,
            dataValue: "ID",
            dataText: "Name",
            width: "120",
            onChange: function (data) {
                $("#clientOrders").nextAll().remove();
                Order.Params.pageIndex = 1;
                Order.Params.type = parseInt(data.value);
                Order.bindData();
            }
        }); 

    });

    //绑定数据
    Order.bindData = function () {
        $(".tr-header").nextAll().remove(); 
        Global.post("/Client/GetAllClientOrders", Order.Params, function (data) {
            doT.exec("template/client/agent-orders.html?3", function (templateFun) {
                var innerText = templateFun(data.Items);
                innerText = $(innerText);
                $(".tr-header").after(innerText);
                innerText.find(".dropdown").click(function () {
                    var _this = $(this);
                    if (_this.data("type") != 1) {
                        var position = _this.find(".ico-dropdown").position();
                        $(".dropdown-ul li").data("id", _this.data("id"));
                        $(".dropdown-ul li").data("amount", _this.data("amount"));
                        $(".dropdown-ul li").data("agentid", _this.data("agentid"));
                        $(".dropdown-ul").css({ "top": position.top + 20, "left": position.left - 55 }).show().mouseleave(function () {
                            $(this).hide();
                        });
                    }
                });
            });

        });
    }

    module.exports = Order;
});