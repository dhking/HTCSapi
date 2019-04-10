using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Utils;
using Burgeon.Wing3.Release.Http;
using Burgeon.Wing3.Release.Models;
using Burgeon.Wing3.Release.StepLog;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 已唤醒方式进行项目版本文件发送
    /// 注意：与SendResourceCommand不同的是
    /// AwakenSendResourceCommand 只是发送版本信息到客户端
    /// 然后客户端 AwakenReceiveResourceCommand根据版本信息 
    /// 自主下载版本文件进行 并且唤醒升级命令 进行升级
    /// </summary>
    public class AwakenSendResourceCommand : BaseCommand
    {
        private ILogger log = null;

        public override ILogger Logger
        {
            get
            {
                return log;
            }
        }

        public override string Command
        {
            get
            {
                return "awaken-send";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult eResult = null;

            Client client = null;
            try
            {
                client = context.GetParam<Client>("Client");
                AppVersion version = context.GetParam<AppVersion>("Version");
                string uRL = this.Environment.GetCommandURL(client.COMPANY_API, "awaken-receive");

                this.InitLogger(client.Id,version.Version);

                Logger.LogDebug("开始初始化...");

                version.UserId=this.User.UserId;
                version.Client = client;
                version.MainHost = this.Environment.GetMainHostURL();
                version.VersionDownloadURL = this.GetVersionDownloadURL(version.Version);

                VersionRequest request = new VersionRequest(uRL, RequestType.POST);
                //发送版本信息
                request.SetPostParam("version", Newtonsoft.Json.JsonConvert.SerializeObject(version));

                Logger.LogDebug("初始化完毕，开始通知客户服务器...");

                //开始发送
                VersionResponse response = request.Request();
                eResult = response.CommandResult;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                eResult = new CommandResult(ResultStatus.Error, ex);
            }
            finally
            {
                if (client != null)
                {
                    this.ResponseRender(eResult, client.Id);
                }
            }
            return eResult;
        }

        private void InitLogger(int companyid,string version)
        {
            log = new VersionRealLogger(companyid, this.User.UserId, this.User.UserName,version);
        }

        private string GetVersionDownloadURL(string v)
        {
            string host = this.Environment.GetMainHostURL();
            if (!host.StartsWith("http:"))
            {
                host = "http://" + host; ;
            }
            return string.Format("{0}/release/version?v={1}", host, v);
        }

        private void ResponseRender(CommandResult cResult, int company)
        {
            if (cResult.Status == ResultStatus.Success)
            {
                this.Logger.LogDebug(string.Format("已成功通知客户服务器,反馈消息：{0}", cResult.Message));
            }
            else if (cResult.Status == ResultStatus.InValid)
            {
                this.Logger.LogError(string.Format("通知期间检验失败，原因：{0}", cResult.Message));
            }
            else
            {
                this.Logger.LogError(string.Format("通知客户服务器失败,原因：{0}", cResult.Message));
            }
        }
    }
}
