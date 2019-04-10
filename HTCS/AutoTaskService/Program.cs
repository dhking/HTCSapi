using AutoTaskService.DB;
using DBHelp;
using log4net;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace AutoTaskService
{
    public static class Program
    {
        public static string DbType;
        public static Dictionary<string, int> shopIDkey = new Dictionary<string, int>();
        public static bool startIncrementServer = false;
        //public static bool startImcmessageServer = false;//消息服务
        public static IList<SysParaModel> paraList = new List<SysParaModel>();
      
        public static ILog logger = null;

        public static Int32 authStatus = 0;//0表示未授权，1表示授权文件过期，2表示产品购买到期，99表示服务到期时间，100表示成功

        public static string AuthStatusMesg = "";//授权状态信息

        public static string imcGroupName = "";//消息服务的组名,如果为空则

        public static string NotStartTaskId = "";

        public static string appkey = "";
        public static string appsecret = "";
        public static bool IsIMCStart = false;//淘宝消息服务是否启动
        /// <summary>
        /// Main.
        /// </summary>
        public static void Main(String[] args)
        {




            log4net.Config.XmlConfigurator.Configure();
            try
            {
                logger = LogManager.GetLogger(typeof(Program));

                string group = System.Configuration.ConfigurationManager.AppSettings["JobGroup"].ToStr().Trim();
                // licenseInfo = LicenseHelper.GetLinceseInfo(out AuthStatusMesg, out authStatus);
                // if (group == "Bos组" || group == "Emax组" || group == "Bos组,WCF组" || group == "Emax组,WCF组" || group == "WCF组")
                authStatus = 100;
                if (authStatus == 0)
                    throw new WingBussinessException("授权失败,失败原因:未授权!授权码:" + authStatus + "本机信息：" + AuthStatusMesg);
                else if (authStatus == 1)
                {
                    //授权文件过期
                    authStatus = 100;
                }
                else if (authStatus == 2)
                    throw new WingBussinessException("产品购买到期!");
                else if (authStatus == 99)
                    AuthStatusMesg = "成功";
                else if (authStatus == 100)
                {
                    AuthStatusMesg = "成功";
                }
                else
                    throw new WingBussinessException("授权错误,无法识别授权文件状态!");

                DbType = System.Configuration.ConfigurationManager.AppSettings["DbType"].ToStr().ToLower();
                //不启动任务编号 一定
                NotStartTaskId = System.Configuration.ConfigurationManager.AppSettings["NotStartTaskId"].ToStr().ToLower();


                if (DbType != "sqlserver" && DbType != "sqlite" && DbType != "oracle")
                {
                    logger.Warn("数据类型配置错误!当前配置为:" + DbType);
                    throw new Exception("app.config内的dbtype属性请设置为sqlserver或oracle,并且设置正确的数据库连接符!");
                }
                else
                {
                    logger.Warn("当前启动数据库类型为:" + DbType);
                }




                WindsorRegistrar.RegisterAllFromAssembliesForWinform("DAL");
                logger.Warn("1:" + DbType);
                
                logger.Warn("2:" + DbType);
                WindsorRegistrar.RegisterAllFromAssembliesForWinform("DBHelp");
                logger.Warn("3:" + DbType);
               
                logger.Warn("4:" + DbType);
                WindsorRegistrar.RegisterAllFromAssembliesForWinform("AutoTaskService");
                logger.Warn("项目加载完成");
                string errmsg = "";
                IAutoTaskService autoTaskServic = IoC.Resolve<IAutoTaskService>();
                autoTaskServic.test();
                paraList = IoC.Resolve<ISysParaService>().GetAllToList(out errmsg, Program.DbType);
                if (paraList == null)
                {
                    logger.Warn(errmsg);
                    throw new Exception(errmsg);
                }
                logger.Warn("参数加载完成!共:" + paraList.Count);

                if (args.Length > 0 && args[0].ToUpper() == "-FORM")
                {
                    //MessageBox.Show(args[0].ToUpper());
                    

                }
                else if (args.Length > 0 && args[0].ToUpper() == "-FORM2")
                {
                   

                }
                else if (args.Length > 0 && args[0].ToUpper() == "-ALIPAY")
                {
                


                }
                else if (args.Length > 0 && args[0].ToUpper() == "-FORMTEST")
                {
                   

                }
                else
                {
                    HostFactory.Run(x =>
                    {
                        x.RunAsLocalSystem();



                        x.SetDescription(Configuration.ServiceDescription);
                        x.SetDisplayName(Configuration.ServiceDisplayName);
                        x.SetServiceName(Configuration.ServiceName);
                        x.StartAutomatically();//自动启动   

                        logger.Warn("自动启动完成");
                        x.Service(factory =>
                        {
                            QuartzServer server = new QuartzServer();
                           
                            server.Initialize();

                            return server;
                        });
                    });




                }
                logger.Warn("--启动结束--");
            }
            catch (Exception ce)
            {
                ////logger.Warn("启动出现异常!错误信息:" + ce.Message+ce.StackTrace);
                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("============================");
                //sb.AppendLine("启动出现异常!错误信息:" + ce.Message + AuthStatusMesg);
                ////sb.AppendLine(ce.StackTrace);
                //sb.AppendLine("============================");
                //logger.Warn(sb.ToStr());
                logger.Warn("启动自动任务异常");
                logException(ce, "启动自动任务异常");
            }
        }

        public static void logException(Exception e, string message = "")
        {

            StringBuilder preamble = new StringBuilder();
            preamble.Append(message);
            preamble.Append("\r\n 异常:");
            preamble.Append(e.Message);

            preamble.Append("\r\n");

            StackTrace st = new StackTrace(true);
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                MethodBase mb = sf.GetMethod();
                if (mb != null && mb.ReflectedType != null)
                {
                    string mname = mb.ReflectedType.FullName;
                    preamble.Append("at method:");
                    preamble.Append(mname);
                    preamble.Append("::");
                    preamble.Append(sf.GetMethod().Name);
                    preamble.Append(" @");
                    preamble.Append(sf.GetFileLineNumber());
                    preamble.Append(" +file:");
                    preamble.Append(sf.GetFileName());
                    preamble.Append("\r\n");
                }
            }

            preamble.Append("\r\n");

            logger.Fatal(preamble, e);
        }

    }
}
