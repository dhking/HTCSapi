using Api.CommonControllers;
using API.CommonControllers;
using ControllerHelper;
using DAL;
using Microsoft.Owin;
using Model;
using Model.Base;
using Model.Bill;
using Model.User;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.WebSockets;
using DBHelp;

namespace Api.Controllers
{
    public class FinanceController : DataCenterController
    {
        FinanceService service = new FinanceService();
        //财务流水分页查询
        
        [JurisdictionAuthorize(name = new string[] { "liushui/" })]
        [Route("api/Finance/Querylist")]
        public SysResult<List<WrapFinanceModel>> Querylist(FinanceModel model)
        {
            SysResult<List<WrapFinanceModel>> sysresult = new SysResult<List<WrapFinanceModel>>();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            model.CompanyId = user.CompanyId;
            InitPage(model.PageSize, (model.PageSize * model.PageIndex));
            sysresult = service.Query(model, this.OrderablePagination);
            return sysresult;
        }
        //财务流水详情
        [Route("api/Finance/Queryxq")]
        public SysResult<WrapFinanceModel> Queryxq(FinanceModel model)
        {
            SysResult<WrapFinanceModel> sysresult = new SysResult<WrapFinanceModel>();

            sysresult = service.Queryxq(model);
            return sysresult;
        }
       
        //保存财务流水
        [HttpPost]
        [Route("api/Finance/save")]
        public SysResult save(FinanceModel bill)
        {
            SysResult sysresult = new SysResult();
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user == null)
            {
                sysresult.Code = 1002;
                sysresult.Message = "请先登录";
                return sysresult;
            }
            bill.CompanyId = user.CompanyId;
            return service.save(bill);
        }
        //意见反馈
        [HttpPost]
        [Route("api/feeback/save")]
        public SysResult feebacksave(feedback bill)
        {
            T_SysUser user = GetCurrentUser(GetSysToken());
            if (user!= null)
            {
                bill.userid = user.Id;
                bill.phone = user.Mobile;
            }
            return service.feeback(bill);
        }
        //测试
        [HttpPost]
        [Route("api/Finance/test")]
        public SysResult test(T_SysUser model)
        {
            SysResult result = new SysResult();
            initgwService service = new initgwService();
            result = service.query(1);
            //try
            //{


            //    WebSocket socket = websocket.CONNECT_POOL[model.Id.ToStr()];
            //    UserDAL1 dal = new UserDAL1();
            //    RoleDAL roledal = new RoleDAL();
            //    SysUserService service = new SysUserService();
            //    List<T_SysUserRole> listrole = dal.listrole(model.Id);
            //    T_SysUser user = new T_SysUser();
            //    List<long> roleids = listrole.Select(p => p.SysRoleId).ToList();
            //    user.roles = roledal.queryrole(roleids);
            //    user.listpression = service.Querybasepressionbuuser(roleids).numberData;
            //    string neft = JsonConvert.SerializeObject(user);
            //    Socket resocket = new Socket();
            //    resocket.UserId = model.Id.ToStr();
            //    resocket.Type = "2";
            //    resocket.Value = neft;
            //    string neft2 = JsonConvert.SerializeObject(resocket);
            //    if (websocket.CONNECT_POOL.ContainsKey(model.Id.ToStr()))//判断客户端是否在线
            //    {
            //         ProcessWSChat(socket, neft2, model.Id.ToStr());
            //    }
            //    else
            //    {
            //        Task.Run(() =>
            //        {
            //            if (!websocket.CONNECT_POOL.ContainsKey(model.Id.ToStr()))//将用户添加至离线消息池中
            //                websocket.LXCONNECT_POOL.Add(model.Id.ToStr(), new List<string>());
            //            websocket.LXCONNECT_POOL[model.Id.ToStr()].Add(neft2);//添加离线消息
            //        });
            //    }


            //}
            //catch(Exception ex)
            //{

            //}

            return result;
        }
        private async Task ProcessWSChat(WebSocket socket,string str,string userid)
        {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
            buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes("现在能接受到吗"));
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            if (socket.State == WebSocketState.Open)
                {
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(str));
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                //如果没有连接
                if (websocket.CONNECT_POOL.ContainsKey(userid)) websocket.CONNECT_POOL.Remove(userid);//删除连接池
                //将用户添加至离线消息池中
                if (!websocket.LXCONNECT_POOL.ContainsKey(userid)) {
                    websocket.LXCONNECT_POOL.Add(userid, new List<string>());
                    websocket.LXCONNECT_POOL[userid].Add(str);//添加离线消息

                }
                else
                {
                    websocket.LXCONNECT_POOL[userid].Add(str);//添加离线消息
                }

               
            }
        }
    }
}