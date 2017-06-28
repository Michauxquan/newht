define(function (require, exports, module) {
    var Global = require("global"),
        ec = require("echarts/echarts"); 
    require("echarts/chart/line");
    require("echarts/chart/bar"); 
    var Params = {
        searchType: "systemVitalityRPT",
        dateType: 1,
        modelname:"system",
        beginTime: new Date().setDate(new Date().getDate() - 15).toString().toDate("yyyy-MM-dd"),
        endTime: Date.now().toString().toDate("yyyy-MM-dd")
    };

    var ObjectJS = {};

    ObjectJS.init = function () {
        var _self = this;
        _self.clientsChart = ec.init(document.getElementById('systemVitalityRPT'));
        _self.bindEvent();
    }
    ObjectJS.bindEvent = function () {
        var _self = this; 
        _self.sourceDate();
    }

    ObjectJS.sourceDate = function () {
        var _self = this;
        _self.clientsChart.showLoading({
            text: "数据正在努力加载...",
            x: "center",
            y: "center",
            textStyle: {
                color: "red",
                fontSize: 14
            },
            effect: "spin"
        });
        Global.post("/Report/GetClientVitalityReport", Params, function (data) {
            var title = [], items = [], datanames = [];
            _self.clientsChart.clear();
            if (data.items.length == 0) {
                _self.clientsChart.hideLoading();
                _self.clientsChart.showLoading({
                    text: "暂无数据",
                    x: "center",
                    y: "center",
                    textStyle: {
                        color: "red",
                        fontSize: 14
                    },
                    effect: "bubble"
                });
                return;
            }
            for (var i = 0, j = data.items.length; i < j; i++) {
                title.push(data.items[i].Name);
                var _items = [];
                for (var ii = 0, jj = data.items[i].Items.length; ii < jj; ii++) {
                    if (i == 0) {
                        datanames.push(data.items[i].Items[ii].Name);
                    }
                    _items.push(data.items[i].Items[ii].Value);
                }
                items.push({
                    name: data.items[i].Name,
                    type: 'line',
                    stack: '活跃度',
                    data: _items
                });
            }
            option = {
                tooltip: {
                    trigger: 'axis'
                },
                legend: {
                    data: title
                },
                toolbox: {
                    show: true,
                    feature: {
                        dataView: {
                            show: true,
                            readOnly: false,
                            optionToContent: function (opt) {
                                var axisData = opt.xAxis[0].data;
                                var series = opt.series;
                                var table = '<table class="table-list"><tr class="tr-header">'
                                             + '<td>时间</td>';
                                for (var i = 0, l = series.length; i < l; i++) {
                                    table += '<td>' + series[i].name + '</td>';
                                }
                                table += '</tr>';
                                for (var i = 0, l = axisData.length; i < l; i++) {
                                    table += '<tr>'
                                    + '<td class="center">' + axisData[i] + '</td>'
                                    for (var ii = 0, ll = series.length; ii < ll; ii++) {
                                        table += '<td class="center">' + series[ii].data[i] + '</td>';
                                    }

                                    table += '</tr>';
                                }
                                table += '</table>';
                                return table;
                            }
                        },
                        magicType: { show: true, type: ['line', 'bar'] },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                xAxis: [
                    {
                        type: 'category',
                        boundaryGap: false,
                        data: datanames
                    }
                ],
                yAxis: [
                    { type: 'value'  }
                ],
                series: items
            };
            _self.clientsChart.hideLoading();
            _self.clientsChart.setOption(option);
        });
    }
    module.exports = ObjectJS;
});