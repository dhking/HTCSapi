using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.WebSockets;
using Newtonsoft.Json;
using DBHelp;
using Service;
using Model.User;

namespace Api.Controllers
{
    public class MessageInfo
    {
        public MessageInfo(DateTime _MsgTime, ArraySegment<byte> _MsgContent)
        {
            MsgTime = _MsgTime;
            MsgContent = _MsgContent;
        }
        public DateTime MsgTime { get; set; }
        public ArraySegment<byte> MsgContent { get; set; }
    }
    //和服务端建立连接
    public class WSChatController : ApiController
    {
        LogService lo = new LogService();
        private static List<WebSocket> _sockets = new List<WebSocket>();
        public HttpResponseMessage Get(FinanceModel bill)
        {
            lo.LogError("开始连接");

            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(ProcessChat);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }
        private async Task ProcessChat(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            string user = HttpContext.Current.Request["UserId"].ToString();

            try
            {
                #region 用户添加连接池
                //第一次open时，添加到连接池中
                if (!websocket.CONNECT_POOL.ContainsKey(user))
                    websocket.CONNECT_POOL.Add(user, socket);//不存在，添加
                else
                    if (socket != websocket.CONNECT_POOL[user])//当前对象不一致，更新
                    websocket.CONNECT_POOL[user] = socket;
                #endregion

                #region 离线消息处理
                if (websocket.LXCONNECT_POOL.ContainsKey(user))
                {
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                    List<string> msgs = websocket.LXCONNECT_POOL[user];
                    foreach (string item in msgs)
                    {
                        buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(item));
                        await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    websocket.LXCONNECT_POOL.Remove(user);//移除离线消息
                }
                #endregion
                while (true)
                {
                    if (socket.State == WebSocketState.Open)
                    {
                        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[2048]);
                        WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                        #region 消息处理（字符截取、消息转发）
                        try
                        {
                            #region 关闭Socket处理，删除连接池
                            if (socket.State != WebSocketState.Open)//连接关闭
                            {
                                if (websocket.CONNECT_POOL.ContainsKey(user)) websocket.CONNECT_POOL.Remove(user);//删除连接池
                                break;
                            }
                            #endregion
                            string userMsg = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);//发送过来的消息
                            //接收到心跳重新加入连接池
                            Socket soc = JsonConvert.DeserializeObject<Socket>(userMsg);
                            if (!websocket.CONNECT_POOL.ContainsKey(soc.UserId))
                            {
                                lo.LogError("我在心跳时加入了队列。。。。" + user);
                                websocket.CONNECT_POOL.Add(soc.UserId, socket);
                            }
                            string returnMessage = "";
                            Socket re = new Socket();
                            re.Type = "1";
                            re.Code = "0";
                            re.UserId = user;
                            returnMessage = JsonConvert.SerializeObject(re);
                            buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(returnMessage));
                            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        catch (Exception exs)
                        {
                            //消息转发异常处理，本次消息忽略 继续监听接下来的消息
                        }
                        #endregion
                    }
                    else
                    {
                        break;
                    }
                }//while end
            }
            catch (Exception ex)
            {
                //整体异常处理
                if (websocket.CONNECT_POOL.ContainsKey(user)) websocket.CONNECT_POOL.Remove(user);
            }
        }
        private async Task ProcessWSChat(AspNetWebSocketContext context)
        {
            lo.LogError("开始连接。。。。");
            WebSocket socket = context.WebSocket;
            string userid = HttpContext.Current.Request["UserId"].ToString();
            #region 离线消息处理
            if (websocket.LXCONNECT_POOL.ContainsKey(userid))
            {
                List<string> msgs = websocket.LXCONNECT_POOL[userid];
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                foreach (string item in msgs)
                {
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(item));
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                websocket.LXCONNECT_POOL.Remove(userid);//移除离线消息
            }
            #endregion
            if (!websocket.CONNECT_POOL.ContainsKey(userid))
            {
                lo.LogError("我加入队列了。。。。" + userid);
                websocket.CONNECT_POOL.Add(userid, socket);
            }
            else
            {
                lo.LogError("我更新队列了。。。。" + userid);
                if (socket != websocket.CONNECT_POOL[userid])
                {
                    websocket.CONNECT_POOL[userid] = socket;
                }
            }
            try
            {
                //while (true)
                //{
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                    //开始接收会话
                    WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await socket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);//如果client发起close请求，对client进行ack
                        _sockets.Remove(socket);
                     
                    }
                    if (socket.State == WebSocketState.Open)
                    {
                        string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                        //接收到心跳重新加入连接池
                        Socket soc = JsonConvert.DeserializeObject<Socket>(message);
                        if (!websocket.CONNECT_POOL.ContainsKey(soc.UserId))
                        {
                            lo.LogError("我在心跳时加入了队列。。。。" + userid);
                            websocket.CONNECT_POOL.Add(soc.UserId, socket);
                        }
                        string returnMessage = "";
                        Socket re = new Socket();
                        re.Type = "1";
                        re.Code = "0";
                        re.UserId = userid;
                        returnMessage = JsonConvert.SerializeObject(re);
                        buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(returnMessage));
                        await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else
                    {
                        if (websocket.CONNECT_POOL.ContainsKey(userid)) websocket.CONNECT_POOL.Remove(userid);//删除连接池
                    }
                //}
            }
            catch (Exception ex)
            {

            }
        }
        public class GetJson
        {
            public string GetquanxianJson(long userid)
            {
                RoleDAL roledal = new RoleDAL();
                UserDAL1 dal = new UserDAL1();
                SysUserService service = new SysUserService();
                List<T_SysUserRole> listrole = dal.listrole(userid);
                T_SysUser user = new T_SysUser();
                List<long> roleids = listrole.Select(p => p.SysRoleId).ToList();
                user.roles = roledal.queryrole(roleids);
                user.listpression = service.Querybasepressionbuuser(roleids).numberData;
                string neft = JsonConvert.SerializeObject(user);
                return neft;
            }
        }

    }
}