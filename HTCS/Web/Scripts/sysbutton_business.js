debugger;
var namearr = [[ //表头
        { type: 'checkbox' }
      , { field: 'Id', width: 100, title: '编号' }
      , { field: 'BtnNo', width: 80, title: '按钮编号' }
      , { field: 'BtnName', width: 200, title: '按钮名称' }
]];
debugger;
LoadTable(namearr, '/api/Sysrole/Querylistbutton', ['1093px', '600px'], false, "sysbutton-table");

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
    var adress = value.CellName + "-" + value.BuildingNumber + "栋" + value.RoomId + "室";
    if (value.HouseName != "" && value.HouseName != null) {
        adress += "-" + value.HouseName;
    }
    return '<div>' + adress + '</div>'
}
function formattertime(value) {

    return '<div>' + value.BeginTime + "→" + value.EndTime + '</div>';
}