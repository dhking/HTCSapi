debugger;
var namearr = [[ //表头
        { type: 'checkbox'}
      , { field: 'Id', width: 100, title: '编号' }
      , { field: 'Name', width: 80, title: '姓名' }
     
      , { field: 'Password', width: 200, title: '密码'}
     
]];
        debugger;
        LoadTable(namearr, '/api/Sysuser/Querylist', ['1093px', '600px'], false,"sysuser-table");
       

        
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