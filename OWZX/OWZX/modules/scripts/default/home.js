

define(function (require, exports, module) {

    require("jquery");
    var Global = require("global"),
        doT = require("dot");

    var ObjectJS = {};

    //首页JS
    ObjectJS.init = function () {
        
        ObjectJS.bindStyle();
        ObjectJS.bindEvent();
         
    }

    //首页样式
    ObjectJS.bindStyle = function () {

        $(".report-box").fadeIn();

        var width = document.documentElement.clientWidth - 300, height = document.documentElement.clientHeight - 200;

        var unit = 302;
        
        $(".report-box").css({
            width: unit * 3 + 121
        });

        $(".report-box").css({
            marginTop: (document.documentElement.clientHeight - 500) / 2
        });
       
    }

    ObjectJS.bindEvent = function () {
        //调整浏览器窗体
        $(window).resize(function () {
            ObjectJS.bindStyle();
        });
    }
    module.exports = ObjectJS;
});