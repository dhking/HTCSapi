/// <reference path="House.js" />
/*****************************************************************************
日期:2017-10-09
作者:董行
描述:自动生成checkbox
*******************************************************************************/
var  House={
    
    InitImage :function(url, document) {
        debugger;
        var baseimage = "~/FILE/";
        $(document).each(function (i, item) {
            eval("addimage" + i + "= new Array()");
            eval("deleteimg" + i + "= new Array()");
            eval("newadddelete" + i + "= new Array()");
            eval("oroimage" + i + "= new Array()");
            var id = "#" + item.id;
            $(id).diyUpload({
                url: url,
                success: function (data, result) {
                    debugger;
                    if (result._raw == "False") {
                        $.messager.alert("温馨提示", "上传失败");
                        return;
                    }
                    $.each(data, function (index, itrm) {
                        eval("addimage" + i).push(data[index].name);
                    });
                },
                error: function (err) {
                    $.alert.message("温馨提示", "上传失败" + err);
                    console.info(err);
                }
            });
            //var initimg = $("#" + item.id + "_KG").val();
            //if (initimg != "") {
            //    var image = initimg.substring(0, initimg.length - 1);
            //    var arr = image.split(';');
            //    var xs = "";
            //    if (arr.length > 0) {
            //        $.each(arr, function (index, value) {
            //            eval("oroimage" + i).push(value);
            //            xs += '<li id="fileBox_WU_FILE_0" class="diyUploadHover" style="float:left;"><div class="viewThumb"><img src="' + baseimage + value + '" /></div><div class="diyCancelh' + i + ' diyCancel3" ></div><div class="diyFileName">' + value + '</div><div class="diyBar"><div class="diyProgress"></div><div class="diyProgressText">0%</div></div>';
            //        });
            //    }
            //    if (xs != "") {
            //        $("#document" + i + " .fileBoxUl").html(xs);
            //    }
            //    $(".diyCancelh" + i).click(function () {
            //        debugger;
            //        var filename = $(this).next().html();
            //        $(this).parent(".diyUploadHover").remove();
            //        eval("deleteimg" + i).push(filename);

            //        //加载数据
            //    });
            //}
        });
    },
    SavePath :function () {
        debugger;
        $(".Diyimagefile").each(function (i, item) {
            var imagePath = get_finalname(eval("newadddelete" + i), eval("deleteimg" + i), eval("oroimage" + i), eval("addimage" + i));
            $("#" + item.id + "_KG").val(imagePath);
        });
    },
     get_finalname : function (newadddelete, deleteimg, oroimage, addimage) {
        debugger;
        var finlname = "";
        $.unique(newadddelete);
        $.each(newadddelete, function (index1, value) {
            if (array_contain(addimage, value)) {
                var index = addimage.indexOf(value);
                addimage.baoremove(index);
            }
        });
        var title = "";
        $.each(deleteimg, function (index1, value) {
            if (array_contain(oroimage, value)) {
                var index = oroimage.indexOf(value);
                oroimage.baoremove(index);
            }
        });
        oroimage = oroimage.concat(addimage);
        $.unique(oroimage);
        oroimage.concat(addimage);
        var final = "";
        $.each(oroimage, function (index, value) {
            final += value + ";";
        });
        return final;
    },
    array_contain : function (array, obj) {
        for (var i = 0; i < array.length; i++) {
            if (array[i] == obj)//如果要求数据类型也一致，这里可使用恒等号===
                return true;
        }
        return false;
    }
}

debugger;
var uploadurl = "../Upload/uploadimage";
var house = Object.create(House);
house.InitImage(uploadurl, "#wrap .Diyimagefile");

