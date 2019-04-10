    window.doc = {
        
    };
    doc.baseurl = "";
    doc.imgurl = "";
    doc.initimg = function (callBack) {
        debugger;
        $.ajax({
            url: '/Base/GetimgUrl',
            type: "POST",
            async: false,
            data: {},
            dataType: 'text',
            success: function (result) {
                debugger;
                var resultData = result;
                if (typeof callBack == 'function') {
                    callBack(resultData);
                }
            },
            error: function (a, b, c) {
                layer.msg("查询异常");
            }
        });
    }
    doc.init = function (callBack) {
        debugger;
        $.ajax({
            url: '/Base/GetapiUrl',
            type: "POST",
            async : false,
            data: {},
            dataType: 'text',
            success: function (result) {
                debugger;
                var resultData = result;
                if (typeof callBack == 'function') {
                    callBack(resultData);
                }
            },
            error: function (a, b, c) {
                layer.msg("查询异常");
            }
        });
    }
    //加载数据
    doc.objectQuery = function (url, queryParam, callBack) {
        debugger;
        var apiurl = doc.baseurl + url;
        //var $ = layui.$;
            $.ajax({
                url: apiurl,
                type: "POST",
                async: false,
                beforeSend: function(request) {
                request.setRequestHeader("Authorization", "eee");
                },
                data: queryParam,
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
    }
    doc.formatterQuery = function (table, field,value, callBack) {
        debugger;
        var apiurl =  doc.baseurl+"/api/Formatter/Query";
        //var $ = layui.$;
        $.ajax({
            url: apiurl,
            type: "POST",
            async: false,
            data: { "Table": table, "Field": field, "Value": value },
            dataType: 'json',
            success: function (result) {
                callBack(result);
            },
            error: function (a, b, c) {
                layer.msg("查询出现错误啦");
            }
        });
    }
    doc.ns = function (value) {
        if (value == null) {
            return "";
        } else {
            return value;
        }
    }
    doc.Submit = function (url, data1, callBack, errcallBack) {
        debugger;
        var apiurl = url;
        var subdata = JSON.parse(JSON.stringify(data1));
        var indexe = layer.load(1); //换了种风格
        $.ajax({
            url: apiurl,
            type: "POST",
            async: false,
            data: subdata,
            contentType: "application/json",
            dataType: 'json',
            success: function (result) {
                debugger;
                layer.close(indexe);
                var resultData = result;
                if (resultData.Code == 0) {
                    if (resultData.Message == "") {
                        resultData.Message = "保存成功";
                    }
                    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                    parent.layer.close(index); //再执行关闭
                    try{
                        parent.reflash();
                    }catch(e){

                    }
                    parent.layts(resultData.Message);
                    
                } else {
                    layer.open({
                        title: '温馨提示'
                        , content: resultData.Message
                    });
                    
                }
                if (typeof callBack == 'function') {
                    callBack(resultData);
                }
            },
            error: function (re) {
                layer.close(indexe);
                var errstr;
                if(re.responseJSON!=null){
                    errstr=re.responseJSON.ExceptionMessage;
                }else{
                    errstr=re.responseText;
                }
                layer.open({
                    title: '温馨提示'
                            , content: '保存异常:' + errstr
                });
                if (typeof errcallBack == 'function') {
                    errcallBack();
                }
                return false;
            }
            
        });
    }
    doc.Submit1 = function (url, data, callBack, errcallBack) {
        debugger;
        var apiurl = url;
        var subdata = JSON.parse(JSON.stringify(data));
        var indexe = layer.load(1); //换了种风格
        $.ajax({
            url: apiurl,
            type: "POST",
            async: false,
            data: subdata,
            contentType: "application/json",
            dataType: 'json',
            success: function (result) {
                debugger;
                layer.close(indexe);
                var resultData = result;
                if (resultData.Code == 0) {
                    if (resultData.Message == "") {
                        resultData.Message = "保存成功";
                    }
                    layer.msg(resultData.Message, {
                        icon: 1,
                        time: 800 //2秒关闭（如果不配置，默认是3秒）
                    });
                    var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
                    parent.layer.close(index);
                } else {
                    layer.open({
                        title: '温馨提示'
                        , content: resultData.Message
                    });
                    return false;
                }
                if (typeof callBack == 'function') {
                    callBack(resultData);
                }
            },
            error: function (re) {
                layer.close(indexe);
                var errstr;
                if (re.responseJSON != null) {
                    errstr = re.responseJSON.ExceptionMessage;
                } else {
                    errstr = re.responseText;
                }
                layer.open({
                    title: '温馨提示'
                            , content: '保存异常:' + errstr
                });
                if (typeof errcallBack == 'function') {
                    errcallBack();
                }
                return false;
            }

        });
    }
  
    doc.deletetable = function (url, data, callBack) {
        debugger;
        var url = doc.baseurl+url;
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
                    if (typeof callBack == 'function') {
                        callBack(resultData);
                    }
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
    doc.deletetable1 = function (url, data, callBack) {
        debugger;
        var url = doc.baseurl+url;
        var index = layer.load(1); //换了种风格
        $.ajax({
            url: url,
            type: "POST",
            async: false,
            data: data,
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
                    if (typeof callBack == 'function') {
                        callBack(resultData);
                    }
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
    doc.cmdpure = function (apiurl, spname) {
        var rows = $('#table').bootstrapTable('getSelections');
        var ids = "";
        if (rows.length == 0) {
            layer.msg("请选择操作的数据");
            return;
        }
        $.each(rows, function (index, value) {
            ids += value.Id + ',';
        });
        ids = ids.substr(0, ids.length - 1);
        var subdata = { "Id": ids, "Spname": spname };
        var index = layer.load(1); //换了种风格
        $.ajax({
            url: doc.baseurl+apiurl,
            type: "POST",
            async: false,
            data: subdata,
            dataType: 'json',
            success: function (result) {
                debugger;
                layer.close(index);
                var resultData = result;
                if (resultData.Code == 0) {
                    reflash();
                    if (resultData.Message == "") {
                        resultData.Message = "执行成功";
                    }
                    layer.msg(resultData.Message);
                    if (typeof callBack == 'function') {
                        callBack(resultData);
                    }
                } else {
                    layer.open({
                        title: '温馨提示'
                        , content: resultData.Message
                    });
                    return false;
                }
                
            },
            error: function (a, b, c) {
                layer.close(index);
                layer.open({
                    title: '温馨提示'
                            , content: '保存异常'
                });
                return false;
            }
        });
    }
    doc.getUrlParam=function(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
        var r = window.location.search.substr(1).match(reg);  //匹配目标参数
        if (r != null) return unescape(r[2]); return null; //返回参数值
    }
    function reflash() {
        $('#table').bootstrapTable('refresh');
    }
