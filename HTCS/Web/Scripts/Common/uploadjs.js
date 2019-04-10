layui.use('upload', function () {
                var imagearr = [];
                var index;
                var $ = layui.jquery
                , upload = layui.upload;
                var currindex;
                var uploadInst;
                Initimg();
                removeEvent();
                //多图片上传
                uploadInst=upload.render({
                   elem: '#test2'
                  ,url: '/Upload/uploadimage'
                  ,multiple: true
                  ,before: function(obj){
                      //预读本地文件示例，不支持ie8
                      index = layer.load(1);
                      obj.preview(function (index, file, result) {
                          debugger;
                          if (currindex == index) {
                              return;
                          }
                          currindex = index;
                          var html = "<div class='col-6-24 item' id=item-" +index+ "><span class='error'>上传失败</span><span class='cc' id='cc-"+index+"'><button class='layui-btn layui-btn-sm btcc'>重新上传</button></span><img src='" + result + "' alt='" + file.name + "' class='layui-upload-img'><i class='layui-icon remove' style='font-size: 30px;color:#FF5722;'>&#x1007;</i></div>"
                          $('#demo2').append(html);
                          removeEvent();
                          $("#cc-" + index).click(function (event) {
                              debugger;
                             
                              obj.upload(index, file);
                            
                              //event.stopPropagation();
                          });
                      });
                  }
                  ,done: function (res){
                      debugger;
                      if (res.code == 0) {
                          imagearr.push(res.Name);
                          $("#item-" + currindex).append('<i class="layui-icon success" style="font-size: 30px;color:#5FB878;">&#x1005;</i>');
                          $("#item-" + currindex + " .cc").hide();
                          $("#item-" + currindex + " .error").hide();
                          
                      } else {
                          $("#item-" + currindex).append('');
                          //对上传失败的单个文件重新上传，一般在某个事件中使用
                          
                      }
                      layer.close(index);
                      return true;
                      //上传完毕
                  }, error: function (index, upload) {
                      layer.close(index);
                  
                  }
                });
              
               
                function removeEvent() {
                    $(".remove").click(function () {
                        debugger;
                        var removeItem = "";
                        removeItem = $(this).prev("img").attr("alt");
                        $(this).parent().remove();
                        imagearr = $.grep(imagearr, function (value) {
                            return value != removeItem;
                        });
                    });
                }
                function Initimg() {
                    debugger;
                    var name = getUrlParam("img");
                    if (name != null && name != "undefined" && name != "") {
                        imagearr = getarr(name);
                        var imgurl = "";
                        doc.initimg(function (result) {
                            imgurl = result;
                        });
                        $.each(imagearr, function (index, value) {
                            var html = '<div class="col-6-24 item"><img src="' + imgurl+value + '" alt="' + value + '"  class="layui-upload-img"><i class="layui-icon remove" style="font-size: 30px;color:#FF5722;">&#x1007;</i><i class="layui-icon success" style="font-size: 30px;color:#5FB878;">&#x1005;</i></div>';
                            $('#demo2').append(html);
                        });
                    }
                };
                function getUrlParam(name) {
                 var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
                 var r = window.location.search.substr(1).match(reg);  //匹配目标参数
                  if (r != null) return unescape(r[2]); return null; //返回参数值
                }
                function getarr(name) {
                    name = name.substr(0, name.length - 1);
                    return name.split(";");
                }
                function getstr(arr) {
                    var revalue = "";
                    $.each(arr, function (index,value) {
                        revalue+=value+";";
                    });
                    return revalue;
                }
                $("#cancel").click(function () {
                    debugger;
                    var index = parent.layer.getFrameIndex(window.name);
                    parent.layer.close(index);
                });
                $("#save").click(function () {
                    debugger;
                    var revalue = getstr(imagearr);
                    parent.complteimg(revalue, imagearr.length);
                    var index = parent.layer.getFrameIndex(window.name);
                    parent.layer.close(index);
                });
                
});
