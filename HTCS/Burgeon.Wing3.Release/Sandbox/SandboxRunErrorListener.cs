using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Burgeon.Wing3.Release.Sandbox
{
    public class SandboxRunErrorListener
    {
        private static SandboxRunErrorListener listener;

        private System.Timers.Timer timer;

        private bool timerIsRunning = false;

        public readonly string file;

        private RunContext runContext;

        private SandboxRunErrorListener()
        {
            file = SandboxRunLog.ErrorLogPath;
            Utils.IOUtil.Delete(file);

            string timeout = Utils.ConfigurationUtil.GetAppsetting("RunTimeout", "1000");
            int seconds = 1000;
            int.TryParse(timeout, out seconds);
            timer = new System.Timers.Timer(seconds);
            timer.Elapsed += SeeSandBoxRunIsError;
        }

        public static SandboxRunErrorListener Instance
        {
            get
            {
                if (listener == null)
                {
                    listener = new SandboxRunErrorListener();
                }
                return listener;
            }
        }

        public void Start(RunContext context)
        {
            runContext = context;
            this.StartTimer();
        }

        void SeeSandBoxRunIsError(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.StopTimer();
                if (Utils.IOUtil.ExistsFile(file))
                { 
                    runContext.SendVersionLog(System.IO.File.ReadAllText(file), runContext.UserId);
                }
                else
                {
                    this.StartTimer();
                }
            }
            catch (Exception ex)
            {
                SandboxRunLog.ListenerErrorLog(ex.ToString());
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
