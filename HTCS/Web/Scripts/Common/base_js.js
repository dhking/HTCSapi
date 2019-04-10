var image_url = "http://139.224.196.42:88/";
//$.ajax({
//    type: 'POST',
//    url: '../Home/getimageurl',
//    async:false,
//    success: function(data){
//        image_url = data;
//    }})


//得到编辑后的图片名称

Array.prototype.baoremove = function (index) {
    if (isNaN(index) || index >= this.length) {
        return false;
    }
    for (var i = 0, n = 0; i < this.length; i++) {
        if (this[i] != this[index]) {
            this[n++] = this[i];
        }
    }
    this.length -= 1;
};
//newadddelete 添加后删除的图片  deleteimg删除的原有图片 oroimage原有图片 addimage新增图片
//function get_finalname(newadddelete, deleteimg, oroimage,addimage) {
//    debugger;
//    var finlname = "";
//    $.unique(newadddelete);
//    $.each(newadddelete, function (index1, value) {
//        if (array_contain(addimage,value)) {
//            var index = addimage.indexOf(value);
//            addimage.baoremove(index);
//        }
//    });


//    var title = "";

//    $.each(deleteimg, function (index1, value) {
//        if (array_contain(oroimage, value)) {
//            var index = oroimage.indexOf(value);
//            oroimage.baoremove(index);
//        }
//    });
//    oroimage = oroimage.concat(addimage);
//    $.unique(oroimage);
//    oroimage.concat(addimage);
    
    

//    return oroimage;
//}
function get_add_image(newadddelete, addimage) {
    var final = "";
    $.each(newadddelete, function (index, value) {
        if (array_contain(addimage, value)) {
            var index = addimage.indexOf(value);
            addimage.baoremove(index);
        }
    });
    $.each(addimage, function (index, value) {
        final += value + ";";
    });
}
//newadddelete 添加后删除的图片  deleteimg删除的原有图片 oroimage原有图片 addimage新增图片
function get_finalname(newadddelete, deleteimg, oroimage, addimage) {
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
}
function array_contain(array, obj){
    for (var i = 0; i < array.length; i++){
        if (array[i] == obj)//如果要求数据类型也一致，这里可使用恒等号===
            return true;
    }
    return false;
}