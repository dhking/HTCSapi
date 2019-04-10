using API.CommonControllers;
using Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace API
{
    [ControllerFix("Fleet")]
    public class HouseControllers: DataCenterController
    {
        HoseService service = new HoseService();
        [HttpPost]
        [ActionRouteFix("FlightPlanLineLists")]
        public List<HouseModel> Queryhouselist()
        {
            List<HouseModel> list = service.Queryhouse();
            return list;
        }
    }
}
