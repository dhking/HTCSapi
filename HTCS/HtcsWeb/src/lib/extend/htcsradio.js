layui.define(function (exports) { //提示：模块也可以依赖其它模块，如：layui.define('layer', callback);
    var $ = layui.jquery
  , jQuery = layui.jquery;
    var obj = {
        CreateInput:function(Dom, options, cllback) {
            debugger;
            
            var _self = Dom;
            var result = "";
            var id = _self.attr("id");
            if (options.data == null) {
                options.data = [{ "value": 1, "text": "整租" }, { "value": 2, "text": "合租" }, { "value": 3, "text": "独栋" }];
            }
            $.each(options.data, function (index, value) {
                if (options.rdefault == value.value) {
                    result = '<span class="htcsspan i-radio-wrapper" htscvalue=' + value.value + '>' + value.text + '</span>';
                } else {
                    result = '<span class="htcsspan" htscvalue=' + value.value + '>' + value.text + '</span>';
                }
                _self.append(result);
            });

            _self.children().click(function (event) {
                $(this).siblings().removeClass("i-radio-wrapper");
                if ($(this).hasClass("i-radio-wrapper") == false) {
                    $(this).addClass("i-radio-wrapper");
                }
                var result = $(this).attr("htscvalue");
                cllback(result);
                event.stopPropagation();
            });
            return _self;
        },
        getnowdate: function (fmt) {
            var myDate = new Date();
            var o = {
                "M+": myDate.getMonth() + 1, //月份     
                "d+": myDate.getDate(), //日     
                "H+": myDate.getHours(), //小时     
                "m+": myDate.getMinutes(), //分     
                "s+": myDate.getSeconds(), //秒     
                "q+": Math.floor((myDate.getMonth() + 3) / 3), //季度     
                "S": myDate.getMilliseconds() //毫秒     
            };
            if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (myDate.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            return fmt;
        }
    };
    //输出test接口
    exports('htcsradio', obj);
}).addcss("htcsradio.css");


