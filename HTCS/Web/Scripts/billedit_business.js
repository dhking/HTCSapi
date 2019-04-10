debugger;
var selecturl="";
doc.init(function (data) {
    doc.baseurl = data;
});

selecturl = doc.baseurl + "/api/Zafei/Zafeilist";
var namearr = [{
            field: 'state',
            checkbox: true
}, {
    field: "index", title: "序号", align: "center", edit: false, formatter: function (value, row, index) {
       
    return row.index=index ; //返回行号  
}},{
    field: 'Id',
    title: '编号',
    edit: false
},{
            field: 'BillType',
            title: '费用类型',
            edit: {
                type: 'select',//下拉框
                url: selecturl,
                valueField: 'Name',
                textField: 'Name',
                onSelect: function (val, rec) {
                    console.log(val, rec);
                }
            }
        }, {
            field: 'Amount',
            title: '金额'
        }, {
            field: 'RecivedAmount',
            title: '已收'
        }, {
            field: 'ReciveAmount',
            title: '待收'
        },{field:"userstatus_end_time",title:"end_time",align:"center",formatter:function(value,row,rowIndex){
						var strHtml = '<a href="javascript:void(0);" onclick="removeRow('+rowIndex+')">删除</a>';
						return strHtml;
					},edit:false}];
debugger;
var saveurl = doc.baseurl + "/api/Bill/edit";
var table = new LoadTable(namearr, '/api/Bill/billlist', ['893px', '600px'], saveurl);
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
    
        
      