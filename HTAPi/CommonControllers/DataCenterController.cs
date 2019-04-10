using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.CommonControllers
{
    public class DataCenterController : RcsBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        protected void InitPage(PagingModel paging)
        {
            if (OrderablePagination != null)
            {
                InitPage(paging.PageSize, (paging.PageIndex - 1) * paging.PageSize);
            }
        }

        /// <summary>
        /// 保存数据之前初始化(lm)
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="model">实体对象</param>
        /// <param name="isAdd">是否是添加操作</param>
        //protected void SaveDataInit(string token, BasicModel model, bool isAdd)
        //{
        //    model.LastModifier = this.GetCurrentUser(token).UserCode;

        //    if (isAdd)
        //    {
        //        model.Creator = model.LastModifier;
        //    }
        //}
    }
}
