define(function (require, exports, module) {
    var Global = require("global"),
        doT = require("dot");
    require("pager");
    var Paras = {
        keyWords: "",
        status: -1,
        sourcetype:0,
        pageIndex: 1
    }

    var ObjectJS = {};
    //初始化
    ObjectJS.init = function (rolelist) {
        var _self = this;
        _self.bindEvent();
        _self.getList();
    }
    //绑定事件
    ObjectJS.bindEvent = function () {
        var _self = this;

        require.async("dropdown", function () {
            var UserStatus = [
               { name: "正常", value: "1" },
               { name: "禁闭", value: "3" },
               { name: "注册未完成", value: "0" }
            ];
            $("#userStatus").dropdown({
                prevText: "状态-",
                defaultText: "全部",
                defaultValue: "-1",
                data: UserStatus,
                dataValue: "value",
                dataText: "name",
                width: "140",
                onChange: function (data) {
                    Paras.status = data.value;
                    Paras.pageIndex = 1;
                    _self.getList();
                }
            });
        });
        //关键字搜索
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                Paras.pageIndex = 1;
                Paras.keyWords = keyWords;
                _self.getList();
            });
        });
        $(document).click(function (e) {
            //隐藏下拉
            if (!$(e.target).parents().hasClass("dropdown-ul") && !$(e.target).parents().hasClass("dropdown") && !$(e.target).hasClass("dropdown")) {
                $(".dropdown-ul").hide();
            }
        });
        //删除
        $("#deleteObject").click(function () {
            var id = $(this).data("id");
             confirm("用户禁闭后不可登录,确认禁闭吗？",function() {
                 Global.post("/SysSet/UpdateUserStatus", { id: id ,status:3}, function (data) {
                    if (data != null && data.status==1) {
                        _self.getList();
                    } else {
                        alert("操作失败,请重新操作！");
                    }
                });
            }); 
        });
        //编辑
        $("#updateObject").click(function () {
            var id = $(this).data("id");
            confirm("用户解禁后允许登录,确认解除禁闭吗？", function () {
                Global.post("/SysSet/UpdateUserStatus", { id: id, status:1 }, function (data) {
                    if (data != null && data.status == 1) {
                        _self.getList();
                    } else {
                        alert("操作失败,请重新操作！");
                    }
                });
            });
        });
    }

    //获取列表
    ObjectJS.getList = function () {
        var _self = this;
        $(".tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='6'><div class='data-loading'><div></td></tr>");
        Global.post("/SysSet/GetUsers", Paras, function (data) {
            _self.bindList(data.Items);
            $("#pager").paginate({
                total_count: data.totalCount,
                count: data.pageCount,
                start: Paras.pageIndex,
                display: 5,
                border: true,
                border_color: '#fff',
                text_color: '#333',
                background_color: '#fff',
                border_hover_color: '#ccc',
                text_hover_color: '#000',
                background_hover_color: '#efefef',
                rotate: true,
                images: false,
                mouse: 'slide',
                onChange: function (page) {
                    Paras.pageIndex = page;
                    _self.getList();
                }
            });
        });
    }

    //加载列表
    ObjectJS.bindList = function (items) {
        var _self = this;
        $(".tr-header").nextAll().remove();
        if (items.length > 0) {
            doT.exec("template/SysSet/webusers.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml);
                //操作
                innerhtml.find(".dropdown").click(function () {
                    var _this = $(this);
                    if (_this.data("type") != 1) {
                        var position = _this.find(".ico-dropdown").position();
                        $(".dropdown-ul li").data("id", _this.data("id"));
                        $(".dropdown-ul").css({ "top": position.top + 20, "left": position.left - 55 }).show().mouseleave(function () {
                            $(this).hide();
                        });
                    }
                });

                $(".tr-header").after(innerhtml);
            });
        }
        else {
            $(".tr-header").after("<tr><td colspan='6'><div class='noDataTxt' >暂无数据!</div></td></tr>");
        }
    } 

    module.exports = ObjectJS;
});