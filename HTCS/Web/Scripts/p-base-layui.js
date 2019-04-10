var search = {};
var searchpara = {};
var index = null;
function LoadTable(arrcolumn, paraurl, paraarea, ismuilti,tableid) {
    layui.use(['layer', 'element', 'form', 'table', 'laytpl'], function () {
        debugger;
        var form = layui.form;
        var IsActive = 999;
        var $ = layui.jquery;
        var element = layui.element;
        var url;
        doc.init(function (data) {
            doc.baseurl = data;
        });
        var laytpl = layui.laytpl;

        var getTpl = main.innerHTML
        , view = document.getElementById('view');
        var buttonurl ="/api/RoleButton/Querylist";
        var buttondata = { Id: layui.data('layuiAdmin').userid };
        debugger;
        url = doc.baseurl + paraurl;
        var table = layui.table;
        var tid="#"+tableid;
        //渲染表格
        table.render({
            elem: tid
            , id: 'idTest'
            , height: 650
            , url: url //数据接口
            , page: true //开启分页
            , method: 'post'
            , cellMinWidth: 80
            , cols: arrcolumn,
            request: {
                pageName: 'PageIndex' //页码的参数名称，默认：page
            , limitName: 'PageSize' //每页数据量的参数名，默认：limit
            }, response:
           {
               statusName: 'Code' //数据状态的字段名称，默认：code
            , statusCode: 0 //成功的状态码，默认：0
            , msgName: 'Message' //状态信息的字段名称，默认：msg
            , countName: 'numberCount' //数据总数的字段名称，默认：count
            , dataName: 'numberData' //数据列表的字段名称，默认：data
           },
            where: {
                access_token: layui.data('layuiAdmin').access_token
            },
            done: function (res) {
                debugger;
                if (res.Code == 1001) {
                    var admin = layui.admin;
                    admin.events.logout();
                }

                $(".layui-table-view").css("margin-top", 0);
                $(".layui-table").css("margin-top", 0);
                $(".layui-table-header").css("background-color", "#fff");
                $("table tr").css("background-color", "#fff");
                $(".layui-table-view").css("border-top-width", 0);

            },
            loading: true
        });
        table.on('checkbox(demoEvent)', function (obj) {
            var tr = $(this).parent().parent().parent();
            debugger;
            if (obj.checked == true) {
                tr.css("background-color", "#f2f2f2");
                if (index != null && ismuilti == false) {
                    var obj1 = event ? event.target : event.srcElement;
                    var tag = obj1.tagName;
                    var checkbox1 = $(".layui-table-body table.layui-table tbody tr[data-index=" + index + "]").find("td div.laytable-cell-checkbox div.layui-form-checkbox I");
                    if (checkbox1.length != 0) {
                        debugger;
                        index = tr.attr("data-index");
                        checkbox1.click();
                    }
                }
                index = tr.attr("data-index");
            } else {
                index = null;
                tr.css("background-color", "#fff");
            }
            console.log(obj.data); //选中行的相关数据
            console.log(obj.type); //如果触发的是全选，则为：all，如果触发的是单选，则为：one
        });
        element.on('tab(tabletab)', function (data) {
            debugger;
            IsActive = $(this).attr("value");
            queryPara();

        });
     
        doc.objectQuery(buttonurl, buttondata, function (result) {
            debugger;
            laytpl(getTpl).render(result.numberData, function (html) {
                view.innerHTML = html;
            });
            EventButton();
        });
        function EventButton() {
            $(".tooadd").click(function () {
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
                    layer.open({
                        skin: 'demo-class',
                        title: '删除提示',
                        content: '您是否确认删除该账单，删除该账单后，将不可恢复。'
                        , btn: ['取消', '确认删除']
                        , yes: function (index, layero) {
                            layer.close(index);
                        }
                        , btn2: function (index, layero) {
                            var ids = [];
                            $.each(getselect, function (index, item) {
                                ids.push(item.Id);
                            });
                            var url = $("#tooldelete").attr("hturl");
                            doc.deletetable(url, ids, reflash);
                        }
                    });
                }
            });
        }
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
                debugger;
                checkbox.click();
            }
            form.render();//此处form在layui.use中声明为全局变量
        });

        $(document).on("click", "td div.laytable-cell-checkbox div.layui-form-checkbox", function (e) {
            e.stopPropagation();
        });
    });

}

function reflash() {
    layui.use(['table'], function () {
        searchpara.access_token = layui.data('layuiAdmin').access_token;
        var $ = layui.jquery;
        var table = layui.table;
        index = null;
        table.reload('idTest', { where: searchpara });
    });
}


