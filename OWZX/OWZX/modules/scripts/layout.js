/*
*布局页JS
*/
define(function (require, exports, module) {
    var $ = require("jquery"),
        doT = require("dot"),
        Global = require("global"),
        Easydialog = require("easydialog");
    require("sortable");

    var LayoutObject = {};
    //初始化数据
    LayoutObject.init = function () {
        
        if ($(window.parent.document).find("#windowItems").length == 0) {
            location.href = "/Default/Index?href=" + location.href;
        }
        LayoutObject.bindStyle();
        LayoutObject.bindEvent();
    }

    //绑定元素定位和样式
    LayoutObject.bindStyle = function () {

    }

    //绑定事件
    LayoutObject.bindEvent = function () {
        var _self = this;

        //展开筛选
        $(".btn-filter").click(function () {
            var _this = $(this);
            if (_this.hasClass("close")) {
                $(".search-body").show("fast");
                _this.removeClass("close");
            } else {
                $(".search-body").hide("fast");
                _this.addClass("close");
            }
            
        });
        //折叠筛选
        $(".close-filter span").click(function () {
            $(".search-body").hide("fast");
        });
        //打开新窗口
        $("body").delegate(".btn-open-window", "click", function () {
            var _this = $(this),
                parent = $(window.parent.document),
                nav = parent.find("#windowItems li[data-id='" + _this.data("id") + "']");

            parent.find("#windowItems li").removeClass("hover");
            parent.find(".iframe-window").hide();
            if (nav.length == 1) {
                nav.addClass("hover");
                parent.find("#iframe" + _this.data("id")).show();
                parent.find("#iframe" + _this.data("id")).attr("src", _this.data("url"));
            } else {
                parent.find("#windowItems").append('<li data-id="' + _this.data("id") + '" class="hover" title="' + _this.data("name") + '">'
                                              + _this.data("name") + ' <span title="关闭" class="iconfont close">&#xe606;</span>'
                                       + '</li>');
                parent.find("#iframeBox").append('<iframe id="iframe' + _this.data("id") + '" class="iframe-window" src="' + _this.data("url") + '"></iframe>');

                var height = window.parent.document.documentElement.clientHeight;
                parent.find("#iframe" + _this.data("id")).css("height", height - 95);
            }

            //拖动排序
            parent.find("#windowItems").sortable({
                items: "li[data-id!='Home']"
            });

            //左右滚动
            if (parent.find("#windowItems li.hover").prevAll().length * 127 + 40 > parent.find(".window-box").width()) {
                parent.find(".left-btn,.right-btn").show();
                parent.find("#windowItems").css("left", parent.find(".window-box").width() - 40 - (parent.find("#windowItems li.hover").prevAll().length + 1) * 127);
            } else {
                parent.find(".left-btn,.right-btn").hide();
                parent.find("#windowItems").css("left", "0")
            }
        });
    }

    module.exports = LayoutObject;
})