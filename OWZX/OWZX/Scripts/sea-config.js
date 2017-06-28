
/*基础配置*/
seajs.config({
    base: "/modules/",
    paths: {
        "echarts": 'plug/echarts/',
        "zrender": 'plug/echarts/zrender/'
    },
    alias: {
        "jquery": "/Scripts/jquery-1.11.1.js",
        "form": "/Scripts/jquery.form.js",
        "parser": "/Scripts/jquery.parser.js",
        "smartzoom": "plug/e-smart-zoom-jquery.min.js",
        //颜色选择器
        "color": "plug/choosecolor/spectrum.js",
        //全局JS
        "global": "scripts/global.js",
        "m_global": "m/scripts/global.js",
        //HTML模板引擎
        "dot": "plug/doT.js",
        //分页控件
        "pager": "plug/datapager/paginate.js",
        //报表底层
        'zrender': 'plug/echarts/zrender/zrender.js',
        //日期控件
        'moment': 'plug/daterangepicker/moment.js',
        'daterangepicker': 'plug/daterangepicker/daterangepicker.js',
        //拖动排序
        "sortable": "plug/sortable.js"
    },
    map: [
        //可配置版本号
        ['.css', '.css?v=20161012'],
        ['.js', '.js?v=20161012']
    ]
});

seajs.config({
    alias: {
        //数据验证
        "verify": "plug/verify.js",
        //上传
        "upload": "plug/upload/upload.js",
        //日志
        "logs": "plug/logs/logs.js",
        //备忘
        "replys": "plug/replys/replys.js",
        //开关插件
        "switch": "plug/switch/switch.js",
        //弹出层插件
        "easydialog": "plug/easydialog/easydialog.js",
        //导入弹出层插件
        "dialog": "plug/dialog/dialog.js",
        //搜索插件
        "search": "plug/seachkeys/seachkeys.js",
        //下拉框
        "dropdown": "plug/dropdown/dropdown.js",
        //显示提示插件
        "tip": "plug/tip/tip.js"
    }
});

