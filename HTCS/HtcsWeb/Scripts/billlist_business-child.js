debugger;
var namearr = [[ //表头
      { field: 'Id'  , width: 80, title: '编号' }
      , { field: 'Name', width: 100, title: '费用类型' }
      , { field: 'Status', width: 80, title: '金额', templet: formatterstatus }
      , { width: 80, title: '已收', templet: formatterhouse }
      , { field: 'ContractTime', width: 80, title: '待收', templet: formattertime }
]];
        var HouseId = doc.getUrlParam("HouseId");
        debugger;
        var table = new LoadTable(namearr, '/api/Contract/Query', ['893px', '600px'], {"HouseId":HouseId},doneevent);
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
        function doneevent(){
            $(".layui-table-view").css("margin-top", 0);
            $(".layui-table").css("margin-top", 0);
            $(".layui-table-header").css("background-color", "#fff");
            $("table tr").css("background-color", "#fff");
            $(".layui-table-view").css("border-top-width", 0);
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