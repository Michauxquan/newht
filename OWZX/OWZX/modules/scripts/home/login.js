

define(function (require, exports, module) {

    require("jquery");
    var Global = require("global"),
        doT = require("dot");

    var ObjectJS = {};
    //登陆初始化
    ObjectJS.init = function (status,bindAccountType, returnUrl) {
        var _self=this;
        if ($(window.parent.document).find("#windowItems").length > 0) {
            window.top.location.href = location.href;
        }
        _self.bindAccountType = bindAccountType;
        _self.returnUrl = returnUrl.replace('&amp;','&');
        ObjectJS.placeholderSupport(); 
        if (status == 2) {
            alert("您的账号已在其它地点登录，如不是本人操作，请及时通知管理员对账号冻结！");
        }  
        ObjectJS.bindEvent();
    }
    //绑定事件
    ObjectJS.bindEvent = function () {
        var _self = this;
        $(document).on("keypress", function (e) {
            if (e.keyCode == 13) {
                $("#btnLogin").click();
            }
        });

        //登录
        $("#btnLogin").click(function () {
            if (!$("#iptUserName").val()) {
                $(".registerErr").html("请输入账号").slideDown();
                return;
            }
            if (!$("#iptPwd").val()) {
                $(".registerErr").html("请输入密码").slideDown();
                return;
            }
            if (_self.bindAccountType == 4 || _self.bindAccountType == 5) {
                $("#btnLogin").html("绑定中...").attr("disabled", "disabled");
            } else {
                $("#btnLogin").html("登录中...").attr("disabled", "disabled");
            }
            $(this).html("登录中...").attr("disabled", "disabled");
            Global.post("/Home/UserLogin", {
                userName: $("#iptUserName").val(),
                pwd: $("#iptPwd").val(),
                remember: $(".cb-remember-password").hasClass("ico-checked") ? 1 : 0,
                bindAccountType: _self.bindAccountType
            },
            function (data) {
                if (_self.bindAccountType == 4 || _self.bindAccountType == 5) {
                    $("#btnLogin").html("绑定").removeAttr("disabled");
                } else {
                    $("#btnLogin").html("登录").removeAttr("disabled");
                }

                console.log(data);

                if (data.result == 1) {
                    if (_self.returnUrl) {
                        location.href = _self.returnUrl;
                    }
                    else {
                        location.href = "/Default/Index";
                    } 
                }
                else if (data.result == 0) {
                    $(".registerErr").html("账号或密码有误").slideDown();
                }
                else if (data.result == 2) {
                    $(".registerErr").html("密码输入错误超过6次，请2小时后再试").slideDown();
                }
                else if (data.result == 3) {
                    $(".registerErr").html("账号或密码有误,您还有" + (6 - parseInt(data.errorCount)) + "错误机会").slideDown();
                }
                else if (data.result == 4) {
                    $(".registerErr").html(data.errinfo).slideDown();
                }
                else if (data.result == -1) {
                    $(".registerErr").html("账号已冻结，请" + data.forbidTime + "分钟后再试").slideDown();
                }
            });
        });

        //记录密码
        $(".cb-remember-password").click(function () {
            var _this = $(this);
            if (_this.hasClass("ico-check")) {
                _this.removeClass("ico-check").addClass("ico-checked");
            } else {
                _this.removeClass("ico-checked").addClass("ico-check");
            }
        });


        $(".txtBoxPassword").click(function () {
            $(this).hide();
            $("#iptPwd").focus();
        });

        $("#iptPwd").blur(function () {
            if ($(this).val() == '')
                $(".txtBoxPassword").show();
        }).focus(function () {
            if ($(this).val() == '')
                $(".txtBoxPassword").hide();
        });

    }

    //判断浏览器是否支持 placeholder
    ObjectJS.placeholderSupport = function () {
        if (!('placeholder' in document.createElement('input'))) {   
            $('[placeholder]').focus(function () {
                var input = $(this);
                input.css("color", "#333");
                if (input.val() == input.attr('placeholder')) {
                    input.val('');
                    input.removeClass('placeholder');
                }

            }).blur(function () {
                var input = $(this);
                if (input.val() == '' || input.val() == input.attr('placeholder')) {
                    input.addClass('placeholder');
                    input.val(input.attr('placeholder')).css("color", "#999");
                }
            }).blur();

        };
    }

    module.exports = ObjectJS;
});