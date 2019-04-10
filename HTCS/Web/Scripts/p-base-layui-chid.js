var search = {};
var searchpara = {};
function LoadTable(arrcolumn, paraurl, paraarea,paraother,doneevent) {
    layui.use(['layer', 'element', 'form', 'table'], function () {
        debugger;
        var form = layui.form;
        var IsActive = 999;
        var $ = layui.jquery;
        var element = layui.element;
        var url;
        doc.init(function (data) {
            doc.baseurl = data;
        });
        url = doc.baseurl + paraurl;
        var table = layui.table;
        var inintrequsst = {
            pageName: 'PageIndex' //页码的参数名称，默认：page
  , limitName: 'PageSize' //每页数据量的参数名，默认：limit
        };
        if (paraother != null) {
            $.extend({}, inintrequsst, paraother);
        }
       
        //渲染表格
        table.render({
            elem: '#table'
            ,id: 'idTest'
           
            ,url:url //数据接口
        
            ,method: 'post'
            ,cellMinWidth: 80
            , cols: arrcolumn,
            request: inintrequsst
                , response: {
        statusName: 'Code' //数据状态的字段名称，默认：code
  , statusCode: 0 //成功的状态码，默认：0
  , msgName: 'Message' //状态信息的字段名称，默认：msg
  , countName: 'numberCount' //数据总数的字段名称，默认：count
  , dataName: 'numberData' //数据列表的字段名称，默认：data
    }, done:function () {
        if (doneevent != null) {
            doneevent();
        }
       
    },
    loading:true
        });
        element.on('tab(tabletab)', function (data) {
            debugger;
            IsActive = $(this).attr("value");
            queryPara();
        
        });
        form.on('submit(search)', function (data) {
            debugger;
            search = JSON.parse(JSON.stringify(data.field));
            queryPara();
           
            return false;
        });
        $("#tooladd").click(function () {
            debugger;
            layer.open({
                type: 2,
                title: '新增',
                skin: 'two-layer',
                //anim: 4,
                shadeClose: true,//开启遮罩关闭
                //shade: ['0.5'],
                maxmin: true, //开启最大化最小化按钮
                area: paraarea,
                content: $("#tooladd").attr("hturl") + doc.baseurl //注意，如果str是object，那么需要字符拼接。
            });
        });
        $("#tooledit").click(function () {
            debugger;
            var $ = layui.$;
            var checkStatus = table.checkStatus('idTest')
                    , getselect = checkStatus.data;
                    if (getselect.length == 0) {
                        layer.msg("请选择要编辑的数据");
                    }
                    if (getselect.length > 1) {
                        layer.msg("不支持多选");
                    }
                    else {
                        var ids = getselect[0].Id;
                        layer.open({
                            type: 2,
                            title: '编辑',
                            skin: 'two-layer',
                            //anim: 4,
                            shadeClose: true,//开启遮罩关闭
                            //shade: ['0.5'],
                            maxmin: true, //开启最大化最小化按钮
                            area: paraarea,
                            content: $("#tooledit").attr("hturl") + doc.baseurl + "&Id=" + ids //注意，如果str是object，那么需要字符拼接。
                        });
                    } 
        });
        $("#tooldelete").click(function () {
            debugger;
            var checkStatus = table.checkStatus('idTest')
                   , getselect = checkStatus.data;
            if (getselect.length == 0) {
                layer.msg("请选择删除的数据");
            }
            else {
                var ids = [];
                $.each(getselect, function (index, item) {
                    ids.push(item.Id);
                });
                var url = $("#tooldelete").attr("hturl");
                doc.deletetable(url, ids, reflash);
            }
        });
        function queryPara() {
            debugger;
            var temp = {
                IsActive: IsActive
            };
            searchpara = $.extend({}, temp, search);
            reflash();
        }
        $(document).on("click", ".layui-table-body table.layui-table tbody tr", function () {
            debugger;
            var itemchecked = false;
            var obj = event ? event.target : event.srcElement;
            var tag = obj.tagName;
            var checkbox = $(this).find("td div.laytable-cell-checkbox div.layui-form-checkbox I");
            if (checkbox.length != 0) {
                if (tag == 'DIV') {
                    debugger;
                    checkbox.click();
                    var css=$(this).css("background-color");
                    if (css == "rgb(242, 242, 242)")
                    {
                        $(this).css("background-color", "#fff");
                    } else {
                        $(this).css("background-color", "#f2f2f2");
                    }
                   
                }
            }
            //var child = $(this).find('input[type="checkbox"]');
            //child.each(function (index, item) {
            //    item.checked = !item.checked;
            //    itemchecked = item.checked;
            //});
            //if (itemchecked == true) {
            //    $(this).css("background-color", "#f2f2f2");
            //} else {
            //    $(this).css("background-color", "#fff");
            //}
          
            form.render();//此处form在layui.use中声明为全局变量
        });

        $(document).on("click", "td div.laytable-cell-checkbox div.layui-form-checkbox", function (e) {
            e.stopPropagation();
        });
    });
    
}

function reflash() {
    layui.use(['table'], function () {
        var $ = layui.jquery;
        var table = layui.table;
        table.reload('idTest', { where: searchpara } );
    })
}
    function layts(msg) {
        debugger;
        layer.msg(msg, {
                        icon: 1,
                        time: 800 //2秒关闭（如果不配置，默认是3秒）
        });
    }

