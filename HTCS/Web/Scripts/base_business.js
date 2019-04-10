debugger;
var namearr = [{
            field: 'state',
            checkbox: true
        }, {
            field: 'Id',
            title: '编号'
        }, {
            field: 'Name',
            title: '名称'
        }, {
            field: 'Code',
            title: '编码'
        }, {
            field: 'IsActive',
            title: '状态',
            formatter: function (value) {
                if (value == 0) {
                    return "未启用"
                } if (value == 1) {
                    return "已启用"
                }
            }
        }, {
            field: 'Type',
            title: '配备',
            formatter: function (value) {
                if (value == 0) {
                    return "私有配备"
                } if (value == 1) {
                    return "公开配备"
                }
            }
        },{
    field: 'ParaType',
    title: '配备'
}];
        debugger;
        var table = new LoadTable(namearr,'/api/BaseData/Querypeibei',['893px', '600px']);
        $("#isactive").click(function () {
            debugger;
            var url = "/api/Procedure/CmdProce";
            doc.cmdpure(url, "sp_peibei_trante1");
        });
        $("#isnotactive").click(function () {
            debugger;
            var url = "/api/Procedure/CmdProce";
            doc.cmdpure(url, "sp_peibei_trante0");
        });
        $("#search").click(function () {
            debugger;
            $('#table').bootstrapTable("refresh");
        });