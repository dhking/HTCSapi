using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Sandbox
{
    public class SandboxRunListener
    {
        private static SandboxRunListener listener;

        private System.Timers.Timer timer;

        private bool timerIsRunning = false;

        private bool keepRunning = false;

        public readonly string file;

        private SandboxRunListener()
        {
            file = Utils.IOUtil.CombineDIR(Environment.ResourceEnvironment.Environment.ClientSandboxNotifyDIR, SandboxRunNotifyBag.notifyfile);

            string timeout = Utils.ConfigurationUtil.GetAppsetting("RunTimeout", "5000");
            int seconds = 5000;
            int.TryParse(timeout, out seconds);
            timer = new System.Timers.Timer(seconds);
            timer.Elapsed += CheckRunNotify;
            timer.Start();
        }

        public static SandboxRunListener Instance
        {
            get
            {
                if (listener == null)
                {
                    listener = new SandboxRunListener();
                }

                return listener;
            }
        }

        /// <summary>
        /// 启动升级监听服务
        /// </summary>
        public void Run()
        {
            //启用文件监听器
            this.keepRunning = true;
            this.timerIsRunning = true;
        }

        public void Stop()
        {
            this.keepRunning = false;
        }

        void CheckRunNotify(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.keepRunning)
            {
                NotifyResponse();
            }
        }

        void NotifyResponse()
        {
            try
            {
                this.StopTimer();
                if (Utils.IOUtil.ExistsFile(file))
                {
                    SandboxRunNotifyBag bag = SandboxRunNotifyBag.Read();
                    if (bag.Validate())
                    {
                        SandboxNotifyRun.RunNotify(bag);
                    }
                }
                this.timer.Start();
            }
            catch (Exception ex)
            {
                SandboxRunLog.Error(ex.ToString());

            }
            finally
            {
                this.StartTimer();
                Utils.IOUtil.Delete(file);
            }
        }

        void StopTimer()
        {
            if (this.timerIsRunning)
            {
                this.timer.Stop();
                this.timerIsRunning = false;
            }
        }

        void StartTimer()
        {
            if (!this.timerIsRunning)
            {
                this.timer.Start();
                this.timerIsRunning = true;
            }
        }
    }
}
