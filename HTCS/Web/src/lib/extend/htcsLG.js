ed
layui.define(["setter", "table"], function (exports) { //提示：模块也可以依赖其它模块，如：layui.define('layer', callback);
    var $ = layui.jquery
    , jQuery = layui.jquery;
    var setter = layui.setter;
    var baseurl = setter.baseurl;
    var weburl = setter.weburl;
    var table = layui.table;
    var imgurl = "";
    var form = layui.form;
    var search = {};
    var statussearch = {};
    var searchpara={};
    statussearch.IsActive = "999";
    var element = layui.element;
    debugger;
    var obj = {
        //加载数据
        objectQuery: function (url, queryParam, callBack) {
            debugger;
            var apiurl = baseurl + url;
            //var $ = layui.$;
            $.ajax({
                url: apiurl,
                type: "POST",
                async: false,
                contentType: "application/json",
                data: JSON.stringify(queryParam),
                dataType: 'json',
                success: function (result) {
                    var resultData = result;
                    if (typeof callBack == 'function') {
                        callBack(resultData);
                    } else {
                        layer.msg(resultData.Message, {
                            icon: 1,
                            time: 800 //2秒关闭（如果不配置，默认是3秒）
                        });
                    }
                },
                error: function (a, b, c) {
                    layer.msg("查询出现错误啦");
                }
            });
        },
        InitTable: function (options) {
            debugger;
            options = jQuery.extend({
                domid: "#table",
                ispaing: true,
                url: null,
                height: 480,

                ismuilti: false,
                arr: [[]],
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
                search: {}
            }, options);
            table.render({
                elem: options.domid
               , url: baseurl + options.url
               , method: 'post'
               , cellMinWidth: 80 //全局定义常规单元格的最小宽度，layui 2.2.1 新增

               , cols: options.arr,
                responseHandler: function (data) {
                    debugger;
                    return {
                        "total": data.numberCount,//总页数
                        "rows": data.numberData   //数据
                    };
                },
                page: options.ispaing,
                done: options.done,
                height: options.height,
                classes: 'table table-hover',
                loading: true,
                where: {
                    access_token: layui.data('layuiAdmin').access_token
                },
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
                queryParams: function queryPara(param) {
                    debugger;
                    param.offset = param.offset / 10 + 1;
                    var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
                        PageSize: param.limit,   //页面大小
                        PageIndex: param.offset,//页码
                    };
                    var object = $.extend({}, temp, options.search);
                    return object;
                }
            });

            table.on('checkbox(demoEvent)', function (obj) {
                debugger;
                if (options.ismuilti == false) {
                    if (obj.type == "all") {
                        var csss = $(".layui-table-cell.laytable-cell-1-0.laytable-cell-checkbox");
                        csss.each(function (index, e) {
                            e.children[0].checked = false;
                            e.children[1].className = "layui-unselect layui-form-checkbox";
                        });
                    } else {
                        var csss = $(".layui-table-cell.laytable-cell-checkbox");
                        csss.each(function (index, e) {

                            var dataIndex = e.parentNode.parentNode.getAttribute("data-index");
                            if (dataIndex != null) {
                                var dom = "tr[data-index=" + dataIndex + "]";
                                if (dataIndex != obj.data.LAY_TABLE_INDEX) {
                                    $(dom).css("background-color", "#fff");
                                    e.children[0].checked = false;
                                    e.children[1].className = "layui-unselect layui-form-checkbox";
                                } else {
                                    if (obj.checked == true) {
                                        $(dom).css("background-color", "#f2f2f2");
                                    } else {
                                        $(dom).css("background-color", "#fff");
                                    }
                                }
                            } else {

                                e.children[0].checked = false;
                                e.children[1].className = "layui-unselect layui-form-checkbox";
                            }
                        });
                    }
                } else {
                    debugger;
                    if (obj.type == "all") {
                        var csss = $(".layui-table-cell.laytable-cell-1-0.laytable-cell-checkbox");
                        csss.each(function (index, e) {
                            var dataIndex = e.parentNode.parentNode.getAttribute("data-index");
                            var dom = $("tr[data-index=" + dataIndex + "]");
                            if (obj.checked == true) {
                                dom.css("background-color", "#f2f2f2");
                            } else {
                                dom.css("background-color", "#fff");
                            }
                        });
                    } else {
                        var dom = $("tr[data-index=" + obj.data.LAY_TABLE_INDEX + "]");
                        if (obj.checked == true) {
                            dom.css("background-color", "#f2f2f2");
                        } else {
                            dom.css("background-color", "#fff");
                        }
                    }
                }

            });
            form.on('submit(search)', function (data) {
                debugger;
                search = JSON.parse(JSON.stringify(data.field));
                obj.queryPara({},options.domid); 
                return false;
            });
            element.on('tab(tabletab)', function (data) {
                debugger;
                statussearch.IsActive = $(this).attr("value");
                obj.getsearch(options.formid);
                obj.queryPara({},options.domid);
            });
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
                //form.render();//此处form在layui.use中声明为全局变量
            });
            $(document).on("click", "td div.laytable-cell-checkbox div.layui-form-checkbox", function (e) {
                e.stopPropagation();
            });
        },
        queryPara: function (ohtersearch,domid) {
            debugger;
            searchpara = $.extend(ohtersearch, statussearch, search);
            var tableid = domid.replace("#", "");
            obj.reflash(tableid);
        },
        getsearch: function (formid) {
            debugger;
            var arrserize = $(formid).serializeArray();
            arrserize.forEach((item,index) => {
                search[item.name] = item.value;
            });
        },
        reflash: function (tableid) {
            debugger;
            searchpara.access_token = layui.data('layuiAdmin').access_token;
            table.reload(tableid, { where: searchpara });
        },
        InitButton: function (options, btnscript) {
            options = jQuery.extend({
                area: ['893px', '600px'],
                editarea: ['893px', '600px'],
                btnview:"view",
                tooladd: 'tooladd',
                tooledit: 'tooedit',
                tooldelete: 'tooldelete',
                deleCont:"确认删除"
            }, options);
            var buttonurl = "api/RoleButton/Querylist";
            var buttondata = { Id: layui.data('layuiAdmin').userid };
            obj.objectQuery(buttonurl, buttondata, function (result) {
                debugger;
                var getTpl = btnscript.innerHTML
             , view = document.getElementById(options.btnview);
                var laytpl = layui.laytpl;
                laytpl(getTpl).render(result.numberData, function (html) {
                    view.innerHTML = html;
                });
                obj.EventButton(options);
            });
        },
        EventButton: function (options) {
            var $ = layui.$;
            $("#"+options.tooladd).click(function () {
                debugger;
                layer.open({
                    type: 2,
                    title: '新增',
                    skin: 'two-layer',
                    //anim: 4,
                    shadeClose: true,//开启遮罩关闭
                    //shade: ['0.5'],
                    maxmin: true, //开启最大化最小化按钮
                    area: options.area,
                    content: $("#" + options.tooladd).attr("hturl") + baseurl //注意，如果str是object，那么需要字符拼接。
                });
            });
            $("#" + options.tooledit).click(function () {
                debugger;
               
                var checkStatus = table.checkStatus(options.tableid)
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
                        area: options.editarea,
                        content: $("#"+options.tooledit).attr("hturl") + baseurl + "&Id=" + ids //注意，如果str是object，那么需要字符拼接。
                    });
                }
            });
            $("#" + options.tooldelete).click(function () {
                debugger;

                var checkStatus = table.checkStatus(options.tableid)
                       , getselect = checkStatus.data;
                if (getselect.length == 0) {
                    layer.msg("请选择删除的数据");
                }
                else {
                    layer.open({
                        skin: 'demo-class',
                        title: '删除提示',
                        content:options.deleCont
                        , btn: ['取消', '确认删除']
                        , yes: function (index, layero) {
                            layer.close(index);
                        }
                        , btn2: function (index, layero) {
                            var ids = [];
                            $.each(getselect, function (index, item) {
                                ids.push(item.Id);
                            });
                            var url = $("#" + options.tooldelete).attr("hturl");
                            obj.deletetable(url, ids,options.tableid)
                        }
                    });
                }
            });
        },
        deletetable: function (url, data, tableid) {
            debugger;
            var url =baseurl+url;
            var subdata = { "ids": data.join(',') };
            var index = layer.load(1); //换了种风格
            $.ajax({
                url: url,
                type: "POST",
                async: false,
                data: subdata,
                dataType: 'json',
                success: function (result) {
                    debugger;
                    layer.close(index);
                    var resultData = result;
                    if (resultData.Code == 0) {
                        if (resultData.Message == "") {
                            resultData.Message = "删除成功";
                        }
                        layer.msg(resultData.Message, {
                            icon: 1,
                            time: 800 //2秒关闭（如果不配置，默认是3秒）
                        });
                        obj.reflash(tableid)
                    } else {
                        layer.open({
                            title: '温馨提示'
                            , content: resultData.Message
                        });
                    }
                },
                error: function (a, b, c) {
                    layer.close(index);
                    layer.open({
                        title: '温馨提示'
                                , content: '删除异常'
                    });
                    return false;
                }
            });
        }
    };
    function layts(msg) {
        debugger;
        layer.msg(msg, {
            icon: 1,
            time: 800 //2秒关闭（如果不配置，默认是3秒）
        });
    }
    //输出test接口
    exports('htcsLG', obj);
});