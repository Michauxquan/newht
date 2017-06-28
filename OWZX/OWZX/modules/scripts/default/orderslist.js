define(function (require, exports, module) {
    var Global = require("global"),
        Easydialog = require("easydialog");
        doT = require("dot"); 
        require("pager");

    var CacheProduct = [];

    var Params = {
        categoryID: "",
        clientid: "",
        BeginPrice: "",
        EndPrice: "",
        pageIndex: 1,
        pageSize: 12,
        keyWords: "" 
    }

    var ObjectJS = {};
    //初始化
    ObjectJS.init = function (clientid, providers) {
        var _self = this;
        _self.clientid = clientid;
        _self.providers = JSON.parse(providers.replace(/&quot;/g, '"')); 
        _self.bindEvent();  
    }
    //绑定事件
    ObjectJS.bindEvent = function () {
        var _self = this;
        var providerids = '-1';
        //for (var t = 0; t < _self.providers.length; t++) {
        //    if (_self.providers[t].CMClientID != "" && _self.providers[t].CMClientID != null) {
        //        providerids += "''" + _self.providers[t].CMClientID + "'',";
        //    }
        //} 
        //if (providerids != "") {
        //    providerids = providerids.substring(2, providerids.length -3);
        //} 
        Params.clientid = providerids;
        require.async("dropdown", function () {
            var dropdown = $("#ddlProviders").dropdown({
                prevText: "供应商-",
                defaultText: "全部",
                defaultValue:"-1",
                data: _self.providers,
                dataValue: "CMClientID",
                dataText: "Name",
                width: "180",
                isposition: true,
                onChange: function (data) {
                    Params.pageIndex = 1;
                    if (data.value == "") {
                        Params.clientid = providerids;
                    } else {
                         Params.clientid = data.value;
                    }
                    _self.getProducts();
                }
            });
        });
        //搜索
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                Params.keyWords = keyWords;
                _self.getProducts();
            });
        });
        _self.getProducts(); 
    }

    //绑定产品列表
    ObjectJS.getProducts = function (params) {
        var _self = this;
        var attrs = [];
        $("#productlist").empty();
        $("#productlist").append("<div class='data-loading' ><div>"); 

        Global.post("/IntFactoryOrder/GetProductList", Params, function (data) {
            $("#productlist").empty();
            if (data.items.length > 0) {
                doT.exec("template/default/zngc-products.html", function (templateFun) {
                    var html = templateFun(data.items);
                    html = $(html);

                    //打开产品详情页
                    html.find(".productimg,.name").each(function () {
                        $(this).data("url", $(this).data("url") + "&type=" + _self.type + "&guid=" + _self.guid);
                    });
                    //加入购物车
                    //html.find(".btnAddCart").click(function () {
                    //    var _this = $(this);
                    //    _self.showDetail($(this));
                    //});

                    $("#productlist").append(html);
                });
            } else {
                $("#productlist").append("<div class='nodata-box' >暂无数据!</div>");
            }

            $("#pager").paginate({
                total_count: data.totalCount,
                count: data.pageCount,
                start: Params.pageIndex,
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
                float: "normal",
                onChange: function (page) {
                    Params.pageIndex = page;
                    ObjectJS.getProducts();
                }
            });
        });
    }
    /*
    //加入购物车
    ObjectJS.showDetail = function (obj) {
        var _self = this;
        if (_self.isLoading) {
            alert("数据加载中，请稍后");
            return false;
        }
        var oid = obj.data('orderid');
       // _self.isLoading = true;
        //缓存产品信息
        if (!CacheProduct[oid]) {
            Global.post("/IntFactoryOrder/GetOrderDetailByID", { orderid: oid, clientid: obj.data('clientid'), categoryid: obj.data('categoryid') }, function (data) {
                doT.exec("template/default/product-detail.html", function(templateFun) { 
                    CacheProduct[oid] = data.items;
                    var html = templateFun(data.items);
                    Easydialog.open({
                        container: {
                            id: "product-zngc-div",
                            header: "选择产品",
                            content: html,
                            callback: function() {

                            }
                        }
                    }); 
                    _self.showAttrForOrder(data.items, 'pdttilel', 'productAttr');
                    Easydialog.toPosition(); 
                });
            });
        } else { 
            doT.exec("template/default/product-detail.html", function (templateFun) {
                var html = templateFun(CacheProduct[oid]);
                Easydialog.open({
                    container: {
                        id: "product-zngc-div",
                        header: "选择产品",
                        content: html,
                        callback: function () {

                        }
                    }
                });
                _self.showAttrForOrder(CacheProduct[oid], 'pdttilel', 'productAttr');
                Easydialog.toPosition(); 
            });
        }
    }
    var CacheItems = [];
    //规格属性加载
    ObjectJS.showAttrForOrder = function (categoryList,obj,contendid) { 
        $(".productsalesattr").remove();
        $("#" + contendid).empty();
        CacheItems = []; 
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
            $(innerhtml).find(".column-title").css("width","50px");
            $("#"+obj).after(innerhtml); 
        }); 
    };
 
    //绑定加入购物车事件
    ObjectJS.bindDetailEvent = function (model, pid, did) {
        var _self = this;
        //选择规格
        $("#productDetails li").click(function () {
            var _this = $(this);
            if (!_this.hasClass("hover")) {
                _this.addClass("hover");
                _this.siblings().removeClass("hover");
                for (var i = 0, j = model.ProductDetails.length; i < j; i++) {
                    if (model.ProductDetails[i].ProductDetailID == _this.data("id")) {
                        $("#addcart").prop("disabled", false).removeClass("addcartun");
                        _self.detailid = model.ProductDetails[i].ProductDetailID;
                        $("#price").html("￥" + model.ProductDetails[i].Price.toFixed(2));
                        $("#productimg").attr("src", model.ProductDetails[i].ImgS || model.ProductImage);
                        $("#productStockQuantity").text(model.ProductDetails[i].StockIn - model.ProductDetails[i].LogicOut);
                        return;
                    } else {
                        $("#addcart").prop("disabled", true).addClass("addcartun");
                    }
                }
            }
        });

        //产品数量
        $("#quantity").blur(function () {
            if (!$(this).val().isInt()) {
                $(this).val("1");
            } else if ($(this).val() < 1) {
                $(this).val("1");
            }
        });
        //+1
        $("#quantityadd").click(function () {
            $("#quantity").val($("#quantity").val() * 1 + 1);
        });
        //-1
        $("#quantityreduce").click(function () {
            if ($("#quantity").val() != "1") {
                $("#quantity").val($("#quantity").val() * 1 - 1);
            }
        });
        $("#addcart").click(function () {
            var cart = $("#shopping-cart").offset();
            var temp = $("<div style='width:30px;height:30px;'><img style='width:30px;height:30px;' src='" + $("#productimg").attr("src") + "' /></div>");
            temp.offset({ top: $(this).offset().top + 20, left: $(this).offset().left + 100 });
            temp.css("position", "absolute");
            $("body").append(temp);
            temp.animate({ top: cart.top, left: cart.left }, 500, function () {
                temp.remove();
                Global.post("/ShoppingCart/AddShoppingCart", {
                    ordertype: _self.type,
                    guid: _self.guid,
                    productid: pid,
                    detailsid: _self.detailid,
                    quantity: $("#quantity").val(),
                    unitid: $("#small").data("id"),
                    name: $("#addcart").data("name"),
                    remark: $("#productDetails li.hover").data("name")
                }, function (data) {
                    if (data.Status) {
                        Easydialog.close();
                        if ($("#shopping-cart .totalcount").html() == '0') {
                            _self.cartitem.pids = '';
                            $("#shopping-cart").data('pid', '');
                        }
                        if ($("#shopping-cart").data('pid').indexOf(_self.detailid) == -1) {
                            $("#shopping-cart").data('pid', $("#shopping-cart").data('pid') + ',' + _self.detailid);
                            $("#shopping-cart .totalcount").html($("#shopping-cart .totalcount").html() * 1 + 1);
                            _self.cartitem.pids += _self.detailid + ',';
                        }
                    }
                });
            });
        });
    }

    //绑定信息
    ObjectJS.bindDetail = function (model, did) {
        var _self = this;
        _self.detailid = did;
        //绑定子产品详情
        for (var i = 0, j = model.ProductDetails.length; i < j; i++) {
            if (model.ProductDetails[i].ProductDetailID == did) {
                $("#productDetails li[data-id='" + _self.detailid + "']").addClass("hover");
                $("#price").html("￥" + model.ProductDetails[i].Price.toFixed(2));
                $("#productimg").attr("src", model.ProductDetails[i].ImgS || model.ProductImage);
                $("#productStockQuantity").text(model.ProductDetails[i].StockIn - model.ProductDetails[i].LogicOut);
                break;
            }
        }

        _self.isLoading = false;
    }
    */
    module.exports = ObjectJS;
});