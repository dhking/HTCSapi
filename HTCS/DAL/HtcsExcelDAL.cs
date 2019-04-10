using Model;
using Model.Contrct;
using Model.House;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HtcsExcelDAL
    {

        public byte[] excel(List<HouseModel> listmodel)
        {
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();
           
            //添加一个sheet
            for (int index = 0; index < 1; index++)
            {
                ISheet sheet = book.CreateSheet($"工作簿{(index + 1)}");

                //行下标记录
                int rowIndex = 0;
                //创建首行
                IRow row0 = sheet.CreateRow(rowIndex++);
                //创建单元格
                ICell cell0 = row0.CreateCell(0);
                cell0.SetCellValue("房源编号");

                ICell cell1= row0.CreateCell(1);
                cell1.SetCellValue("小区");

                ICell cell2 = row0.CreateCell(2);
                cell2.SetCellValue("地址");

                ICell cell3 = row0.CreateCell(3);
                cell3.SetCellValue("房管员");

                ICell cell4 = row0.CreateCell(4);
                cell4.SetCellValue("户型");

                ICell cell5 = row0.CreateCell(5);
                cell5.SetCellValue("面积");

                ICell cell6 = row0.CreateCell(6);
                cell6.SetCellValue("朝向");

                ICell cell7 = row0.CreateCell(7);
                cell7.SetCellValue("空置时间");
                foreach (var mo in listmodel)
                {
                    var row = sheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue(mo.Id);
                    row.CreateCell(1).SetCellValue(mo.CellName);
                    row.CreateCell(2).SetCellValue(mo.CellName+mo.BuildingNumber+"栋"+mo.Unit+"单元"+mo.RoomId+"室");
                    row.CreateCell(3).SetCellValue(mo.Fangguanyuan);
                    row.CreateCell(4).SetCellValue(mo.ShiNumber+"室"+mo.TingNumber+"厅"+mo.WeiNumber+"卫");
                    row.CreateCell(5).SetCellValue(mo.Measure + "平方");
                    row.CreateCell(6).SetCellValue(mo.Orientation );
                    row.CreateCell(7).SetCellValue(mo.Idletime+"天");
                }
            }
            using (MemoryStream st = new MemoryStream())
            {
                book.Write(st);
                var buffer = st.GetBuffer();
                return buffer;
            }
        }


        public byte[] hexcel(List<HousePendent> listmodel)
        {
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();

            //添加一个sheet
            for (int index = 0; index < 1; index++)
            {
                ISheet sheet = book.CreateSheet($"工作簿{(index + 1)}");
                //行下标记录
                int rowIndex = 0;
                //创建首行
                IRow row0 = sheet.CreateRow(rowIndex++);
                //创建单元格
                ICell cell0 = row0.CreateCell(0);
                cell0.SetCellValue("房源编号");

                ICell cell1 = row0.CreateCell(1);
                cell1.SetCellValue("小区");

                ICell cell2 = row0.CreateCell(2);
                cell2.SetCellValue("地址");


                ICell cell4 = row0.CreateCell(4);
                cell4.SetCellValue("面积");
                ICell cell5 = row0.CreateCell(5);
                cell5.SetCellValue("面积");

                ICell cell6 = row0.CreateCell(6);
                cell6.SetCellValue("朝向");

                ICell cell7 = row0.CreateCell(7);
                cell7.SetCellValue("空置时间");

                foreach (var mo in listmodel)
                {
                    var row = sheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue(mo.ID);
                    row.CreateCell(1).SetCellValue(mo.Cellname);
                    row.CreateCell(2).SetCellValue(mo.Adress);
                    row.CreateCell(4).SetCellValue(mo.Price + "平方");
                    row.CreateCell(5).SetCellValue(mo.Measure + "平方");
                    row.CreateCell(6).SetCellValue(mo.Orientation);
                    row.CreateCell(7).SetCellValue(mo.Idletime + "天");
                }
            }
            using (MemoryStream st = new MemoryStream())
            {
                book.Write(st);
                var buffer = st.GetBuffer();
                return buffer;
            }
        }
        public byte[] contractexcel(List<WrapContract> listmodel)
        {
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();

            //添加一个sheet
            for (int index = 0; index < 1; index++)
            {
                ISheet sheet = book.CreateSheet($"工作簿{(index + 1)}");

                //行下标记录
                int rowIndex = 0;
                //创建首行
                IRow row0 = sheet.CreateRow(rowIndex++);
                //创建单元格
                ICell cell0 = row0.CreateCell(0);
                cell0.SetCellValue("所在区");

                ICell cell1 = row0.CreateCell(1);
                cell1.SetCellValue("租客姓名");

                ICell cell2 = row0.CreateCell(2);
                cell2.SetCellValue("联系方式");

                ICell cell3 = row0.CreateCell(3);
                cell3.SetCellValue("证件号码");

                ICell cell4 = row0.CreateCell(4);
                cell4.SetCellValue("地址");

                //ICell cell5 = row0.CreateCell(5);
                //cell5.SetCellValue("面积");

                //ICell cell6 = row0.CreateCell(6);
                //cell6.SetCellValue("户型");

                ICell cell7 = row0.CreateCell(7);
                cell7.SetCellValue("签订时间");


                ICell cell9 = row0.CreateCell(9);
                cell9.SetCellValue("起租日");

                ICell cell10 = row0.CreateCell(10);
                cell10.SetCellValue("合同结束日期");

                ICell cell11 = row0.CreateCell(11);
                cell11.SetCellValue("出租价格");

                ICell cell12 = row0.CreateCell(12);
                cell12.SetCellValue("押金");

                ICell cell13 = row0.CreateCell(13);
                cell13.SetCellValue("租期");


                ICell cell14 = row0.CreateCell(14);
                cell14.SetCellValue("交租方式");

                ICell cell15 = row0.CreateCell(15);
                cell15.SetCellValue("经办人");

                foreach (var mo in listmodel)
                {
                    var row = sheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue(mo.AreaName);
                    row.CreateCell(1).SetCellValue(mo.Name);
                    row.CreateCell(2).SetCellValue(mo.Phone);
                    row.CreateCell(3).SetCellValue(mo.Document);
                    row.CreateCell(4).SetCellValue(mo.HouseName);
                    row.CreateCell(7).SetCellValue(mo.CreateTime.ToStr());
                    row.CreateCell(9).SetCellValue(mo.BeginTime.ToStr());
                    row.CreateCell(10).SetCellValue(mo.EndTime.ToStr());
                    row.CreateCell(11).SetCellValue(mo.Recent.ToStr()+"元");
                    row.CreateCell(12).SetCellValue(mo.Deposit.ToStr()+ "元");
                    row.CreateCell(13).SetCellValue((mo.EndTime-mo.BeginTime).Days+"天");
                    row.CreateCell(14).SetCellValue(mo.Pinlv+"月一付");

                    row.CreateCell(15).SetCellValue(mo.CreatePerson);

                }
            }
            using (MemoryStream st = new MemoryStream())
            {
                book.Write(st);
                var buffer = st.GetBuffer();
                return buffer;
            }
        }
        //业主合同
        public byte[] ycontractexcel(List<WrapOwernContract> listmodel)
        {
            //创建Excel文件的对象
            HSSFWorkbook book = new HSSFWorkbook();

            //添加一个sheet
            for (int index = 0; index < 1; index++)
            {
                ISheet sheet = book.CreateSheet($"工作簿{(index + 1)}");

                //行下标记录
                int rowIndex = 0;
                //创建首行
                IRow row0 = sheet.CreateRow(rowIndex++);
                //创建单元格
                ICell cell0 = row0.CreateCell(0);
                cell0.SetCellValue("所在区");

                ICell cell1 = row0.CreateCell(1);
                cell1.SetCellValue("租客姓名");

                ICell cell2 = row0.CreateCell(2);
                cell2.SetCellValue("联系方式");

                ICell cell3 = row0.CreateCell(3);
                cell3.SetCellValue("证件号码");

                ICell cell4 = row0.CreateCell(4);
                cell4.SetCellValue("地址");

                //ICell cell5 = row0.CreateCell(5);
                //cell5.SetCellValue("面积");

                //ICell cell6 = row0.CreateCell(6);
                //cell6.SetCellValue("户型");

                ICell cell7 = row0.CreateCell(7);
                cell7.SetCellValue("签订时间");


                ICell cell9 = row0.CreateCell(9);
                cell9.SetCellValue("起租日");

                ICell cell10 = row0.CreateCell(10);
                cell10.SetCellValue("合同结束日期");

                ICell cell11 = row0.CreateCell(11);
                cell11.SetCellValue("出租价格");

                ICell cell12 = row0.CreateCell(12);
                cell12.SetCellValue("押金");

                ICell cell13 = row0.CreateCell(13);
                cell13.SetCellValue("租期");


                ICell cell14 = row0.CreateCell(14);
                cell14.SetCellValue("交租方式");

                ICell cell15 = row0.CreateCell(15);
                cell15.SetCellValue("经办人");

                foreach (var mo in listmodel)
                {
                    var row = sheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue("");
                    row.CreateCell(1).SetCellValue(mo.Name);
                    row.CreateCell(2).SetCellValue(mo.Phone);
                    row.CreateCell(3).SetCellValue("");
                    row.CreateCell(4).SetCellValue(mo.HouseName);
                    row.CreateCell(7).SetCellValue(mo.CreateTime.ToStr());
                    row.CreateCell(9).SetCellValue(mo.BeginTime.ToStr());
                    row.CreateCell(10).SetCellValue(mo.EndTime.ToStr());
                    row.CreateCell(11).SetCellValue(mo.Recent.ToStr() + "元");
                    row.CreateCell(12).SetCellValue(mo.Deposit.ToStr() + "元");
                    row.CreateCell(13).SetCellValue((mo.EndTime - mo.BeginTime).Days + "天");
                    row.CreateCell(14).SetCellValue(mo.Pinlv + "月一付");
                    row.CreateCell(15).SetCellValue(mo.CreatePerson);
                }
            }
            using (MemoryStream st = new MemoryStream())
            {
                book.Write(st);
                var buffer = st.GetBuffer();
                return buffer;
            }
        }
    }
}
