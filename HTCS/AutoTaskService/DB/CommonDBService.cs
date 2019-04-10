using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AutoTaskService.DB
{
    public class CommonDBService
    {
        public static HttpClient client;
        public bool InsertAutotaskHistory(int SysAutoTaskId, byte execStatus, string execMessage, int totalSeconds, string jobPara1, string jobPara2, string OwnerId, string InstanceId, out string msg)
        {
            msg = "";
            return true;
        }
    }
}
