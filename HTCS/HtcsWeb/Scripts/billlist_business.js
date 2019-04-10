debugger;
var namearr = [[ //表头
      { field: 'Id'  , width: 80, title: '编号' }
      , { field: 'BillType', width: 100, title: '费用类型' }
      , { field: 'Amount', width: 80, title: '金额' }
       , { field: 'BillStage', width: 80, title: '几期' }
      , { width: 80, title: '已收', field: 'ReciveAmount' }
      , { width: 80, title: '待收', field: 'RecivedAmount' }
]];
        debugger;
        var table = new LoadTable(namearr, '/api/Bill/billlist', ['893px', '600px']);
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
        function formatterstatus(value) {
            if (value == 1) {
                return '<div><span style="padding:10px;background-color:#1bb974;color:#ffffff;border-radius:5px;">在租中</span></div>'
            } else {
                return '<div><span style="padding:10px;background-color:#ff4b5e;color:#ffffff;border-radius:5px;">已预订</span></div>';
            }
            
        }
      
        function formattertime(value) {
         
            return '<div>' + value.BeginTime + "→" + value.EndTime + '</div>';
        }