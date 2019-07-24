//using ControllerHelper;
//using DAL;
//using DAL.Common;
//using Model;
//using Model.Base;
//using Model.Contrct;
//using Model.House;
//using Model.User;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Service
//{
//    public   class initgwService
//    {
//        queryupperDAl dal = new queryupperDAl();
//        HouseDAL housedal = new HouseDAL();
//        HousePentDAL pentdal = new HousePentDAL();
//        //查询未上架数据
//        public SysResult query(int gwisupper)
//        {
//            SysResult result = new SysResult();

//            List<houresourcesupper> list = dal.Query(new houresourcesupper() { gwisupper = gwisupper });
//            foreach (var mo in list)
//            {
//                HouseModel housemodel = new HouseModel();
//                HousePendent pent = new HousePendent();
//                //组合入参
//                HouseZK zk = new HouseZK();
//                //判断房源类型
//                if (mo.housetype == 1)
//                {
//                    housemodel = housedal.Queryhouse(new HouseModel() { Id = mo.houseid },null,null);
//                    if (housemodel == null)
//                    {
//                        continue;
//                    }
//                    zk.Image = housemodel.PublicImg;
//                    zk.Title = housemodel.CellName + "整租";
//                    zk.Price = housemodel.Price;
//                    zk.Type = 1;
//                    zk.RecentTime = housemodel.Renttime;//处理
//                    zk.Measure = housemodel.Measure;
//                    if (housemodel.PublicTeshe != null)
//                    {
//                        List<string> listts = housemodel.PublicTeshe.Split(","[0]).ToList();
//                        if (listts != null)
//                        {
//                            zk.pts = PuclicDataHelp.getstr(listts);
//                        }
//                    }
//                    if ( housemodel.PublicPeibei != null)
//                    {
//                        List<string> list1 = housemodel.PublicPeibei.Split(","[0]).ToList();
//                        List<PeibeiZK> listzk = new List<PeibeiZK>();
//                        foreach (var mo1 in list1)
//                        {
//                            listzk.Add(new PeibeiZK() { Name = mo1 });
//                        }
//                        zk.Peipei = listzk;
//                    }
//                }
//                if (mo.housetype == 2 || mo.housetype == 3)
//                {
//                    pent = pentdal.Querybyid(new HousePendent() { ID = mo.houseid });
                    
//                    if (pent == null)
//                    {
//                        continue;
//                    }
//                    if (pent.ID == 11275)
//                    {

//                    }
//                    housemodel = housedal.Queryhouse(new HouseModel() { Id = pent.ParentRoomid },null,null);
//                    if (housemodel == null)
//                    {
//                        continue;
//                    }
//                    zk.Image = housemodel.PublicImg + pent.PrivateImage;
//                    zk.Title = housemodel.CellName + "合租";
//                    zk.Price = pent.Price;
//                    zk.RecentTime = pent.RecentTime;//处理
//                    zk.Measure = pent.Measure;
//                    if (pent.PrivateTeshe != null)
//                    {
//                        List<string> listts= pent.PrivateTeshe.Split(","[0]).ToList();
//                        if (listts != null)
//                        {
//                            zk.pts = PuclicDataHelp.getstr(listts);
//                        }
//                    }
//                    if (pent.PrivatePeibei != null|| housemodel.PublicPeibei!=null)
//                    {
//                        List<string> list1 = (pent.PrivatePeibei + housemodel.PublicPeibei).Split(","[0]).ToList();
//                        List<PeibeiZK> listzk = new List<PeibeiZK>();
//                        foreach (var mo1 in list1)
//                        {
//                            listzk.Add(new PeibeiZK() { Name = mo1 });
//                        }
//                        zk.Peipei = listzk;
//                    }
//                    if (mo.housetype == 3)
//                    {
//                        zk.Type = 1;
//                    }
//                    else
//                    {
//                        zk.Type = 2;
//                    }
//                }
//                zk.CompanyId = housemodel.CompanyId;
//                zk.TingWei = housemodel.ShiNumber.ToStr() + "室" + housemodel.TingNumber.ToStr() + "厅" + housemodel.WeiNumber.ToStr() + "卫";
//                zk.Cx = pent.Orientation;
//                zk.Adress = housemodel.Adress;
//                zk.PushTime = housemodel.CreateTime;
//                zk.CreateTime = DateTime.Now;
//                zk.area = housemodel.AreamName;
//                zk.businessarea = housemodel.BusinessArea;
//                zk.city = housemodel.CityName;
//                zk.Fukuan = "押一付一";//处理
//                zk.Adress = housemodel.Adress;
//                zk.Shi = housemodel.ShiNumber;
//                zk.Phone = housemodel.housekeeperphone;//处理
//                zk.LatiTude = housemodel.latitude;
//                zk.LongiTude = housemodel.longitude;
//                zk.Floor = housemodel.AllFloor;//处理
//                zk.CellName = housemodel.CellName;
//                if (mo.housetype == 2|| mo.housetype == 1)
//                {
//                    zk.FloorIndex = housemodel.NowFloor;
//                }
//                if (mo.housetype == 3)
//                {
//                    T_Floor floor = pentdal.queryfloorxq(pent.FloorId);
//                    if (floor != null)
//                    {
//                        zk.FloorIndex = floor.Floor;
//                    }
//                }
//                zk.metro = "";//处理
//                if (string.IsNullOrEmpty(zk.Image))
//                {
//                    zk.Image = "zkmoren.jpg;";
//                }
//                result=inerthouse(zk);
//                if (result.Code == 0)
//                {
//                    mo.gwisupper = 2;
//                    dal.Save(mo);
//                }
//            }
//            return result;
//        }
        
//        public SysResult inerthouse(HouseZK zk)
//        {
//            SysResult result = new SysResult();
//            //执行插入操作
//            HtcsZKClient htcs = new HtcsZKClient("api/House/Save");
//            result= htcs.DoExecute2<HouseZK>(zk);
//            return  result;
//        }
       
//    }
//}
