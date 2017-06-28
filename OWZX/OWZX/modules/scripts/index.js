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
    LayoutObject.init = function (href,remainDay, remainDate) {
        var _self = this;
        LayoutObject.remainDay = remainDay;
        LayoutObject.remainDate = remainDate;
        if (href) {
            $("#windowItems li").removeClass("hover");
            $("#iframeHome").hide();
            var titleName = _self.getParam(href, 'name');
            titleName = titleName != "" ? titleName : "新页面";
            $("#windowItems").append('<li data-id="href" class="hover" title="' + titleName + '">' + titleName + '<span title="关闭" class="iconfont close">&#xe606;</span>'
                                   + '</li>');
            $("#iframeBox").append('<iframe id="iframehref" class="iframe-window" src="' + href + '"></iframe>');

        }

        LayoutObject.bindStyle();
        LayoutObject.bindEvent(); 
        LayoutObject.bindUpcomings();

        $("#iframeBox").css("height", document.documentElement.clientHeight - 95);
    }

    LayoutObject.getParam = function (url, name) { 
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); 
        if (url == '') {
            url = window.location.search;
        } else {
            url = url.replace('&amp;', '&'); 
            if (url.indexOf('?') > -1) {
                url = url.substr(url.indexOf('?')+1);
            }
        } 
        var r = url.substr(1).match(reg);
        if (r != null && r.toString().length > 1) {
            return unescape(r[2]);
        }
        return "";
    }
    //绑定元素定位和样式
    LayoutObject.bindStyle = function () {
        var height = document.documentElement.clientHeight;
        $(".iframe-window").css("height", height - 95);

        //左右滚动
        if ($("#windowItems li.hover").prevAll().length * 127 + 40 > $(".window-box").width()) {
            $(".left-btn,.right-btn").show();
            $("#windowItems").css("left", $(".window-box").width() - 40 - ($("#windowItems li.hover").prevAll().length + 1) * 127);
        } else {
            $(".left-btn,.right-btn").hide();
            $("#windowItems").css("left", "0")
        }
    }

    //绑定事件
    LayoutObject.bindEvent = function () {
        var _self = this; 
        //调整浏览器窗体
        $(window).resize(function () {
            _self.bindStyle();
        });

        $(document).click(function (e) {

            if (!$(e.target).parents().hasClass("currentuser") && !$(e.target).hasClass("currentuser")) {
                $(".dropdown-userinfo").fadeOut("1000");
            }

            if (!$(e.target).parents().hasClass("company-logo") && !$(e.target).hasClass("company-logo")) {
                $(".dropdown-companyinfo").fadeOut("1000");
            }

            if (!$(e.target).parents().hasClass("open-collect-store") && !$(e.target).hasClass("open-collect-store")) {
                $(".dropdown-collect-store").fadeOut("1000");
            }

            $("#contentMenu").hide();
        });
        
        //向左滑动
        $(".left-btn").click(function () {
            var position = $("#windowItems").position();
            if (position.left < -500) {
                $("#windowItems").animate({ left: position.left + 500 }, 200);
            } else {
                $("#windowItems").animate({ left: 0 }, 200);
            }
        });

        //向右滑动
        $(".right-btn").click(function () {
            var position = $("#windowItems").position();
            if (position.left > $(".window-box").width() - 40 - $("#windowItems li").length * 127 + 500) {
                $("#windowItems").animate({ left: position.left - 500 }, 200);
            } else {
                $("#windowItems").animate({ left: $(".window-box").width() - 40 - $("#windowItems li").length * 127 }, 200);
            }
        });

        //登录信息展开
        $("#currentUser").click(function () {
            $(".dropdown-userinfo").fadeIn("1000");
        }); 
        //公司名称信息展开
        $("#companyName,#companyLogo").click(function () {
            $(".dropdown-companyinfo").fadeIn("1000");
        });

        //一级菜单图标事件处理
        $("#modulesMenu li").mouseenter(function() {
            var _this = $(this).find("img");
            _this.attr("src", _this.data("hover"));
        });
        $("#modulesMenu li").mouseleave(function () {
            if (!$(this).hasClass("hover")) {
                var _this = $(this).find("img");
                _this.attr("src", _this.data("ico"));
            }
        });

        $("#modulesMenu li").click(function () {
            var _this = $(this);
            if (!_this.hasClass("hover") && _this.attr("id") != "openStores") {
                _this.addClass("hover");
                _this.siblings().removeClass("hover");
                _this.siblings().find("img").each(function() {
                    $(this).attr("src", $(this).data("ico"));
                }); 
                if (_this.attr('id') == 'intfactoryli') {
                    $('#providerul').show(); 
                } else {
                    _self.getChildMenu(_this.data("code"));
                }
            }
        });
         
        //打开窗口
        $("nav").delegate(".action-items li", "click", function () {
            _self.openWindows($(this));
        });

        //打开窗口
        $(".btn-open-window").click( function () {
            _self.openWindows($(this));
        });

        //切换窗口
        $("#windowItems").delegate("li", "click", function () {
            var _this = $(this);
            if (!_this.hasClass("hover")) {
                _this.siblings().removeClass("hover");
                _this.addClass("hover");

                $(".iframe-window").hide();
                $("#iframe" + _this.data("id")).show();
            }
        });
        
        //右键菜单
        $("#windowItems").delegate("li", "contextmenu", function (e) {
            var _this = $(this);
            if (e.clientX < $(".window-box").width()) {
                $("#contentMenu").css({ left: e.clientX, top: e.clientY }).show().mouseleave(function () {
                    $(this).hide();
                });
            } else {
                $("#contentMenu").css({ left: e.clientX - 130, top: e.clientY }).show().mouseleave(function () {
                    $(this).hide();
                });
            }
            $("#contentMenu li").data("id", _this.data("id"));

            if (window.Event) {
                if (e.which == 2 || e.which == 3) {
                    e.cancelBubble = true
                    e.returnValue = false;
                    return false;
                }
            } else if (event.button == 2 || event.button == 3) {
                event.cancelBubble = true
                event.returnValue = false;
                return false;
            }
            return false;
        });

        //关闭窗口
        $("#windowItems").delegate(".close", "click", function () {
            var _this = $(this).parent();
            if (_this.hasClass("hover")) {
                _this.prev().addClass("hover");
                $("#iframe" + _this.prev().data("id")).show();
            }
            _this.remove();
            $("#iframe" + _this.data("id")).remove();

            _self.bindStyle();

            return false;
        }); 


        //意见反馈浮层
        $(".help-feedback .ico-open").click(function () {
            var _this = $(this);
            if (_this.data("open") && _this.data("open") == "1") {
                _this.data("open", "0");
                _self.setRotateL(_this, 45, 0);

                _this.parent().animate({ "height": "45px" }, "fast");
            } else {
                _this.data("open", "1");
                _self.setRotateR(_this, 0, 45);
                _this.parent().animate({ "height": "180px" }, "fast");
            }
        });
        //关注微信号
        $(".help-feedback .ico-help").click(function() {
            Easydialog.open({
                container: {
                    id: "",
                    header: "白水非凡微信公众号",
                    content: "<div class='center'><img src='/modules/images/wx-code.jpg' /><br><span class='font14'>扫一扫关注公众号</span></div>",
                    yesfn: function() {

                    }
                }
            });
        });
        //关闭标签
        $("#closeThis").click(function () {
            $("#windowItems li[data-id='" + $(this).data("id") + "']").find(".close").click();
        });

        //关闭其他标签
        $("#closeOthers").click(function () {
            $("#windowItems li[data-id!='" + $(this).data("id") + "']").find(".close").click();
        });

        //关闭全部标签
        $("#closeAll").click(function () {
            $("#windowItems li .close").click();
        });

        //刷新标签
        $("#refreshThis").click(function () {
            $("#iframe" + $(this).data("id")).attr("src", $("#iframe" + $(this).data("id")).attr("src"));
        });

        $("#modulesMenu li").first().click();
    }

    //打开新窗口
    LayoutObject.openWindows = function (element) {
        var _self = this,
            _this = element,
            nav = $("#windowItems li[data-id='" + _this.data("code") + "']");

        $(".action-items li").removeClass("hover");
        _this.addClass("hover");

        $("#windowItems li").removeClass("hover");
        $(".iframe-window").hide();
        if (nav.length == 1) {
            nav.addClass("hover");
            $("#iframe" + _this.data("code")).show();
            $("#iframe" + _this.data("code")).attr("src", _this.data("url"));
        } else {
            $("#windowItems").append('<li data-id="' + _this.data("code") + '" class="hover" title="' + _this.data("name") + '">'
                                          + _this.data("name") + ' <span title="关闭" class="iconfont close">&#xe606;</span>'
                                   + '</li>');
            $("#iframeBox").append('<iframe id="iframe' + _this.data("code") + '" class="iframe-window" src="' + _this.data("url") + '"></iframe>');
        }
        //拖动排序
        $("#windowItems").sortable({
            items: "li[data-id!='Home']"
        });

        _self.bindStyle();

    }

    //下级菜单
    LayoutObject.getChildMenu = function (code) {
        var _self = this;

        $.get("/Default/LeftMenu", { id: code }, function (html) {
            $("#leftNav").empty();
            $("#leftNav").append(html);

            _self.bindUpcomings();
        });
    }

    //旋转按钮（顺时针）
    LayoutObject.setRotateR = function (obj, i, v) {
        var _self = this;
        if (i < v) {
            i += 3;
            setTimeout(function () {
                obj.css("transform", "rotate(" + i + "deg)");
                _self.setRotateR(obj, i, v);
            }, 5)
        }
    }

    //旋转按钮(逆时针)
    LayoutObject.setRotateL = function (obj, i, v) {
        var _self = this;
        if (i > v) {
            i -= 3;
            setTimeout(function () {
                obj.css("transform", "rotate(" + i + "deg)");
                _self.setRotateL(obj, i, v);
            }, 5)
        } 
    }
     

    //待办小红点
    LayoutObject.bindUpcomings = function () {
        return;
        Global.post("/Default/GetClientUpcomings", {}, function (data) {
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];

                //采购
                if (item.DocType == 1) {
                    if ($("#modulesMenu li[data-code='103000000']").find(".point").length == 0) {
                        $("#modulesMenu li[data-code='103000000']").find(".name").after("<span class='point'></span>");
                    }

                    var controller = $("nav .controller[data-code='103020000']");
                    if (controller.find(".point").length == 0) {
                        controller.find(".controller-item .name").after("<span class='point'></span>");
                    }
                    if (controller.find("li[data-code='103020200']").find(".point").length == 0) {
                        controller.find("li[data-code='103020200']").find(".name").append("<span class='point'></span>");
                    }
                } else if (item.DocType == 3) { //报损
                    if ($("#modulesMenu li[data-code='103000000']").find(".point").length == 0) {
                        $("#modulesMenu li[data-code='103000000']").find(".name").after("<span class='point'></span>");
                    }

                    var controller = $("nav .controller[data-code='103030000']");
                    if (controller.find(".point").length == 0) {
                        controller.find(".controller-item .name").after("<span class='point'></span>");
                    }
                    if (controller.find("li[data-code='103030300']").find(".point").length == 0) {
                        controller.find("li[data-code='103030300']").find(".name").append("<span class='point'></span>");
                    }
                } else if (item.DocType == 4) { //报溢
                    if ($("#modulesMenu li[data-code='103000000']").find(".point").length == 0) {
                        $("#modulesMenu li[data-code='103000000']").find(".name").after("<span class='point'></span>");
                    }

                    var controller = $("nav .controller[data-code='103030000']");
                    if (controller.find(".point").length == 0) {
                        controller.find(".controller-item .name").after("<span class='point'></span>");
                    }
                    if (controller.find("li[data-code='103030400']").find(".point").length == 0) {
                        controller.find("li[data-code='103030400']").find(".name").append("<span class='point'></span>");
                    }
                } else if (item.DocType == 6) { //退货
                    if ($("#modulesMenu li[data-code='103000000']").find(".point").length == 0) {
                        $("#modulesMenu li[data-code='103000000']").find(".name").after("<span class='point'></span>");
                    }

                    var controller = $("nav .controller[data-code='103030000']");
                    if (controller.find(".point").length == 0) {
                        controller.find(".controller-item .name").after("<span class='point'></span>");
                    }
                    if (controller.find("li[data-code='103031000']").find(".point").length == 0) {
                        controller.find("li[data-code='103031000']").find(".name").append("<span class='point'></span>");
                    }
                } else if (item.DocType == 7) { //手工出库
                    if ($("#modulesMenu li[data-code='103000000']").find(".point").length == 0) {
                        $("#modulesMenu li[data-code='103000000']").find(".name").after("<span class='point'></span>");
                    }

                    var controller = $("nav .controller[data-code='103030000']");
                    if (controller.find(".point").length == 0) {
                        controller.find(".controller-item .name").after("<span class='point'></span>");
                    }
                    if (controller.find("li[data-code='103030500']").find(".point").length == 0) {
                        controller.find("li[data-code='103030500']").find(".name").append("<span class='point'></span>");
                    }
                } else if (item.DocType == 21) { //代理商采购单
                    if ($("#modulesMenu li[data-code='103000000']").find(".point").length == 0) {
                        $("#modulesMenu li[data-code='103000000']").find(".name").after("<span class='point'></span>");
                    }

                    var controller = $("nav .controller[data-code='103040000']");
                    if (controller.find(".point").length == 0) {
                        controller.find(".controller-item .name").after("<span class='point'></span>");
                    }
                    if (item.SendStatus == 0) {
                        if (controller.find("li[data-code='103040100']").find(".point").length == 0) {
                            controller.find("li[data-code='103040100']").find(".name").append("<span class='point'></span>");
                        }
                    } else if (item.SendStatus == 1) {
                        if (controller.find("li[data-code='103040200']").find(".point").length == 0) {
                            controller.find("li[data-code='103040200']").find(".name").append("<span class='point'></span>");
                        }
                    }

                    if (item.ReturnStatus == 1 && item.SendStatus == 0) {
                        if (controller.find("li[data-code='103040300']").find(".point").length == 0) {
                            controller.find("li[data-code='103040300']").find(".name").append("<span class='point'></span>");
                        }
                    } else if (item.ReturnStatus == 1 && item.SendStatus > 0) {
                        if (controller.find("li[data-code='103040400']").find(".point").length == 0) {
                            controller.find("li[data-code='103040400']").find(".name").append("<span class='point'></span>");
                        }
                    }
                } else if (item.DocType == 111) { //财务
                    if (item.SendStatus == 1) {
                        if ($("#modulesMenu li[data-code='104000000']").find(".point").length == 0) {
                            $("#modulesMenu li[data-code='104000000']").find(".name").after("<span class='point'></span>");
                        }

                        var controller = $("nav .controller[data-code='104010000']");
                        if (controller.find(".point").length == 0) {
                            controller.find(".controller-item .name").after("<span class='point'></span>");
                        }

                        if (controller.find("li[data-code='104010200']").find(".point").length == 0) {
                            controller.find("li[data-code='104010200']").find(".name").append("<span class='point'></span>");
                        }
                    }
                }
            }
        });
    }  
    // 判断浏览器是否支持 placeholder
    LayoutObject.placeholderSupport = function () {
        if (! ('placeholder' in document.createElement('input')) ) {   
            $('[placeholder]').focus(function () {
                var input = $(this);
                if (input.val() == input.attr('placeholder')) {
                    input.val('');
                    input.removeClass('placeholder');
                }
            }).blur(function () {
                var input = $(this);
                if (input.val() == '' || input.val() == input.attr('placeholder')) {
                    input.addClass('placeholder');
                    input.val(input.attr('placeholder'));
                }
            }).blur();
        };
    }

    module.exports = LayoutObject;
})