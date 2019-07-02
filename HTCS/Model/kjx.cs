using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   
    public  class T_kjx:BasicModel
    {
        public long CompanyId { get; set; }
        public string lockId { get; set; }
        public long id { get; set; }
        public long UserId { get; set; }
        public string refresh_token { get; set; }
        public int openid { get; set; }
        public long expires_in { get; set; }
        public DateTime r_expires_in { get; set; }
        public string scope { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string redirect_uri { get; set; }
        [NotMapped]
        public string errcode { get; set; }
        [NotMapped]
        public string errmsg { get; set; }
        
        public string description { get; set; }
    }
    public class local : BasicModel
    {
        public List<locklist> list { get; set; }
        public string  UserName { get; set; }
        public string lockData { get; set; }
        public string gatewayId { get; set; }
        public string lockAlias { get; set; }
        public long UserId { get; set; }
        public long HouseId { get; set; }
        public int pages { get; set; }
        public int total { get; set; }

        public int SearchType { get; set; }
    }
    public class localkey : BasicModel
    {
        public List<keylist> list { get; set; }
        public long UserId { get; set; }
        public int pages { get; set; }
        public int total { get; set; }
        public string errcode { get; set; }
        public string description { get; set; }
        public string errmsg { get; set; }
    }
    public  class locklist
    {
        public int id { get; set; }
        public string lockId { get; set; }

        public long UserId { get; set; }
        public long date { get; set; }
       
        public string lockName { get; set; }
        public string lockAlias { get; set; }
        public string lockMac { get; set; }
        public int rssi { get; set; }
        public long updateDate { get; set; }
        
        public int electricQuantity { get; set; }
        public int keyboardPwdVersion { get; set; }
        public int specialValue { get; set; }
        public string pwdInfo { get; set; }
        public string timestamp { get; set; }
        public string HouseName { get; set; }

        public string password { get; set; }
    }
    public class paralock
    {
        public long UserId { get; set; }
        
        public string lockId { get; set; }

        public string lockData { get; set; }
        
        public int HouseType { get; set; }
        public string lockAlias { get; set; }

        public long HouseId { get; set; }

    }
    public partial class Wraplocklist
    {
        public int id { get; set; }
        public string lockId { get; set; }
        public long date { get; set; }

        public string lockName { get; set; }
        public string lockAlias { get; set; }
        public string lockMac { get; set; }
        public int electricQuantity { get; set; }
        public int keyboardPwdVersion { get; set; }
        public int specialValue { get; set; }

        public string HouseName { get; set; }

        public long HouseId { get; set; }

        public int HouseType { get; set; }
    }

    public class keylock
    {
        public List<kl> keyList { get; set; }
        public long openid { get; set; }
        public long lastUpdateDate { get; set; }
    }

    public class gateway
    {
        public string gatewayId { get; set; }
        //网关mac地址
        public string gatewayMac { get; set; }
        //网关版本号：1-G1，2-G2
        public string gatewayVersion { get; set; }
        //网关连接的网络名称
        public string networkName { get; set; }
        //数量
        public string lockNum { get; set; }
        //是否在线：0-否，1-是
        public string isOnline { get; set; }


        public string modelNum { get; set; }
        
        public string hardwareRevision { get; set; }


        public string firmwareRevision { get; set; }
       
        public string gatewayNetMac { get; set; }
    }
   
    public class syspara:BasicModel
    {
        public string UserName { get; set; }
        public long UserId { get; set; }

        public long CompanyId { get; set; }
        public long lastUpdateDate { get; set; }
        public string ptusername { get; set; }
    }
    public class kl
    {
        public int keyId { get; set; }
        public int lockId { get; set; }
        public String userType { get; set; }
        public String keyStatus { get; set; }
        public String lockName { get; set; }
        public String lockAlias { get; set; }
        public String lockKey { get; set; }
        public String lockMac { get; set; }
        public int lockFlagPos { get; set; }
        public String adminPwd { get; set; }
        public String noKeyPwd { get; set; }
        public String deletePwd { get; set; }
        public int electricQuantity { get; set; }
        public String aesKeyStr { get; set; }
        public long startDate { get; set; }
        public long endDate { get; set; }
        public long timezoneRawOffset { get; set; }
        public String remarks { get; set; }
        public lockVersion lockVersion { get; set; }
        public string adminPs { get; set; }
    }
    public class lockVersion
    {
        public int scene { get; set; }
        public int groupId { get; set; }
        public int orgId { get; set; }
        public int protocolType { get; set; }
        public int protocolVersion { get; set; }
    }
    //钥匙
    public partial class keylist:BasicModel
    {
        public string content { get; set; }
        public string title { get; set; }
        public string senduser_content { get; set; }
        public string sendphone { get; set; }
        public long UserId { get; set; }
    }
    public partial class keylist
    {
        public int id { get; set; }
        public Nullable<int> keyId { get; set; }
        public string lockId { get; set; }
        public Nullable<int> openid { get; set; }
        public string username { get; set; }
        public string keyStatus { get; set; }
        public Nullable<long> startDate { get; set; }
        public Nullable<long> endDate { get; set; }
        public Nullable<long> date { get; set; }
        public string remarks { get; set; }
        public Nullable<int> is_administrators { get; set; }
        public Nullable<System.DateTime> updatetime { get; set; }
    }
    public class reclass
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string description { get; set; }
        public int expires_in { get; set; }
        public string keyId { get; set; }
    }
        public class UserKey:BasicModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string UserName { get; set; }

        public string  KeyId { get; set; }

    }
    public class shouquan
    {
        public string UserName { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }


       
    }
    public class jplist : BasicModel
    {
        public List<jpkeylist> list { get; set; }
    }
    //键盘密码
    public  class jpkeylist: BasicModel
    {
        public string deleteType { get; set; }
        public string errcode { get; set; }
        public string description { get; set; }
        public string errmsg { get; set; }
        public int isExclusive { get; set; }
        public int changeType { get; set; }
        public int addType { get; set; }
        public string newKeyboardPwd { get; set; }
        public long startDate { get; set; }
        public long endDate { get; set; }
        public long sendDate { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string lockId { get; set; }
        public string keyboardPwdId { get; set; }
        public string keyboardPwd { get; set; }
        public string keyboardPwdType { get; set; }
        public string ShareContent { get; set; }


    }
    public class parajpkey
    {
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public string UserName { get; set; }
        public int id { get; set; }
        public Nullable<int> keyboardPwdId { get; set; }
        public string lockId { get; set; }
        public string keyboardPwd { get; set; }
        public Nullable<int> keyboardPwdVersion { get; set; }
        public Nullable<int> keyboardPwdType { get; set; }
        public long startDate { get; set; }
        public long endDate { get; set; }
        public long sendDate { get; set; }
        public int isExclusive { get; set; }
    }
    public class paraversion
    {
        public int keyboardPwdVersion { get; set; }
    }
    public class MyRemark
    {
        public string username { get; set; }

        public string FN_lockname { get; set; }
    }
    public class kjxbase
    {
        public string errcode { get; set; }
        public string description { get; set; }
        public string errmsg { get; set; }
    }
    
}
