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
        }];
        debugger;
        var table = new LoadTable(namearr, '/api/BaseData/Querytype', ['893px', '600px']);
       
        $("#search").click(function () {
            debugger;
            $('#table').bootstrapTable("refresh");
        });