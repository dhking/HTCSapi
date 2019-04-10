using DAL.Common;
using Jiguang.JPush;
using Jiguang.JPush.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Mapping.cs;
using Model.Base;
using ControllerHelper;
using DBHelp;
using System.Linq.Expressions;

namespace DAL
{
    public   class PushDAL : RcsBaseDao
    {
        private static JPushClient client = new JPushClient("3631cd3d92737efd24604d35", "df430c9046262733ab2c1e41");

        //推送系统消息
        public string  ExecutePushExample(ParamPhsh param)
        {
            PushPayload pushPayload = new PushPayload()
            {
                Platform = new List<string> { "android", "ios" },
                Audience = "all",
                Notification = new Notification
                {
                    Alert = param.Content,
                    Android = new Android
                    {
                        Alert = param.Content,
                        Title = "",
                        Extras = new Dictionary<string, object>
                        {
                            ["Url"] = param.Url,
                            ["Type"] = "1",
                        }
                    },
                    IOS = new IOS
                    {
                        Alert = param.Content,
                        Badge = "+1",
                        Extras = new Dictionary<string, object>
                        {
                            ["Url"] = param.Url,
                            ["Type"] = "1"
                        }
                    }
                   
                },
                Message = new Message
                {
                    Title = param.Content,
                    Content = param.Content,
                    Extras = new Dictionary<string, string>
                    {
                        ["Url"] = param.Url,
                        ["Type"]="1"
                    }
                },
                Options = new Options
                {
                    IsApnsProduction = false  // 设置 iOS 推送生产环境。不设置默认为开发环境。
                }
            };
            var response = client.SendPush(pushPayload);
            return response.Content;
        }
        //注册
        public   string  ExecuteDeviceEample(ParamPhsh param)
        {
            var registrationId = param.deviceid;
            var devicePayload = new DevicePayload
            {
                Alias = param.Alias,
                Mobile = param.Mobile,
                Tags = new Dictionary<string, object>
                {
                    { "add", new List<string>() { "tag1", "tag2" } },
                    { "remove", new List<string>() { "tag3", "tag4" } }
                }
            };
            var response = client.Device.UpdateDeviceInfo(registrationId, devicePayload);
            return response.Content;
        }

        private static void ExecuteReportExample()
        {
            var response = client.Report.GetMessageReport(new List<string> { "1251231231" });
            Console.WriteLine(response.Content);
        }

        private static void ExecuteScheduleExample()
        {
            var pushPayload = new PushPayload
            {
                Platform = "all",
                Notification = new Notification
                {
                    Alert = "Hello JPush"
                }
            };
            var trigger = new Trigger
            {
                StartDate = "2017-08-03 12:00:00",
                EndDate = "2017-12-30 12:00:00",
                TriggerTime = "12:00:00",
                TimeUnit = "week",
                Frequency = 2,
                TimeList = new List<string>
                {
                    "wed", "fri"
                }
            };
            var response = client.Schedule.CreatePeriodicalScheduleTask("task1", pushPayload, trigger);
            Console.WriteLine(response.Content);
        }
        public List<T_SysMessage> Queryfy(T_SysMessage model, OrderablePagination orderablePagination)
        {
            var data = from m in SysMessage select m;
            IOrderByExpression<T_SysMessage> order = new OrderByExpression<T_SysMessage, long>(p => p.Id, false);
            Expression<Func<T_SysMessage, bool>> where = m => 1 == 1;
            List<T_SysMessage> list = QueryableForList(data, orderablePagination, order);
            return data.ToList();
        }
        public DbSet<T_SysMessage> SysMessage { get; set; }
        protected override void CreateModelMap(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SysMessageMapping());
        }
    }
}
