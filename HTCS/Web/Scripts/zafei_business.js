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
            field: 'Type',
            title: '类别'
        }];
        debugger;
        var table = new LoadTable(namearr, '/api/Zafei/QueryMenulist', ['893px', '600px']);
        