using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerHelper
{
    public class AccountHelper
    {
        public static Guid GetUUID(string token)
        {
            return new Guid(token);
        }

        public static UserType GetUserType(string token)
        {
            return 0;
        }
    }
    public enum UserType
    {
        A,
        B,
    }
}
