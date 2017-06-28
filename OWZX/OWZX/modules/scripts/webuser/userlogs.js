define(function (require, exports, module) {
    var Global = require("global"),
        doT = require("dot"); 
    var Paras = { pageIndex: 1,keyWords:'',uid:-1}
    require("pager");
    var ObjectJS = {};
    //初始化
    ObjectJS.init = function (uid) {
        var _self = this;
        _self.bindEvent();
        Paras.uid = uid;
        _self.getList();
    }
    //绑定事件
    ObjectJS.bindEvent = function () {
        var _self = this; 
        //关键字搜索
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                Paras.pageIndex = 1;
                Paras.keyWords = keyWords;
                _self.getList();
            });
        }); 
    } 

    //获取列表
    ObjectJS.getList = function () {
        var _self = this;
        console.log(Paras);
        $(".tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='5'><div class='data-loading'><div></td></tr>");
        Global.post("/WebUser/UserLogsList", Paras, function (data) {
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
            doT.exec("template/webuser/userlogs.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml); 
                $(".tr-header").after(innerhtml);
            });
        }
        else {
            $(".tr-header").after("<tr><td colspan='5'><div class='noDataTxt' >暂无数据!</div></td></tr>");
        }
    }  
    module.exports = ObjectJS;
});