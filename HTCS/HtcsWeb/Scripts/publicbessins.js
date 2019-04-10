var apiurl;
layui.use(['laypage', 'layer', 'htcsradio', 'laytpl', 'jquery', 'form','htcsLG'], function () {
        var laypage = layui.laypage
        , layer = layui.layer;
        var laytpl = layui.laytpl;
        var $ = layui.$;
        var getTpl = demo.innerHTML
      , view = document.getElementById('view');
        var mymod = layui.htcsradio;
        var doc = layui.htcsLG;
        var $ = layui.jquery;
        apiurl =layui.setter.baseurl;
        var url ='/api/House/Queryhouselist';
        var paradata = { "PageSize": 10, "PageIndex": 1 };
        var option = { data: [{ "value": 0, "text": "全部" }, { "value": 2, "text": "已租" }, { "value": 3, "text": "未租" }], rdefault: 0 };
        mymod.CreateInput($("#house-othersrarch"), option, function (result) {

        });
        doc.objectQuery(url, paradata, function (data) {
                debugger;
                //调用分页
                laypage.render({
                    elem: 'fy'
               , count: data.numberCount //数据总数，从服务端得到
               , jump: function (obj, first) {
                   if (!first) {
                       var pagedata = { "PageIndex": obj.curr, "PageSize": obj.limit, ID: 0 };
                       doc.objectQuery(url, pagedata, function (data1) {
                           laytpl(getTpl).render(data1.numberData, function (html) {
                               view.innerHTML = html;
                               InitEvent();
                           })
                       });
                   } else {
                       laytpl(getTpl).render(data.numberData, function (html) {
                           view.innerHTML = html;
                           InitEvent();
                       });
                   }
               }
                });
            });
    });
var testiframe;
function ParentOpen(url,paraarea) {
    layui.use(['layer'], function () {
        layer.open({
            type: 2,
       
            title: '编辑',
            skin: 'two-layer',
            shadeClose: true,//开启遮罩关闭
            maxmin: true, //开启最大化最小化按钮
            area: paraarea,
            content: url//注意，如果str是object，那么需要字符拼接。,
           
        });
    }); 
}
function reflash() {
    debugger;
    var doc = testiframe.find("#iframe_oBasicInfo");
    doc.attr('src', doc.attr('src'));
   
    
}
function layts(msg) {
    debugger;
    layer.msg(msg, {
        icon: 1,
        time: 800 //2秒关闭（如果不配置，默认是3秒）
    });
}
function InitEvent() {
    debugger;
    var bodyheight = $(".layui-body").height();
    var tableheight = bodyheight - $(".m-search").height() - $("#fy").height() - 40;
    $("#view").css("height", tableheight);
    var thisbody = $(this).parent(".m-panel-title");
    $(".m-row").click(function () {
        debugger;
        //alert($(this).html());
        var idvalue = $(this).attr("id");
        var classvalue = $(this).attr("class");
        var index = getIndex(idvalue, classvalue);
        if (index != 0) {
            var body = $("#m-wrap-body-" + index);
            body.slideToggle();
        } else {
            $(".m-wrap-body").slideToggle();
        }

    });
    $("#img").click(function () {
        layer.closeAll();
        layer.open({
            type: 2,
            title: '图片上传插件',
            skin: 'two-layer',
            shadeClose: true,
            maxmin: true,
            area: ['920px', '600px'],
            content: 'http://localhost:4411/Upload/Index'
        });
    });
    $(".addhouse").click(function () {
        debugger;
        var url = apiurl + "/api/House/adddepentHouse";
        var index = $(this).attr("id").replace("addhouse-", "");
        var $id = $("#m-wrap-body-" + index).children(".m-panel-body:last");
        var parentid = $("#houseid-" + index).html();
        var paradata = { "id": parentid };
        doc.Submit1(url, paradata, function (data) {
            debugger;
            var html = "<div class='m-panel-body' id='panel-body-" + data.numberData.ID + "' ><div class='m-top m-zt-k' id='m-top'><ul><li><span>" + data.numberData.ID + "</span>-<span>" + data.numberData.Name + "</span><span class='caidan-icon'  id='caidan-icon-" + data.numberData.ID + "' ><i class='layui-icon' id='lay-ic-" + data.numberData.ID + "' style='font-size:22px;'  >&#xe65f;</i></span></li><ul class='project-span-box span-box' id='span-box-" + data.numberData.ID + "'><li style='line-height: 30px;height: 30px;' class='deletedepent'  onclick='deleteevent(" + data.numberData.ID + ");'><a data='18220' class='editApartment' >删除小区</a></li><li style='line-height: 30px;height: 30px;' class='editdepent' ><a class='view-del-project'>编辑小区</a></li></ul></li></li><li><span>暂未定价</span></li><li><span>房型</span><span>面积</span><span>朝向</span></li><li><span>空置</span><span class='m-red'>1天</span></li></ul></div></div>";
            $id.after(html);
            $(".caidan-icon").mouseover(function () {
                debugger;
                var index = $(this).attr("id").replace("caidan-icon-", "");
                $(".span-box").hide();
                $("#span-box-" + index).show();

            });
            $(".span-box").mouseleave(function () {
                $(this).hide();
            });
            ViewEvent();
        });
    });
    $(".caidan-icon").mouseover(function () {
        debugger;
        var index = $(this).attr("id").replace("caidan-icon-", "");
        $(".span-box").hide();
        $("#span-box-" + index).show();

    });
    $(".span-box").mouseleave(function () {
        $(this).hide();
    });
    ViewEvent();
    
}
function getIndex(idvalue,classvalue) {
    var index = idvalue.replace(classvalue + "-", "");
    return index;
}
function deleteevent(obj) {
    debugger;
    var url = apiurl + "/api/House/deletedepentHouse";
    doc.deletetable1(url, { "id": obj }, function () {
        $("#panel-body-" + obj).remove();
    });
};
function ViewEvent() {

    $(".m-panel-body").not(".layui-icon").dblclick(function () {
        debugger;
        var id = $(this).attr("id").replace("panel-body-", "");
        var parentid = $(this).parent(".m-wrap-body").attr("parentid");
        layer.open({
            type: 2,
            title: '查看',
            skin: 'two-layer',
            anim: -1,
            offset: 'r',
            shade: .1,
            shadeClose: true,
            maxmin: true,
            area: ['820px', '1100px'],
            skin: "layui-anim layui-anim-rl layui-layer-adminRight",
            content: '/House/InterView?ParentRoomid=' + parentid + '&Id=' + id,
            success: function (layero, index) {
                debugger;
                testiframe = layer.getChildFrame('body', index); //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();
               
            }
        });
    });
    $(".editdepent").click(function (index,value) {
        layer.open({
            type: 2,
            title: '图片上传插件',
            skin: 'two-layer',
            shadeClose: true,
            maxmin: true,
            area: ['920px', '600px'],
            content: 'http://localhost:4411/Upload/Index'
        });
    });

}
