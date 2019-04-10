layui.use(['table', 'htcsradio', 'form', 'htcsLG', 'laydate', 'console'], function () {
            var doc = layui.htcsLG;
            var $ = layui.jquery
            var url = "api/Formatter/QueryCell";
            var areaData = [];
            var form = layui.form;
            var $form= $('form');
            doc.objectQuery(url, {}, function (result) {
                debugger;
                areaData = result.numberData;
                loadProvince();
            });
            function loadProvince() {
                var proHtml = '';
                var FirstValue = '';
                for (var i = 0; i < areaData.length; i++) {
                    if (i == 0) {
                        FirstValue = areaData[i].provinceCode + '_' + areaData[i].mallCityList.length + '_' + i;
                        proHtml += '<option selected value="' + areaData[i].provinceCode + '_' + areaData[i].mallCityList.length + '_' + i + '">' + areaData[i].provinceName + '</option>';
                    } else {
                        proHtml += '<option  value="' + areaData[i].provinceCode + '_' + areaData[i].mallCityList.length + '_' + i + '">' + areaData[i].provinceName + '</option>';
                    }
                }
                //初始化省数据
                $form.find('select[name=province]').append(proHtml);
                form.render();
                var vd = FirstValue.split('_');
                var vcode = vd[0];
                var vcount = vd[1];
                var vindex = vd[2];
                if (vcount > 0) {
                    loadCity(areaData[vindex].mallCityList);
                } else {
                    $form.find('select[name=city]').attr("disabled", "disabled");
                }
                form.on('select(province)', function (data) {
                    $form.find('select[name=area]').html('<option value="">所有小区</option>');
                    var value = data.value;
                    var d = value.split('_');
                    var code = d[0];
                    var count = d[1];
                    var index = d[2];
                    if (count > 0) {
                        loadCity(areaData[index].mallCityList);
                    } else {
                        $form.find('select[name=city]').attr("disabled", "disabled");
                    }
                });
            }
            //加载市数据
            function loadCity(citys) {
                debugger;
                var cityHtml = '';
                var cityHtml = '<option value="">请选择区</option>';
                for (var i = 0; i < citys.length; i++) {         
                    cityHtml += '<option  value="' + citys[i].cityCode + '_' + citys[i].mallAreaList.length + '_' + i + '">' + citys[i].cityName + '</option>';
                }
                $form.find('select[name=city]').html(cityHtml).removeAttr("disabled");
                form.render();
                form.on('select(city)', function (data) {
                    var value = data.value;
                    var d = value.split('_');
                    var code = d[0];
                    var count = d[1];
                    var index = d[2];
                    if (count > 0) {
                        loadArea(citys[index].mallAreaList);
                    } else {
                        $form.find('select[name=area]').attr("disabled", "disabled");
                    }
                });
            }
            //加载县/区数据
            function loadArea(areas) {
                var areaHtml = '<option value="">所有小区</option>';
                for (var i = 0; i < areas.length; i++) {
                    areaHtml += '<option value="' + areas[i].areaCode + '">' + areas[i].areaName + '</option>';
                }
                $form.find('select[name=area]').html(areaHtml).removeAttr("disabled");
                form.render();
                form.on('select(area)', function (data) { });
            }
});
