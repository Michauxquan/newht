
/*
    --选择客户插件--
    --引用
    choosePDtOther = require("choosepdtfromother");
    choosePDtOther.createPDtOther({});
*/
define(function (require, exports, module) {
    var $ = require("jquery"),
        Global = require("global"),
        doT = require("dot"),
        Easydialog = require("easydialog");
    require("pager");
    require("plug/chooseproduct/style.css");

    var PlugJS = function (options) {
        var _this = this;
        _this.setting = $.extend([], _this.default, options);
        _this.init();
    }

    //默认参数
    PlugJS.prototype.default = {
        title:"选择产品", //标题
        type: 1, //1 智能工厂
        multiple: false,//是否允许多选
        clientid: "",//第三方公司ID
        pageIndex: 1,
        pageSize: 10,
        callback: null   //回调
    };

    PlugJS.prototype.init = function () {
        var _self = this, url = "/plug/choosepdtfromother/choosepdtfromother.html"; 
        doT.exec(url, function (template) {
            var innerHtml = template({});

            Easydialog.open({
                container: {
                    id: "choose-product-add",
                    header: _self.setting.title,
                    content: innerHtml,
                    yesFn: function () {
                        var list = [];
                        $(".product-all .product-items .check").each(function() {
                            var _this = $(this);
                            if (_this.hasClass("ico-checked")) {
                                var model = {
                                    pname: _this.data("pname"),
                                    orderid: _this.data("orderid"),
                                    clientid: _this.data("clientid"),
                                    price: _this.data("price"),
                                    code: _this.data("code"),
                                    categoryid: _this.data("categoryid")
                                };
                                list.push(model);
                            }
                        });
                        _self.setting.callback && _self.setting.callback(list);
                    },
                    callback: function () {

                    }
                }
            });
            //绑定事件
            _self.bindEvent();
        });
    };

    //绑定事件
    PlugJS.prototype.bindEvent = function () {
        var _self = this, url = "/plug/choosepdtfromother/productfromother.html", posturl = "/IntFactoryOrder/GetProductList";
        require.async("search", function () {
            $("#chooseproductfromotherSearch").searchKeys(function (keyWords) {
                if (keyWords) {
                    $(".product-all .product-items").empty();
                    _self.FindOtherPdt(keyWords,posturl, url);
                } else {
                    $(".product-items").empty();
                }
            });
        });
        _self.FindOtherPdt('',posturl, url);
    }
    PlugJS.prototype.FindOtherPdt = function (keyWords,posturl, url) { 
        var _self = this;
        Global.post(posturl, {
            clientid: _self.setting.clientid,
            keywords: keyWords,
            pageIndex: _self.setting.pageIndex,
            pageSize: _self.setting.pageSize
        }, function (data) {
            $(".product-all .product-items").empty();
            doT.exec(url, function (template) {
                var innerHtml = template(data.items);
                innerHtml = $(innerHtml);
                innerHtml.click(function () {
                    var _this = $(this);
                    if (!_this.find(".check").hasClass("ico-checked")) {
                        if (_self.setting.multiple) {
                            _this.find(".check").removeClass("ico-check").addClass("ico-checked");
                        } else {
                            _this.find(".check").removeClass("ico-checked").addClass("ico-check");
                            _this.find(".check").removeClass("ico-check").addClass("ico-checked");
                        }
                    } else {
                        _this.find(".check").removeClass("ico-checked").addClass("ico-check");
                    }
                });
                $(".product-all .product-items").append(innerHtml);
            });
            $("#pagerPdt").paginate({
                total_count: data.totalCount,
                count: data.pageCount,
                start: _self.setting.pageIndex,
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
                    _self.setting.pageIndex = page;
                    _self.FindOtherPdt(keyWords, posturl, url);
                }
            });
        });
    }
    exports.createPDtOther = function (options) {
        return new PlugJS(options);
    }
});