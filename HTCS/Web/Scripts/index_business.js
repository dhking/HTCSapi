layui.use(['table', 'htcsradio', 'echarts', 'htcsLG', 'laydate', 'console'], function () {
    var table = layui.table;
    var mymod = layui.htcsradio;
    debugger;
    var $ = layui.jquery;
    
    var doc = layui.htcsLG;
    var laydate = layui.laydate;
    //库存选项
    var option = { data: null, rdefault: 1 };
    //逾期数据选项
    var option1 = { data: [{ "value": 0, "text": "全部" }, { "value": 2, "text": "逾期" }, { "value": 3, "text": "今天" }, { "value": 4, "text": "1-7天" }], rdefault: 0 };
    //加载基础数据
    var nowDate = mymod.getnowdate("yyyy-MM-dd");
    var chosedate;
    laydate.render({
        elem: '#billdate'
    });
    if ($("#billdate").val() == "") {
        chosedate = nowDate;
    }
    else {
        chosedate = $("#billdate").val();
    }
    var url = "api/Statistics/Query";
    initLoad(1, function (data, chartdata, datakucun) {
        InitNumber(data);
        InitTablekucun(datakucun);
        InitChart(chartdata);
        InitDaiban();
    });
    InitwebiBill();
    mymod.CreateInput($("#housetype"), option, function (result) {
        debugger;
        initLoad(result, function (data, chartdata, datakucun) {
            InitTablekucun(datakucun);
            InitChart(chartdata);
        });
    });
    mymod.CreateInput($("#yuqitype"), option1, function (result) {
        
    });
    function initLoad(housetype,callback) {
        doc.objectQuery(url, { date: chosedate, housetype: housetype }, function (result) {
            debugger;
            var data = result.numberData;
            var chartdata = [data.Stock.Vacancy10, data.Stock.Vacancy20, data.Stock.Vacancy30
      , data.Stock.Vacancyover30];
            var datakucun = [{
                "All": data.Stock.All
                 , "Configuration": data.Stock.Configuration
                 , "Vacancy": data.Stock.Vacancy
                 , "Rent": data.Stock.Rent
            }];
           
            callback(data, chartdata, datakucun);
        });
    }
    function InitNumber(realdata) {
        var laytpl = layui.laytpl;
        var getTplcaiwu = indexcaiwuscript.innerHTML
        , viewcaiwu = document.getElementById('index-caiwu-view');
        laytpl(getTplcaiwu).render(realdata, function (html) {
            viewcaiwu.innerHTML = html;
        });
        var getTplyuding = indexyudingscript.innerHTML
        , viewyuding = document.getElementById('index-yuding-view');
        laytpl(getTplyuding).render(realdata, function (html) {
            viewyuding.innerHTML = html;
        });
    }
    function InitTablekucun(kucundata) {
      
        table.render({
            elem: '#index-kucun-table'
        , data: kucundata
        , cellMinWidth: 80 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
        , cols: [[
        { field: 'All', title: '全部' }
      , { field: 'Configuration', title: '配置中' }
      , { field: 'Vacancy', title: '空置中' }
      , { field: 'Rent', title: '已出租' }
      , {
          field: 'RentPert', title: '空置率', templet: function (value) {
              debugger;
              var percent = 0;
              if (value.Vacancy != 0 && value.All != 0) {
                  percent = (value.Vacancy / value.All).toFixed(4) * 100;
              }
              return "<span style='color:red;font-size:larger;'>" + percent + "%</span>"
          }
      }
        ]]
        });
    }
    function InitDaiban() {
        var apiurl = layui.setter.baseurl;
        table.render({
            elem: '#index-daiban-table'
         , url: apiurl + 'api/Bw/Query'
         , method: 'post'
         , cellMinWidth: 80 //全局定义常规单元格的最小宽度，layui 2.2.1 新增
         , cols: [[
         { field: 'Content', title: '内容' }
           , { field: 'Pdate', title: '待办日期' }
           , { field: 'Status', title: '状态' }

         ]],
         height:580,
            responseHandler: function (data) {
                debugger;
                return {
                    "total": data.numberCount,//总页数
                    "rows": data.numberData   //数据
                };
            },
            pagination: true,
            height:80,
            classes: 'table table-hover',
            queryParams: function queryPara(param) {
                debugger;
                param.offset = param.offset / 10 + 1;
                var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
                    PageSize: param.limit,   //页面大小
                    PageIndex: param.offset,//页码

                };
                var object = $.extend({}, temp, search);
                return object;
            }
        });
    }
    function InitwebiBill() {
        var nowDate = mymod.getnowdate("yyyy-MM-dd");
        var tableoption = {
            domid: "#index-daishou-table", arr: [[ //表头
        { type: 'checkbox' }
      , { field: 'Id', width: 100, title: '编号' }
      , {width: 80, title: '状态', templet: formastatus }
      , { field: 'ShouldReceive', title: '收租时间' }
      , { width: 200, title: '房间', templet: formatterhouse }
      , { field: 'TeantId', width: 80, title: '租客姓名' }
      , { field: 'Amount', width: 80, title: '金额' }
      , { width: 180, title: '操作' }
     
            ]], height: 580, url: 'api/Bill/Querylist', done: function (res) {
                debugger;
                if (res.Code == 1001) {
                    var admin = layui.admin;
                    admin.events.logout();
                }
            }
        };
        doc.InitTable(tableoption);
        function formatterhouse(value) {
            var adress = value.CellName + "-" + value.BuildingNumber + "栋" + value.RoomId + "室";
            if (value.HouseName != "" && value.HouseName != null) {
                adress += "-" + value.HouseName;
            }
            return '<div>' + adress + '</div>'
        }
        function formastatus(value) {
            if (value.ShouldReceive = nowDate) {
                return '<span>' + "到期" + '</span>'
            } else {
                var day = parseInt(d2 - d1) / 1000 / 60/24
                return '<span>逾期' + day + '天</span>'
            }
        }
        
    }
    function InitChart(chartdata) {
        var layecharts = layui.echarts;
        var myChart = layecharts.init(document.getElementById('index-kucun-chart'));
        // 指定图表的配置项和数据
        var option3 = {
            title: {
                text: '空置天数'
            },
            tooltip: {},
            legend: {
                data: ['空置天数']
            },
            xAxis: {
                data: ["<10天", "11-20天", "21-30天", ">30天"]
            },
            yAxis: {},
            series: [{
                name: '天数',
                type: 'bar',
                data: chartdata
            }]
        };
        // 使用刚指定的配置项和数据显示图表。
        myChart.setOption(option3);
    }
  
});