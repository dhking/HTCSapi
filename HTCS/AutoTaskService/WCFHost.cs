using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace AutoTaskService
{
    internal class WCFHost
    {
        List<BurgeonServiceHost> _service = null;

        private static ILog logger = null;

        internal WCFHost()
        {


            logger = LogManager.GetLogger(typeof(WCFHost));
            logger.Warn("Setting up services...");


        }


        public void Start()
        {
            logger.Warn("Starting services...");
            _service = new List<BurgeonServiceHost>();
            GetAllServiceFromConfig();
            //_service.Open();
            if (_service != null && _service.Count > 0)
            {

                foreach (ServiceHost host in _service)
                {
                    if (host != null && host.State != CommunicationState.Opened)
                    {
                        host.Open();
                    }

                }

            }
            logger.Warn("Started!");
        }

        public void Stop()
        {
            logger.Warn("Stopping services...");
            try
            {
                if (_service != null && _service.Count > 0)
                {

                    foreach (BurgeonServiceHost host in _service)
                    {
                        if (host != null && host.State == CommunicationState.Opened)
                        {
                            host.Close();
                        }

                    }

                }


                logger.Warn("Stopped!");
            }
            catch (Exception ex)
            {
                logger.Warn("Could not stop: " + ex.Message);
            }
        }


        /// <summary>
        /// 获取所有
        /// </summary>
        private void GetAllServiceFromConfig()
        {
            //配置文件目录
            string configdir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //具体配置文件地址
            string wcfConfigPath = System.IO.Path.Combine(configdir, @"config\wcf.config");

            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = wcfConfigPath;
            logger.Info("开始读取服务配置文件......");
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            if (config != null)
            {
                ServiceModelSectionGroup svcmod = (ServiceModelSectionGroup)config.GetSectionGroup("system.serviceModel");
                Assembly asmb = Assembly.LoadFrom(configdir + "Burgeon.Wing3.Service.dll");
                Assembly asmb2 = Assembly.LoadFrom(configdir + "Burgeon.Wing3.DRPService.dll");
                foreach (ServiceElement el in svcmod.Services.Services)
                {
                    logger.Info("开始加载服务:" + el.Name + ".....");
                    BurgeonServiceHost host = null;
                    //string className = el.Name.Substring(el.Name.LastIndexOf('.') + 1);

                    //Assembly asmb = Assembly.LoadFrom(configdir.Substring(0, config.FilePath.LastIndexOf('\\') + 1) + className + ".dll");


                    //Burgeon.Wing3.Service
                    Type svcType = asmb.GetType(el.Name);

                    if (svcType == null)
                    {
                        logger.Info("开始转到加载DRP服务:" + el.Name + ".....");

                        svcType = asmb2.GetType(el.Name);

                    }
                    if (svcType == null)
                    {

                        logger.Info("服务:" + el.Name + "没有找到。。。");
                        continue;
                    }
                    host = new BurgeonServiceHost(svcType);

                    _service.Add(host);
                    logger.Info("服务:" + el.Name + "加载结束");

                }
            }
        }
    }



    /// <summary>
    /// 重写ServiceHost
    /// 目的是让配置文件可以不用放在app.config 里面
    /// </summary>
    public class BurgeonServiceHost : ServiceHost
    {

        public BurgeonServiceHost()
            : base()
        {

        }


        public BurgeonServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        /// <summary>
        /// override ApplyConfiguration to load config from custom file
        /// </summary>
        protected override void ApplyConfiguration()
        {
            //get custom config file name by our rule: config file name = ServiceType.Name
            var myConfigFileName = @"config\wcf.config";
            //get config file path
            string dir = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string myConfigFilePath = System.IO.Path.Combine(dir, myConfigFileName);
            if (!System.IO.File.Exists(myConfigFilePath))
            {
                base.ApplyConfiguration();
                return;
            }
            var configFileMap = new System.Configuration.ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = myConfigFilePath;
            var config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            var serviceModel = System.ServiceModel.Configuration.ServiceModelSectionGroup.GetSectionGroup(config);
            if (serviceModel == null)
            {
                base.ApplyConfiguration();
                return;
            }
            foreach (ServiceElement serviceElement in serviceModel.Services.Services)
            {
                if (serviceElement.Name == this.Description.ServiceType.FullName)
                {
                    LoadConfigurationSection(serviceElement);
                    return;
                }
            }
            throw new Exception("there is no service element match the description!");
        }


    }
}
