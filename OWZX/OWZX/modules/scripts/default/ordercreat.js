define(function (require, exports, module) {
    var Upload = require("upload"), ProductIco, ImgsIco,
        Global = require("global"),
        doT = require("dot"),
        choosePDtOther = require("choosepdtfromother"),
        City = require("city"), CityObject,
        Verify = require("verify"), VerifyObject; 

    var ObjectJS = {}, CacheCategory = [], CacheChildCategory = [], CacheItems = [];

    //初始化
    ObjectJS.init = function (customerid, clientid, categoryitem) {
        var _self = this;
        _self.customerid = customerid;
        _self.clientid = clientid;

        if ($(".category-item").length == 1) {
            $(".category-item").addClass("hover");
        }

        if (categoryitem != null) {
            var categoryitems = JSON.parse(categoryitem.replace(/&quot;/g, '"'));
            ObjectJS.categoryitems = categoryitems;
            for (var i = 0; i < categoryitems.length; i++) {
                CacheChildCategory[categoryitems[i].CategoryID] = categoryitems[i].ChildCategory;
            }
            _self.bigCategoryValue = _self.categoryitems[0].CategoryID;
            _self.categoryValue = "";
        }
        _self.bindEvent('');
    }

    //绑定事件
    ObjectJS.bindEvent = function (citycode) {
        var _self = this;
        //保存
        $("#btnSave").click(function () {
            if ($(".tab-nav-ul li.hover").data('type') == 2) {
                if (!VerifyObject.isPass()) {
                    return false;
                }
                if ($(".category-item.hover").length != 1) {
                    alert("请选择加工品类");
                    return false;
                }
                if (!_self.checkPDTList()) {
                    alert('请选择下单产品明细');
                    return false;
                }
                _self.saveModel();
            } else {
                if ($('#pdtName').val().trim() == "") {
                    alert('请选择产品！');
                    return false;
                } if (!_self.checkPDTList()) {
                    alert('请选择产品规格尺码并填写数量');
                    return false;
                }
            }
        });

        //大品类下拉
        require.async("dropdown", function () {
            $(".bigcategory").dropdown({
                prevText: "",
                defaultText: _self.categoryitems[0].CategoryName,
                defaultValue: _self.categoryitems[0].CategoryID,
                data: _self.categoryitems,
                dataValue: "CategoryID",
                dataText: "CategoryName",
                width: 78,
                onChange: function (data) {
                    ObjectJS.bigCategoryValue = data.value;
                    ObjectJS.bindCategory(data);
                }
            });
        });

        ObjectJS.bindCategory({ value: _self.categoryitems[0].CategoryID });

        //切换类型
        $(".ico-radiobox").click(function () {
            var _this = $(this);
            if (!_this.hasClass("hover")) {
                $(".ico-radiobox").removeClass("hover");
                _this.addClass("hover");
                _self.showAttrForOrder(CacheCategory[_self.categoryValue.trim()], 'checkOrderType', 'childGoodsQuantity');
            }
        });
        $(".tab-nav-ul li").click(function() {
            var _this = $(this);
            _this.siblings().removeClass("hover");
            _this.addClass("hover");
            $(".nav-partdiv").hide();
            $("#" + _this.data("id")).show();
        });
        //切换品类
        $(".category-item").click(function () {
            var _this = $(this);
            if (!_this.hasClass("hover")) {
                $(".category-item").removeClass("hover");
                _this.addClass("hover");
            }
        });  
        var uploader = Upload.uploader({
            browse_button: 'productIco',
            file_path: "/Content/UploadFiles/Order/",
            picture_container: "orderImages",
            maxQuantity: 5,
            maxSize: 5,
            successItems: '#orderImages li',
            fileType: 1,
            init: {}
        });
        VerifyObject = Verify.createVerify({
            element: ".verify",
            emptyAttr: "data-empty",
            verifyType: "data-type",
            regText: "data-text"
        });

        CityObject = City.createCity({
            cityCode: "CHN",
            elementID: "city"
        });

        $("#choosePDTOther").click(function () { 
                choosePDtOther.createPDtOther({
                    title: "选择采购产品",
                    type: 1, //1智能工厂
                    clientid: '381c8429-dec0-4143-ad7f-6364e8f0e5e4',//_self.clientid,
                    callback: function (products) { 
                        if (products.length > 0) {
                            $('#pdtName').val(products[0].pname).data("categoryid", products[0].categoryid).data("orderid", products[0].orderid).data("code", products[0].code).data("price", products[0].price).show();
                       
                            Global.post("/IntFactoryOrder/GetZNGCCategorys", { categoryid: products[0].categoryid }, function (data) {
                                if (data.items!=null ) {  
                                    _self.showAttrForOrder(data.items, 'titleName','goodsQuantity');
                                }
                            });
                        }
                    }
                });
        });
    }

    //确认下单明细 goodsQuantity  childGoodsQuantity
    ObjectJS.showAttrForOrder = function (categoryList,obj,contendid) {
        //var _self = this;
        $(".productsalesattr").remove();
        $("#" + contendid).empty();
        CacheItems = []; 
        if ($(".ico-radiobox.hover").data('type') == 2 || contendid == "goodsQuantity") {
            doT.exec("template/default/createorder-checkattr.html", function (template) { 
                var innerhtml = template(categoryList);
                innerhtml = $(innerhtml);
                //组合产品
                innerhtml.find(".check-box").click(function () {
                    var _this = $(this).find(".checkbox");
                    if (_this.hasClass("hover")) {
                        _this.removeClass("hover");
                    } else {
                        _this.addClass("hover");
                    }

                    var bl = false, details = [], isFirst = true, xattr = [], yattr = [];
                    $(".productsalesattr").each(function () {
                        bl = false;
                        var _attr = $(this), attrdetail = details;
                        //组合规格
                        _attr.find(".checkbox.hover").each(function () {
                            bl = true;
                            var _value = $(this);
                            //首个规格
                            if (isFirst) {
                                var model = {};
                                model.ids = _attr.data("id") + ":" + _value.data("id");
                                model.saleAttr = _attr.data("id");
                                model.attrValue = _value.data("id");
                                model.xRemark = _value.data("type") == 1 ? ("【" + _value.data("text") + "】") : "";
                                model.yRemark = _value.data("type") == 2 ? ("【" + _value.data("text") + "】") : "";
                                model.xyRemark = "【" + _value.data("text") + "】";
                                model.names = "【" + _attr.data("text") + "：" + _value.data("text") + "】";
                                model.layer = 1;
                                details.push(model);

                            } else {
                                for (var i = 0, j = attrdetail.length; i < j; i++) {
                                    if (attrdetail[i].ids.indexOf(_value.data("attrid")) < 0) {
                                        var model = {};
                                        model.ids = attrdetail[i].ids + "," + _attr.data("id") + ":" + _value.data("id");
                                        model.saleAttr = attrdetail[i].saleAttr + "," + _attr.data("id");
                                        model.attrValue = attrdetail[i].attrValue + "," + _value.data("id");
                                        model.xRemark = attrdetail[i].xRemark + (_value.data("type") == 1 ? ("【" + _value.data("text") + "】") : "");
                                        model.yRemark = attrdetail[i].yRemark + (_value.data("type") == 2 ? ("【" + _value.data("text") + "】") : "");
                                        model.xyRemark = attrdetail[i].xyRemark + "【" + _value.data("text") + "】";
                                        model.names = attrdetail[i].names + "【" + _attr.data("text") + "：" + _value.data("text") + "】";
                                        model.layer = attrdetail[i].layer + 1;
                                        details.push(model);
                                    }
                                }
                            }
                            //处理二维表
                            if (_value.data("type") == 1 && xattr.indexOf("【" + _value.data("text") + "】") < 0) {
                                xattr.push("【" + _value.data("text") + "】");
                            } else if (_value.data("type") == 2 && yattr.indexOf("【" + _value.data("text") + "】") < 0) {
                                yattr.push("【" + _value.data("text") + "】");
                            }

                        });
                        isFirst = false;
                    });
                    $("#" + contendid).empty();
                    //选择所有属性
                    if (bl) {
                        var layer = $(".productsalesattr").length, items = [];
                        for (var i = 0, j = details.length; i < j; i++) {
                            var model = details[i];
                            if (model.layer == layer) {
                                items.push(model);
                                CacheItems[model.xyRemark] = model;
                            }
                        }
                        var tableModel = {};
                        tableModel.xAttr = xattr;
                        tableModel.yAttr = yattr;
                        tableModel.items = items;

                        //加载子产品
                        doT.exec("template/default/orders_child_list.html", function (templateFun) {
                            var innerText = templateFun(tableModel);
                            innerText = $(innerText);
                            $("#" + contendid).append(innerText);
                            //数量必须大于0的数字
                            innerText.find(".quantity").change(function () {
                                var _this = $(this);
                                if (!_this.val().isInt() || _this.val() <= 0) {
                                    _this.val("0");
                                }

                                var total = 0;
                                $(".child-product-table .tr-item").each(function () {
                                    var _tr = $(this), totaly = 0;
                                    if (!_tr.hasClass("total")) {
                                        _tr.find(".quantity").each(function () {
                                            var _this = $(this);
                                            if (_this.val() > 0) {
                                                totaly += _this.val() * 1;
                                            }
                                        });
                                        _tr.find(".total-y").text(totaly);
                                    } else {
                                        _tr.find(".total-y").each(function () {
                                            var _td = $(this), totalx = 0;
                                            $(".child-product-table .quantity[data-x='" + _td.data("x") + "']").each(function () {
                                                var _this = $(this);
                                                if (_this.val() > 0) {
                                                    totalx += _this.val() * 1;
                                                }
                                            });
                                            total += totalx;
                                            _td.text(totalx);
                                        });
                                        _tr.find(".total-xy").text(total);
                                    }
                                });
                            });
                        });
                    }
                });
                $("#"+obj).after(innerhtml); 
            });
        }
    };

    //绑定小品类
    ObjectJS.bindCategory = function (item) {
        var _self = this;
        var isOnce = true;

        $('.ordercategory').empty();

        var items = CacheChildCategory[_self.bigCategoryValue];

        for (var i = 0; i < items.length; i++) {
            if (!CacheCategory[items[i].CategoryID]) {
                CacheCategory[items[i].CategoryID] = items[i];
            }
        }
        if (isOnce) {
            _self.categoryValue = items[0].CategoryID;
            _self.showAttrForOrder(CacheCategory[_self.categoryValue.trim()], 'checkOrderType', 'childGoodsQuantity');
            isOnce = false;
        }

        require.async("dropdown", function () {
            $(".ordercategory").dropdown({
                prevText: "",
                defaultText: items[0].CategoryName,
                defaultValue: items[0].CategoryID,
                data: items,
                dataValue: "CategoryID",
                dataText: "CategoryName",
                width: 78,
                onChange: function (data) {
                    _self.categoryValue = data.value;
                    _self.showAttrForOrder(CacheCategory[_self.categoryValue.trim()], 'checkOrderType', 'childGoodsQuantity');
                }
            });
        });
    }

    //保存实体
    ObjectJS.saveModel = function () {
        var _self = this;
        var images = "";  
        $("#orderImages li").each(function () {
            var _this = $(this);
            images += _this.data("server") + _this.data("filename") + ",";
        }); 
        var model = {
            CustomerID: _self.customerid,
            PersonName: $("#name").val().trim(),
            OrderType: $(".ico-radiobox.hover").data('type'),
            PlanTime: $("#iptCreateTime").val() == null ? "" : $("#iptCreateTime").val(),
            BigCategoryID: $(".category-item.hover").data("id"),
            CategoryID: _self.categoryValue.trim(),
            CityCode: CityObject.getCityCode(),
            ExpressCode: $("#expressCode").val().trim(),
            Address: $("#address").val().trim(),
            OrderImage: images,
            PlanPrice: $("#planPrice").val().trim(),
            PlanQuantity: 1,
            MobileTele: $("#contactMobile").val().trim(),
            Remark: $("#remark").val().trim(),
            OrderGoods: []
        };

        //大货单遍历下单明细
        if ($(".ico-radiobox.hover").data('type') == 2) {
            $(".child-product-table .quantity").each(function () {
                var _this = $(this);
                if (_this.val() > 0) {
                    var item = CacheItems[_this.data("remark")];
                    model.OrderGoods.push({
                        SaleAttr: item.saleAttr,
                        AttrValue: item.attrValue,
                        SaleAttrValue: item.ids,
                        Quantity: _this.val(),
                        XRemark: item.xRemark,
                        YRemark: item.yRemark,
                        XYRemark: item.xyRemark,
                        Remark: item.names
                    });
                }
            });
        }

        Global.post("/IntFactoryOrder/CreateOrder", { entity: JSON.stringify(model), clientid: _self.clientid }, function (data) {
            if (data.id) {
                alert("新增成功！");
                window.location = location.href;
            } else {
                alert("网络异常,请稍后重试!");
            }
        });
    }

    ObjectJS.checkPDTList = function () {
        if ($(".tab-nav-ul li.hover").data('type') == 2 && $(".ico-radiobox.hover").data('type') == 1) {
            return true;
        }else {
            var totalnum = 0;
            $(".child-product-table .quantity").each(function () {
                var _this = $(this);
                totalnum += parseInt($(this).val());
            }); 
            return totalnum != 0;
        }
    }


    module.exports = ObjectJS;
});