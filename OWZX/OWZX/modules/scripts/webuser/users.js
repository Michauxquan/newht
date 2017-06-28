define(function (require, exports, module) {
    var Global = require("global"),
        doT = require("dot"),
        Verify = require("verify"), VerifyObject,
        Easydialog = require("easydialog");
    require("pager");
    var Model = {};

    var Paras = {
        keyWords: "",
        sourcetype:1,
        pageIndex: 1
    }

    var ObjectJS = {};
    //初始化
    ObjectJS.init = function () {
        var _self = this;
        _self.bindEvent();
        _self.getList(); 
    }
    //绑定事件
    ObjectJS.bindEvent = function () {
        var _self = this;

        $(document).click(function (e) {
            //隐藏下拉
            if (!$(e.target).parents().hasClass("dropdown-ul") && !$(e.target).parents().hasClass("dropdown") && !$(e.target).hasClass("dropdown")) {
                $(".dropdown-ul").hide();
            }
        });
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                Paras.pageIndex = 1;
                Paras.keyWords = keyWords;
                _self.getList();
            });
        });
        $("#createModel").click(function () {
            var _this = $(this);
            Model.RoleID = "";
            Model.UserID = "";
            Model.Name = "";
            Model.Description = "";
            Model.LoginName = "";
            Model.LoginPWD = "";
            Model.Jobs = "";
            Model.Email = "";
            Model.MobilePhone = "";
            Model.OfficePhone = "";
            _self.createModel();
        });
        //删除
        $("#deleteObject").click(function () {
            var id = $(this).data("id");
             confirm("用户删除后不可恢复,确认删除吗？",function() {
                Global.post("/WebUser/DeleteMUser", { id: id }, function(data) {
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
            var _this = $(this);
            Global.post("/WebUser/GetUserDetail", { id: _this.data("id") }, function (data) {
                var model = data.Item;
                Model.NickName = model.NickName;
                Model.Uid = model.Uid;
                Model.UserName = model.UserName; 
                Model.Email = model.Email;
                Model.Mobile = model.Mobile;
                Model.UserType = model.UserType;
                Model.RealName = model.RealName;
                Model.QQ = model.QQ;
                Model.Bio = model.Bio;
                Model.Gender = model.Gender;
                Model.IDCard = model.IDCard;
                Model.Address = model.Address;
                Model.RoleID = model.RoleID;  
                _self.createModel();
            });
        });
    } 
    //登陆名称 密码失焦验证
    ObjectJS.initBindblur = function () {
        $("#modelLoginName").change(function () {
            $("#LoginameInfo").html('');
            if ($("#modelLoginName").val() != '') {
                Global.post("/WebUser/ValidateLoginName", { loginName: $("#modelLoginName").val() }, function (data) {
                    $("#LoginameInfo").html(data.Info);
                });
            }
        });
        $("#modelLoginPWD").blur(function () {
            $("#NewPwdError").html('');
            if ($("#modelLoginPWD").val() == '') {
                $("#NewPwdError").html('密码不能为空');
            }
        });
        $("#modelNewConfirmPwd").blur(function () {
            $("#NewConfirmPwdError").html('');
            if ($("#modelNewConfirmPwd").val() != $("#modelLoginPWD").val()) {
                $("#NewConfirmPwdError").html('确认密码与登录密码不一致');
            }
        });
    }
    //添加/编辑弹出层
    ObjectJS.createModel = function () {
        var _self = this;
        doT.exec("template/WebUser/user-detail.html", function (template) {
            var html = template([]);
            Easydialog.open({
                container: {
                    id: "show-model-detail",
                    header: !Model.Uid>0 ? "新建用户" : "编辑用户",
                    content: html,
                    yesFn: function () {
                        if (!VerifyObject.isPass()) {
                            return false;
                        }
                        Model.NickName = $("#modelName").val();
                        Model.Email = $("#modelEmail").val();
                        Model.Mobile = $("#modelMobilePhone").val(); 
                        Model.RoleID ='';
                        Model.PassWord = $("#modelLoginPWD").val();
                        Model.QQ = $('#modelQQ').val();
                        Model.Uid = $("#modelName").data('uid');   
                        Model.UserType = $('#modelUserType').val();
                        Model.RealName = $('#modelRealName').val();
                        Model.Bio = $('#modelBio').val();
                        Model.Gender = $('.Gender:checked').val();
                        Model.IDCard = $('#modelIdCard').val();
                        Model.Address = $('#modelAddress').val();
                        _self.saveModel(Model);
                    },
                    callback: function () {

                    }
                }
            });
            VerifyObject = Verify.createVerify({
                element: ".verify",
                emptyAttr: "data-empty",
                verifyType: "data-type",
                regText: "data-text"
            });
            $('#modelLoginPWD').addClass('verify');
            $('#modelNewConfirmPwd').addClass('verify');
 
            $("#modelName").focus();
            $("#modelName").val(Model.NickName).data('uid', Model.Uid);
            $("#modelEmail").val(Model.Email); 
            $("#modelMobilePhone").val(Model.Mobile);
            $('#modelQQ').val(Model.QQ);
            $('#modelUserType').val(Model.UserType);
            $('#modelRealName').val(Model.RealName);
            $('#modelBio').val(Model.Bio);
            $('#modelIdCard').val(Model.IDCard);
            $('#modelAddress').val(Model.Address);
            $('.Gender').eq(Model.Gender).click();
            if (Model.Uid > 0) {
                $('#modelLoginPWD').removeClass('verify');
                $('#modelNewConfirmPwd').removeClass('verify');
            }
        });
    }

    //获取列表
    ObjectJS.getList = function () {
        var _self = this;
        $(".tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='11'><div class='data-loading'><div></td></tr>");
        Global.post("/WebUser/GetUsers", Paras, function (data) {
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
            doT.exec("template/WebUser/users.html", function (template) {
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
            $(".tr-header").after("<tr><td colspan='11'><div class='noDataTxt' >暂无数据!</div></td></tr>");
        }
    }
    ObjectJS.validatePWD = function (userid) {
        if (userid) {
            if ($("#modelNewConfirmPwd").val() != $("#modelLoginPWD").val()) {
                return false;
            }
        }
        if ($("#LoginameInfo").html() != "") { return false; }
        return true;
    }
    //保存实体
    ObjectJS.saveModel = function (model) {
        var _self = this;
        Global.post("/WebUser/SaveUser", { entity: JSON.stringify(model) }, function (data) {
            if (data.errmeg == "执行成功") {
                _self.getList();
            } else { alert(data.errmeg); }
        })
    }

    module.exports = ObjectJS;
});