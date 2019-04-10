using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Models;

namespace Burgeon.Wing3.Release.Environment.Commands
{
    /// <summary>
    /// 发布版本命令
    /// </summary>
    public class VersionUpgradeCommand : BaseCommand
    {
        public override string Command
        {
            get
            {
                return "upgrade";
            }
        }

        public override CommandResult Execute(CommandContext context)
        {
            CommandResult cResult = null;

            AppVersion version = context.GetParam<AppVersion>("version");

            try
            {
                cResult = Upgrade(BuildRunContext(version));
            }
            catch (Exception ex)
            {
                cResult = new CommandResult(ResultStatus.Error, ex.ToString());
            }

            return cResult;
        }

        private CommandResult Upgrade(RunContext runContext)
        {
            string app = AppDomain.CurrentDomain.BaseDirectory;
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(app);
            runContext.Client.BaseDIR = info.Parent.FullName;

            string args = Newtonsoft.Json.JsonConvert.SerializeObject(runContext);
            string run = Utils.CmdUtil.GetTool("upgrade.exe", "");
            string path = Utils.CmdUtil.GetTool("version.json", "");
            if (Utils.IOUtil.ExistsFile(run))
            {
                //写出升级元数据
                System.IO.File.WriteAllText(path, args);
                //启动错误监听
                Sandbox.SandboxRunErrorListener.Instance.Start(runContext);
                //通知升级程序启动
                bool r = Sandbox.SandboxNotifyRun.Notify(runContext.Version, run, path);
                if (r)
                {
                    return new CommandResult(ResultStatus.Success, "已通知升级程序,请稍后...");
                }
                else
                {
                    return new CommandResult(ResultStatus.Error, "通知升级程序失败");
                }
            }
            else
            {
                return new CommandResult(ResultStatus.InValid, "找不升级程序(Tools\\run\\upgrade.exe)，升级失败");
            }
        }

        private RunContext BuildRunContext(AppVersion version)
        {
            RunContext runContext = new RunContext();
            runContext.VersionRAR = version.VersionRAR;
            runContext.MainHost = version.MainHost;
            runContext.Client = version.Client;
            runContext.StopApiApp = version.StopApiApp;
            runContext.StopAutoTask = version.StopAutoTask;
            runContext.StopWebApp = version.StopWebApp;
            runContext.Version = version.Version;
            runContext.UserId = version.UserId;
            runContext.DownloadURL = version.VersionDownloadURL;

            this.SqlConnectionRender(runContext);

            return runContext;
        }

        private void SqlConnectionRender(RunContext context)
        {
            //获取当前api的数据库连接字符串
            string dbname = Utils.ConfigurationUtil.GetAppsetting("DBFrom", "PMSDB");
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[dbname].ConnectionString;

            System.Data.SqlClient.SqlConnectionStringBuilder connectBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);

            context.UID = connectBuilder.UserID;
            context.Password = connectBuilder.Password;
            context.DatabaseName = connectBuilder.InitialCatalog;
            context.DataSource = connectBuilder.DataSource;
        }
    }
}
