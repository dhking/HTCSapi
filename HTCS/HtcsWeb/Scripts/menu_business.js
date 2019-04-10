layui.use(['laypage','layer', 'htcsradio', 'laytpl', 'jquery', 'form', 'htcsLG'], function () {
    var laypage = layui.laypage
    , layer = layui.layer;
    var laytpl = layui.laytpl;
    var $ = layui.$;
    var doc = layui.htcsLG;
   
    var tableoption = {
        domid: "#menu-index-table", formid: "#menu-search-form", arr: [[
            { type: 'checkbox' },
            { field: 'Id', width: 100, title: '编号' }
            , {
                field: 'name', width: 200,
                title: '菜单代码'
            }, {
                field: 'title', width: 200,
                title: '菜单名称'
            }, {
                field: 'icon', width: 200,
                title: '图标'
            }
            ]], height: 650, url: 'api/Menu/QueryMenulist',
        ismuilti: false
    };
    doc.InitTable(tableoption);
    var BtnOption = {
        area: ['893px', '600px'],
        tableid: "menu-index-table",
        btnview: "menu-button-view",
        tooladd: "menu-add-btn",
        tooledit: "menu-edit-btn",
        tooldelete: "menu-delete-btn"
    };
    doc.InitButton(BtnOption, menubtnscribt);
   
});
function reflash() {
    debugger;
    layui.use(['table'], function () {
        var table = layui.table;
        table.reload("menu-index-table");
    });
}