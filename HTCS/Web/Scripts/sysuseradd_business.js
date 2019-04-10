debugger;
var selecturl="";
doc.init(function (data) {
    doc.baseurl = data;
});
selecturl = doc.baseurl + "/api/Sysrole/Querylistrolenopage";
var namearr = [{
            field: 'state',
            checkbox: true
             },
            {
                field: "index", title: "序号", align: "center", edit: false, formatter: function (value, row, index) {
                    row.SysUserId = 12;
            return row.index=index ; //返回行号  
}},{
    field: 'Id',
    title: '编号',
    edit: false
},{
    field: 'SysRoleId',
    title: '角色名称',
    formatter: function (value, row, index) {
        doc.formatterQuery("T_SysRole", "ROLENAME", value, function (data) {
            return data;
        });
    },
    edit: {
        type: 'select',//下拉框
        editable: false,
        url: selecturl,
        valueField: 'Id',
        textField: 'RoleName',
        onSelect: function (val, rec) {
            console.log(val, rec);
        },
        required: true

    }
},{field:"userstatus_end_time",title:"操作",align:"center",formatter:function(value,row,rowIndex){
    var strHtml = '<a href="javascript:void(0);" onclick="removeRow('+rowIndex+')">删除</a>';
    return strHtml;
},edit:false}];
debugger;
var saveurl = doc.baseurl + "/api/Sysuserrole/edit";
var search = {};
search.SysUserId = 0;
search.edtype = 1;
var table = new LoadTable(namearr, '/api/Sysrole/Querylistuserrole', ['993px', '900px'], saveurl,search);
        
    
        
      