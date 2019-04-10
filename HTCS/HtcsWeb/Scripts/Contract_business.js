debugger;
var namearr = [[ //表头
        { type: 'checkbox' }
      , { field: 'Id'  , width: 100, title: '合同编号' }
      , { field: 'Name', width: 80, title: '租客姓名' }
      , { field: 'Status', width: 80, title: '状态', templet: formatterstatus }
      , { width: 210, title: '所属房源', templet: formatterhouse }
      , { field: 'ContractTime', width: 200, title: '合同周期', templet: formattertime }
      , { field: 'Deposit', width: 80, title: '押金' }
      , { field: 'Recent', width: 80, title: '租金' }
      , { field: 'Pinlv', width: 180, title: '付款频率' }
      , { field: 'Phone', width: 180, title: '租客电话' }
]];
        debugger;
        var table = new LoadTable(namearr, '/api/Contract/Query', ['893px', '600px']);
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
        function formatterhouse(value) {
            var adress=value.CellName + "-" + value.BuildingNumber + "栋" + value.RoomId + "室";
            if(value.HouseName!=""&&value.HouseName!=null){
                adress+="-"+value.HouseName;
            }
            return '<div>' + adress + '</div>'
        }
        function formattertime(value) {
         
            return '<div>' + value.BeginTime + "→" + value.EndTime + '</div>';
        }