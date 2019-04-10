debugger;
var namearr = [[ //表头
        { type: 'checkbox'}
      , { field: 'Id', width: 100, title: '编号' }
      , { field: 'RoleName', width: 120, title: '角色名称' }
        , { field: 'RoleDesc', width: 150, title: '描述' }
      , { field: 'PasswordExpiration', width: 200, title: '密码' }
     , { field: 'IsActive', width: 200, title: '是否激活' }
]];
        debugger;
        var table = new LoadTable(namearr, '/api/Sysrole/Querylistrole', ['893px', '600px'], false);
        $("#receivebtn").click(function () {
            debugger;
            var table1 = layui.table;
            var checkStatus = table1.checkStatus('idTest')
                    , getselect = checkStatus.data;
            if (getselect.length == 0) {
                layer.msg("请选择要编辑的数据");
                return;
            }
            if (getselect.length > 1) {
                layer.msg("不支持多选");
                return;
            }
            var ids = getselect[0].Id;
            layer.open({
                type: 2,
                title: '收款',
                skin: 'two-layer',
                //anim: 4,
                shadeClose: true,//开启遮罩关闭
                //shade: ['0.5'],
                maxmin: true, //开启最大化最小化按钮
                area: ['1000px', '900px'],
                content:$("#receivebtn").attr("hturl")+"?Id=" + ids //注意，如果str是object，那么需要字符拼接。
            });
        });
        $("#Permissions").click(function () {
            debugger;
            layer.open({
                type: 2,
                title: '分配权限',
                skin: 'two-layer',
                //anim: 4,
                shadeClose: true,//开启遮罩关闭
                //shade: ['0.5'],
                maxmin: true, //开启最大化最小化按钮
                area: ['1000px', '900px'],
                content: $("#Permissions").attr("hturl") + "?Id=" + ids //注意，如果str是object，那么需要字符拼接。
            });
        });
        $("#search").click(function () {
            debugger;
            $('#table').bootstrapTable("refresh");
        });
        function formatterstatus(value) {
            if (value == 1) {
                return '<div><span style="padding:10px;background-color:#1bb974;color:#ffffff;border-radius:5px;">在租中</span></div>'
            } else {
                return '<div><span style="padding:10px;background-color:#ff4b5e;color:#ffffff;border-radius:5px;">已预订</span></div>';
            }
            
        }
        function formatterhouse(value) {
            var adress=value.CellName + "-" + value.BuildingNumber + "栋" + value.RoomId + "室";
            if(value.HouseName!=""&&value.HouseName!=null){
                adress+="-"+value.HouseName;
            }
            return '<div>' + adress + '</div>'
        }
        function formattertime(value) {
         
            return '<div>' + value.BeginTime + "→" + value.EndTime + '</div>';
        }