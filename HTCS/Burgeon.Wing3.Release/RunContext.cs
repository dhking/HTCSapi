using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Burgeon.Wing3.Release.Environment;

namespace Burgeon.Wing3.Release
{
    public class RunContext
    {
        private int logCount = 0;

        StringBuilder log = null;

        StringBuilder localBuilder = null;

        public RunContext()
        {
            log = new StringBuilder();
            localBuilder = new StringBuilder();
        }

        /// <summary>
        /// 获取当期升级所有日志信息
        /// </summary>
        public StringBuilder AllLogs
        {
            get
            {
                return localBuilder;
            }
        }

        /// <summary>
        /// 获取或者设置当前启动升级用户的Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 获取或者设置版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 获取或者设置版本下载地址
        /// </summary>
        public string DownloadURL { get; set; }

        /// <summary>
        /// 获取或者设置主服务器地址
        /// </summary>
        public string MainHost { get; set; }

        /// <summary>
        /// 当前升级包压缩包路径
        /// </summary>
        public string VersionRAR { get; set; }

        /// <summary>
        /// 获取临时版本库主目录
        /// </summary>
        public string SandboxDIR { get; set; }

        /// <summary>
        /// 设置 停止网站或者自动任务尝试此处
        /// </summary>
        public int TryNum
        {
            get
            {
                int num = 20;
                int.TryParse(Utils.ConfigurationUtil.GetAppsetting("TRYNUM", "20"), out num);
                return num;
            }
        }

        public Client Client { get; set; }

        /// <summary>
        /// 获取或者设置当前升级数据库登陆用户名
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// 获取或者设置当前升级数据库登陆密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或者设置当前升级数据库登陆数据库
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 获取或者设置当前升级数据服务
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 获取或者设置当前升级是否停止Web网站
        /// </summary>
        public bool StopWebApp { get; set; }

        /// <summary>
        /// 获取或者设置当前升级是否停止API网站
        /// </summary>
        public bool StopApiApp { get; set; }

        /// <summary>
        /// 获取或者设置当前升级是否停止自动任务
        /// </summary>
        public bool StopAutoTask { get; set; }

        /// <summary>
        /// 获取临时版本库 不完整版本 发布目录
        /// </summary>
        public string SandboxPartVersionDIR
        {
            get
            {
                return Environment.ResourceEnvironment.Environment.ClientSandboxPartVersionDIR;
            }
        }

        /// <summary>
        /// 获取当前运行网站根目录
        /// </summary>
        public string RunWebAppDomainDIR
        {
            get
            {
                return Utils.IOUtil.CombineDIR(Client.BaseDIR, Utils.ConfigurationUtil.GetAppsetting("WEBAPP", "WEB"));
            }
        }

        /// <summary>
        /// 获取当前运行API根目录
        /// </summary>
        public string RunAPIAppDomainDIR
        {
            get
            {
                return Utils.IOUtil.CombineDIR(Client.BaseDIR, Utils.ConfigurationUtil.GetAppsetting("APIAPP", "API"));
            }
        }

        /// <summary>
        /// 获取或者设置当前运行AutoTask根目录
        /// </summary>
        public string RunAutoTaskAppDomainDIR
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或者设置当前运行自动任务 标题
        /// </summary>
        public string AutotaskTitle
        {
            get;
            set;
        }

        /// <summary>
        /// 根据当前更新版本号 获取该版本对应的api应用程序补丁包下载后保存路径
        /// </summary>
        /// <returns></returns>
        public string SqlResourceRARPath
        {
            get
            {
                string path = System.IO.Path.Combine(SandboxPartVersionDIR, Version, "DB.rar");
                if (!Utils.IOUtil.ExistsFile(path))
                {
                    path = System.IO.Path.Combine(SandboxPartVersionDIR, Version, "RdsDB.rar");
                }
                return path;
            }
        }

        /// <summary>
        /// 获取当前数据库压缩文件 解压目录
        /// </summary>
        /// <returns></returns>
        public string SqlResourceUnRARPath
        {
            get
            {
                string path = System.IO.Path.Combine(SandboxPartVersionDIR, Version, "DB");
                if (!Utils.IOUtil.ExistsFile(path))
                {
                    path = System.IO.Path.Combine(SandboxPartVersionDIR, Version, "RdsDB");
                }
                return path;
            }
        }

        public string AppDataDIR
        {
            get
            {
                return Utils.IOUtil.AutoCreateDIR(Utils.IOUtil.CombineDIR(AppDomain.CurrentDomain.BaseDirectory, "App_Data"));
            }
        }

        public string InfoMessage { get; set; }

        public Exception Exception { get; set; }

        public bool IsError
        {
            get
            {
                return this.Exception != null;
            }
        }

        public void Log(string message)
        {
            logCount++;
            message = message.Replace("\'", "\\\'").Replace("\n", "").Replace("\t", "").Replace("\r", "");
            if (!string.IsNullOrWhiteSpace(message))
            {
                this.localBuilder.AppendLine("<li>" + message + "</li>");
                log.AppendLine(message);
                if (logCount > 10)
                {
                    string content = this.GetAllLogs();
                    this.SendVersionLog(content, this.UserId);
                    log.Clear();
                    logCount = 0;
                }
                Console.WriteLine(message);
            }
        }

        public void StartLocal()
        {
            localBuilder = new StringBuilder();
            localBuilder.AppendLine(string.Format("<html><head><title>{0}升级日志</title><meta charset='utf-8' /></head>", this.Version));
            localBuilder.AppendLine(string.Format("<body style='font-size: 12px;background-color: rgba(204, 204, 204, 0.2);'>", this.Version));
            localBuilder.AppendLine(string.Format("<h1>升级日志{0}</h1>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            localBuilder.AppendLine("<ol>");
        }

        public void EndLocal()
        {
            this.localBuilder.AppendLine("</ol></body></html>");
            string dir = AppDataDIR;
            string file = Utils.IOUtil.CombineDIR(dir, "upgrade.html");
            System.IO.File.WriteAllText(file, this.localBuilder.ToString());
            System.IO.File.Copy(file, Utils.IOUtil.CombineDIR(RunAPIAppDomainDIR, "upgrade.html"), true);
        }

        public static RunContext RunParse(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<RunContext>(json);
            }
            catch
            {
                return null;
            }
        }

        public string GetAllLogs()
        {
            return this.log.ToString();
        }

        /// <summary>
        /// 发送日志到主服务器
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public CommandResult SendVersionLog(string message, string userid)
        {
            Environment.Commands.VersionSendVersionLogCommand command = new Environment.Commands.VersionSendVersionLogCommand();
            Environment.CommandContext context = new CommandContext();
            context["cpy"] = this.Client.Id;
            context["main"] = this.MainHost;
            context["message"] = message;
            context["m"] = "multi";
            context["userid"] = userid;
            context["version"] = this.Version;
            return command.Execute(context);
        }

        /// <summary>
        /// 设置当前升级结束
        /// </summary>
        /// <param name="message"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        public CommandResult VersionUpgradeFinal(bool isError, string err, string userid)
        {
            Environment.Commands.VersionSendPublishFinalCommand command = new Environment.Commands.VersionSendPublishFinalCommand();
            Environment.CommandContext context = new CommandContext();
            context["company"] = this.Client.Id;
            context["host"] = this.MainHost;
            context["message"] = isError ? "发布失败" : "发布成功";
            context["detail"] = err;
            context["userid"] = userid;
            return command.Execute(context);
        }
    }
}
