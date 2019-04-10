function LoadTable(arrcolumn, paraurl,paraarea) {
    layui.use(['layer', 'element', 'form'], function () {
        debugger;
        var form = layui.form;
        var IsActive = 999;
        var $ = layui.jquery;
        var element = layui.element;
        var url;
        var search = {};
        doc.init(function (data) {
            doc.baseurl = data;
        });
        url = doc.baseurl + paraurl;
        $('#table').bootstrapTable({
            url: url,
            method: 'post',
            pageNumber: 1,
            clickToSelect: true,
            queryParams: queryPara,
            queryParamsType: 'limit',
            classes: 'table table-hover table-no-bordered',
            responseHandler: function (data) {
                debugger;
                return {
                    "total": data.numberCount,//总页数
                    "rows": data.numberData   //数据
                };
            },
            onLoadSuccess: function () {
                debugger;
            
                var otherheight = $(".m-search").height() + $("#toolbar").height() + $(".clearfix").height() + $(".layui-tab-title", parent.document).height() + $(".layui-header", parent.document).height() + $(".layadmin-pagetabs", parent.document).height()+230;
                var tableheight =1078 - otherheight;
               
                $(".bootstrap-table").css("height", 630);
            },
            columns: arrcolumn,
            pagination: true,
            sidePagination: "server",
            checkboxHeader: true,
            clickToSelectv: true,
            //selectItemName: "btSelectItem",
            //showRefresh: true,
            idField: "ID"
            //showColumns: true
        });
        element.on('tab(tabletab)', function (data) {
            IsActive = $(this).attr("value");
            reflash();
        });
        form.on('submit(search)', function (data) {
            debugger;
            search = JSON.parse(JSON.stringify(data.field));
            reflash();
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
            var getselect = $('#table').bootstrapTable('getSelections');
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
            var getselect = $('#table').bootstrapTable('getSelections');
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
        function queryPara(param) {
            debugger;
            param.offset = param.offset / 10 + 1;
            var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
                PageSize: param.limit,   //页面大小
                PageIndex: param.offset,//页码
                IsActive: IsActive
            };
            var object = $.extend({}, temp, search);
            return object;
        }
    });
}
    function reflash() {
        var $ = layui.jquery;
        $('#table').bootstrapTable('refresh');
    }
    function layts(msg) {
        debugger;
        layer.msg(msg, {
                        icon: 1,
                        time: 800 //2秒关闭（如果不配置，默认是3秒）
        });
    }

