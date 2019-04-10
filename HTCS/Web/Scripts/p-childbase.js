var deletedata = [];
var updatedata = [];
var adddata = [];
function LoadTable(arrcolumn, paraurl, paraarea, saveurl, search) {
    layui.use(['layer', 'element', 'form'], function () {
        debugger;
        var form = layui.form;
        var oridata = [];
        var $ = layui.jquery;
        var element = layui.element;
        var url;
        doc.init(function (data) {
            doc.baseurl = data;
        });
        url = doc.baseurl + paraurl;
        $('#table').bootstrapTable({
            url: url,
            method: 'post',
            editable: true,//开启编辑模式
            uniqueId: 'Id', //将index列设为唯一索引  
            pageNumber: 1,
            clickToSelect: true,
            queryParams: queryPara,
            queryParamsType: 'limit',
            classes: 'table table-hover',
           
            responseHandler: function (data) {
                debugger;
                return {
                    "total": data.numberCount,//总页数
                    "rows": data.numberData   //数据
                };
            },
            onLoadSuccess: function () {
                debugger;
                oridata = $('#table').bootstrapTable('getData');
         
                $(".bootstrap-table").css("margin-bottom",70);
            },
            columns: arrcolumn,
            pagination: true,
            sidePagination: "server",
            checkboxHeader: true,
            clickToSelectv: true
        });
       
        $("#tooladd").click(function () {
            debugger;
            var data = {
                Id:0,
                BillType: '',
                Amount: 0,
                RecivedAmount: 0,
                ReciveAmount:0,
                BillId: search.BillId
            };
            $('#table').bootstrapTable('append', data);
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
                $('#table').bootstrapTable({ editable: true })
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
        
        $("#save1").click(function () {
            debugger;
            var re = $('#table').bootstrapTable('Save');
            if (re == false) {
                return false;
            }
            var allTableData = $('#table').bootstrapTable('getData');//获取表格的所有内容行  
            $.each($('#table').bootstrapTable('getModiDatas'), function (index, value) {
                if (value.Id != 0) {
                    updatedata.push(value);
                }
            });
            $.each(allTableData, function (index, value) {
                if (value.Id == 0) {
                    adddata.push(value);
                }
            });
            var savedata = {};
            savedata.delete = deletedata;
            savedata.update = updatedata;
            savedata.dataadd = adddata;
            doc.Submit(saveurl, savedata, function (result) {
                if (result.Code == 0) {
                    deletedata = [];
                    updatedata = [];
                    adddata = [];
                }
            });
        });
        function queryPara(param) {
            debugger;
            param.offset = param.offset / 10 + 1;
            var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
                PageSize: param.limit,   //页面大小
                PageIndex: param.offset,//页码
              
            };
            var object = $.extend({}, temp, search);
            return object;
        }
    });
}
function removeRow(rowIndex) {
    debugger;
    var row = $("#table").bootstrapTable('getData')[rowIndex];
    if (row.Id != 0)
    {
        deletedata.push(row);
    }
    $('#table').bootstrapTable('removeRow', rowIndex);
}
function CloseEvent() {
    $("#cancel").click(function () {
        debugger;
        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
        parent.layer.close(index); //再执行关闭
    });
}
function Search(data, datavalue) {
    var resulr = false;
    $.each(data, function (index, value) {
        if (datavalue.Id == value.Id) {
            resulr = true;
        } else {
            return false;
        }
    });
    return resulr;
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
 
