using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public  class ElecUser:BasicModel
    {
    
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string username { get; set; }

        public string pass { get; set; }

        public int paratype { get; set; }

      

        public int Type { get; set; }

        [NotMapped]
        public string Uuid { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        [NotMapped]
        public string Expand { get; set; }
        [NotMapped]
        public DateTime now { get; set; }
    }
    public class Expanduser2
    {
        public int cash { get; set; }


        public DateTime settletime { get; set; }
    }

    public class Elec<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public Expand Expand { get; set; }

        public T Data { get; set; }
    }

    public class Elec1<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Expand { get; set; }

        public T Data { get; set; }
    }

    public class Elec2<T>
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public T Expand { get; set; }

       
    }
    public class Expand
    {
        public string Uuid { get; set; }
    }
    public class Elec
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Expand { get; set; }
    }
    public class cell
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string Expand { get; set; }
    }

    public class ruzhu
    {
        public string name { get; set; }

        public string phone { get; set; }

        public DateTime checkingdate { get; set; }

        public DateTime checkoutdate { get; set; }

        public int staymode { get; set; }
        public string devid { get; set; }

        public string CompanyId { get; set; }
        [NotMapped]
        public string Uuid { get; set; }
        [NotMapped]
        public string Expand { get; set; }
    }
    [NotMapped]
    public class other: ElecUser
    {
        public string devid { get; set; }

        public string  hid { get; set; }
        
        public decimal Price { get; set; }

        public string nid { get; set; }

        public string cid { get; set; }

        public string macid { get; set; }

        public string userid { get; set; }

        public string title { get; set; }

        public string houseid { get; set; }

        public string AreaCode { get; set; }

        public string CellName { get; set; }

        public decimal value { get; set; }
    }
    //电量统计
    public class ElecStatic
    {
        //初始电量
        public decimal Initpower { get; set; }
        //结束电量
        public decimal Lastpower { get; set; }
        //分摊电量
        public decimal Apportion { get; set; }
        //总电量
        public decimal Allpower { get; set; }
        //金额
        public decimal Money { get; set; }
        //日期
        public DateTime Date { get; set; }
        public string _id { get; set; }
        public string addr { get; set; }
        public string allpower { get; set; }
        public string apportion { get; set; }
        public string ccode { get; set; }
        public string initpower { get; set; }
        public string lastpower { get; set; }
        public string pcode { get; set; }
        public string title { get; set; }
        public DateTime bdate { get; set; }

        public DateTime edate { get; set; }

        public DateTime Month { get; set; }

        public string devid { get; set; }
        public long HouseId { get; set; }
    }
    public class zkelec
    {
        public decimal Price { get; set; }

        public decimal surplus { get; set; }
    }
    public class localcity
    {
        public int subtype { get; set; }
        public string pcode { get; set; }
    }
    //设备
    public class DeviceData
    {
        public string Addr { get; set; }
        public string devid { get; set; }
        public string search { get; set; }
        public int Value { get; set; }
        public int type { get; set; }
        public string Pcode { get; set; }
        public long HouseId { get; set; }
       
        public string Ccode { get; set; }
        public string HouseUuid { get; set; }
        public string Cid { get; set; }
        public string Nid { get; set; }

        public string Pid { get; set; }
        public int Devno { get; set; }
        public bool Isnode { get; set; }

        public int ElecType { get; set; }
        public decimal Price { get; set; }
        public decimal Money { get; set; }
        public DateTime Lasttime { get; set; }
        public int HouseType { get; set; }
        public int Status { get; set; }

        public string Uuid { get; set; }
        public DeviceExpand Expand { get; set; }
        //入住人信息
        public External External { get; set; }
        public Param Param { get; set; }
        //房间名称
        public string HouseName { get; set; }

       

        //房间标题

        public string Title { get; set; }
        public int iscuizu { get; set; }
        public List<fentan> fentan { get; set; }
    }
    public class External
    {
        public string alarm { get; set; }
        public string alarmvalue { get; set; }
        public List<apportion> apportion { get; set; }
        public string checkindate { get; set; }
        public string checkoutdate { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string stay { get; set; }
        public string staycostday { get; set; }
        public string staydate { get; set; }
        public string staymode { get; set; }
    }
    public class apportion
    {
        public string allpower { get; set; }
        public string money { get; set; }
        public string percent { get; set; }
        public string devid { get; set; }

        public string Uid{ get; set; }
        public string HouseName { get; set; }
    }
    public class fenzu
    {
        public string district { get; set; }

        public string code { get; set; }

        public string Title { get; set; }

        public int Quantity { get; set; }

        public int Online { get; set; }

        public List<Citys> Citys { get; set; }
    }
    public class Dataq
    {
        public Citys Data { get; set; }
    }
    public class  Citys
    {
        public string Addr { get; set; }

        public int Quantity { get; set; }

        public int Online { get; set; }

        public string code { get; set; }

        public string Uuid { get; set; }

        [JsonProperty("Citys")]
        public List<Dataq> Citys1 { get; set; }
    }
    public class fentan
    {
        public string Devid { get; set; }

        public decimal Percent { get; set; }
    }
    //房源
    public class elechouse
    {
        public string code { get; set; }
        public string districtcode { get; set; }
        public string Uuid { get; set; }
        public string  title { get; set; }

        public int level { get; set; }
    }
    //创建小区
    public class eleccell
    {
        public string code { get; set; }

        public string title { get; set; }

        public string content { get; set; }
    }
    public class Param
    {
        public int mode { get; set; }
    }
    public class DeviceExpand
    {
        public decimal allpower { get; set; }

        public decimal surplus { get; set; }

        public decimal voltage { get; set; }

        public time2 lasttime { get; set; }

        public string Uuid { get; set; }


    }
    public class time2
    {
        public DateTime time { get; set; }
    }
}
