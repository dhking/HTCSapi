layui.use('form', function () {
    //如果有需要到镇的改改就可以实现
    var form = layui.form(),
    layer = layui.layer,
    $ = layui.jquery;
    var url = 'getArea.htm'; //数据请求地址
    var province = "广东省"; //编辑需要的省
    var city = "广州市"; //编辑需要的市
    var district = "天河区"; //编辑需要的县/区
    getJSON(url, $("select[name='province']").closest("div"));
    form.on('select(state)', function (data) {
        $that = $(data.elem);
        urls = url + "?id=" + data.value;
        getJSON(urls, $that.closest("div").next());
    });
    function getJSON(urls, even) {
        $.getJSON(urls, function (json) {
            var pid = 0;
            var name = even.find("select").attr("name");
            var select = "<select name=\"" + name + "\" lay-filter=\"state\">";
            select += "<option value=\"0\">请选择 </option>";
            $(json).each(function () {
                select += "<option value=\"" + this.id + "\"";
                if (province == this.value || city == this.value || district == this.value) {
                    select += " selected=\"selected\" ";
                    pid = this.id;
                }
                select += ">" + this.value + "</option>";
            });
            select += "</select>";
            even.html(select);
            var nextName = even.next().find("select").attr("name");
            even.next().html("<select name=\"" + nextName + "\" lay-filter=\"state\"><option value=\"0\">请选择 </option></select>");
            form.render('select');
            if (pid != 0) {
                getJSON(url + "?id=" + pid, even.next());
            }
        });
    }
});