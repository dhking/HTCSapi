debugger;
layui.use(['table', 'htcsradio', 'htcsLG', 'laydate'], function () {
   
    var $ = layui.$;
    var mymod = layui.htcsradio;
    var nowDate = mymod.getnowdate("yyyy-MM-dd");
    var doc = layui.htcsLG;
    var laydate = layui.laydate;
    laydate.render({
        elem: '#ShouldReceive'
    });
    var option1 = { data: [{ "value": 0, "text": "全部" }, { "value": 2, "text": "逾期" }, { "value": 3, "text": "今天" }, { "value": 4, "text": "1-7天" }], rdefault: 0 };
    mymod.CreateInput($("#yuqitype"), option1, function (result) {
        
    });
    var tableoption = {
        domid: "#bill-main-table", formid: "#bill-search-form", arr: [[ //表头
    { type: 'checkbox' }
  , { field: 'Id', width: 100, title: '编号' }
  , { width: 80, title: '状态', templet: formastatus }
  , { field: 'ShouldReceive', title: '收租时间' }
  , { width: 200, title: '房间', templet: formatterhouse }
  , { field: 'TeantId', width: 80, title: '租客姓名' }
  , { field: 'Amount', width: 80, title: '金额' }
  , { width: 180, title: '操作' }
        ]], height: 620, url: 'api/Bill/Querylist',
        ismuilti: false
    };
    doc.InitTable(tableoption);
    var BtnOption = {
        area: ['893px', '600px'],
        tableid: "bill-main-table",
        btnview: "bill-button-view",
        tooladd: "menu-add-btn",
        tooledit: "menu-edit-btn",
        tooldelete: "menu-delete-btn"
    };
    doc.InitButton(BtnOption, billbtnscribt);
   $("#receivebtn").click(function () {
            debugger;
            var table1 = layui.table;
            var checkStatus = table1.checkStatus('idTest')
                    , getselect = checkStatus.data;
            if (getselect.length == 0) {
                layer.msg("请选择要编辑的数据");
                return;
            }
            if (getselect.length > 1) {
                layer.msg("不支持多选");
                return;
            }
            var ids = getselect[0].Id;
            layer.open({
                type: 2,
                title: '收款',
                skin: 'two-layer',
                //anim: 4,
                shadeClose: true,//开启遮罩关闭
                //shade: ['0.5'],
                maxmin: true, //开启最大化最小化按钮
                area: ['1000px', '900px'],
                content:$("#receivebtn").attr("hturl")+"?Id=" + ids //注意，如果str是object，那么需要字符拼接。
            });
        });
        $("#isnotactive").click(function () {
            debugger;
            var url = "/api/Procedure/CmdProce";
            doc.cmdpure(url, "sp_peibei_trante0");
        });
       
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
                var day = parseInt(d2 - d1) / 1000 / 60 / 24
                return '<span>逾期' + day + '天</span>'
            }
        }
});